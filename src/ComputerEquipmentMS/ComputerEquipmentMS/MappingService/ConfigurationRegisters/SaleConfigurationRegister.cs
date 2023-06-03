using System.Data;
using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.DataAccess.Entities;
using ComputerEquipmentMS.Models.Domain;
using ComputerEquipmentMS.ViewModels;
using Mapster;

namespace ComputerEquipmentMS.MappingService.ConfigurationRegisters;

// ReSharper disable once UnusedType.Global - will be used on startup while IRegisters scanning
public class SaleConfigurationRegister : IRegister
{
    private readonly IRepository<SalePosition, int> _salePositionsRepository;
    private readonly NpgsqlStoredFunctionsExecutor _functionsExecutor;

    public SaleConfigurationRegister()
    {
        // todo remove static StaticServiceProvider
        /*
         * Cannot use DI for repository because Mapster requires parameterless constructor 
         */
        
        var services = StaticServiceProvider.Services;
        if (services is null)
            throw new NoNullAllowedException("Service Provider was not registered as static");
        
        var salePositionsRepository = GetSalePositionsRepository(services);
        var executor = GetFunctionsExecutor(services);

        _salePositionsRepository = salePositionsRepository;
        _functionsExecutor = executor;
    }
    
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<SaleEntity, Sale>()
            .Map(sale => sale.SalePositions, entity => GetSalePositions(entity.SalePositionIds))
            .Map(sale => sale.StartingCost, entity => _functionsExecutor.CalculateSaleCost(entity.Id));

        config
            .NewConfig<Sale, SaleViewModel>()
            .Map(viewModel => viewModel.FinalCost, sale => CalculateFinalCost(sale.StartingCost, sale.DiscountPercentage));
    }
    
    
    
    private static IRepository<SalePosition, int> GetSalePositionsRepository(IServiceProvider services)
    {
        var salePositionsRepository = services.GetService<IRepository<SalePosition, int>>();
        if (salePositionsRepository is null)
            throw new NoNullAllowedException("Repository for SalePosition was not registered as DI service");

        return salePositionsRepository;
    }

    private static NpgsqlStoredFunctionsExecutor GetFunctionsExecutor(IServiceProvider services)
    {
        var executor = services.GetService<NpgsqlStoredFunctionsExecutor>();
        if (executor is null)
            throw new NoNullAllowedException("NpgsqlStoredFunctionsExecutor was not registered as DI service");

        return executor;
    }
    
    private ICollection<SalePosition> GetSalePositions(IEnumerable<int>? salePositionIds)
    {
        var salePositions = new List<SalePosition>();

        if (salePositionIds is null)
            return salePositions;
        
        foreach (var id in salePositionIds)
        {
            var salePosition = _salePositionsRepository.GetById(id);
            if (salePosition is null)
                throw new NoNullAllowedException($"could not get sale position with id = {id} from repository");
            
            salePositions.Add(salePosition);
        }

        return salePositions;
    }

    private static decimal CalculateFinalCost(decimal startingCost, short discountPercentage) =>
        startingCost - (startingCost * discountPercentage) / 100;
}
