using System.Data;
using System.Text.Json;
using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.DataAccess.Entities;
using ComputerEquipmentMS.Models.Domain;
using ComputerEquipmentMS.ViewModels.Sales;
using Mapster;
using NodaTime;
using static ComputerEquipmentMS.MappingService.ConfigurationRegisters.CommonFuncs;

namespace ComputerEquipmentMS.MappingService.ConfigurationRegisters;

// ReSharper disable once UnusedType.Global - will be used on startup while IRegisters scanning
public class SaleConfigurationRegister : IRegister
{
    private readonly IRepository<SalePosition, int> _salePositionsRepository;
    private readonly IRepository<Customer, int> _customersRepository;
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
        var customersRepository = GetCustomersRepository(services);
        var executor = GetFunctionsExecutor(services);

        _salePositionsRepository = salePositionsRepository;
        _customersRepository = customersRepository;
        _functionsExecutor = executor;
    }
    
    public void Register(TypeAdapterConfig config)
    {
        // entity
        config
            .NewConfig<SaleEntity, Sale>()
            .Map(sale => sale.SalePositions, entity => GetSalePositions(entity.SalePositionIds))
            .Map(sale => sale.StartingCost, entity => _functionsExecutor.CalculateStartingSaleCost(entity.Id))
            .Map(sale => sale.Customer, entity => GetCustomer(entity.CustomerId));

        config
            .NewConfig<Sale, SaleEntity>()
            .Map(entity => entity.SalePositionIds, sale => GetSalePositionIds(sale.SalePositions))
            .Map(entity => entity.CustomerId, sale => sale.Customer.Id);
        
        // viewModel
        config
            .NewConfig<Sale, SaleInfoViewModel>()
            .Include<Sale, SaleDetailsViewModel>()
            .Map(vm => vm.FinalCost, sale => CalculateFinalCost(sale.StartingCost, sale.DiscountPercentage));

        config
            .NewConfig<CreateSaleViewModel, Sale>()
            .Map(sale => sale.Customer.Id, vm => vm.CustomerId)
            .Map(sale => sale.SalePositions, vm => CreateSalePositions(vm.ComputerConfigurationIdsWithDiscountsJson));
    }
    
    
    
    private static IRepository<SalePosition, int> GetSalePositionsRepository(IServiceProvider services)
    {
        var salePositionsRepository = services.GetService<IRepository<SalePosition, int>>();
        if (salePositionsRepository is null)
            throw new NoNullAllowedException("Repository for SalePosition was not registered as DI service");

        return salePositionsRepository;
    }
    
    private static IRepository<Customer, int> GetCustomersRepository(IServiceProvider services)
    {
        var customersRepository = services.GetService<IRepository<Customer, int>>();
        if (customersRepository is null)
            throw new NoNullAllowedException("Repository for Customers was not registered as DI service");

        return customersRepository;
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

    private Customer GetCustomer(int id)
    {
        var customer = _customersRepository.GetById(id);
        if (customer is null)
            throw new NoNullAllowedException($"could not get customer with id = {id} from repository");

        return customer;
    }

    private static ICollection<int> GetSalePositionIds(ICollection<SalePosition> salePositions) =>
        salePositions.Select(sp => sp.Id).ToList();

    private static ICollection<SalePosition> CreateSalePositions(string configurationIdsWithDiscountsJson)
    {
        var salePositions = new List<SalePosition>();
        
        var configurationIdsWithDiscounts = JsonSerializer.Deserialize<Dictionary<int, short>>(configurationIdsWithDiscountsJson);
        if (configurationIdsWithDiscounts is null)
            throw new InvalidOperationException("cannot parse json: json is not a dictionary of int : short");
            
        foreach (var (id, discount) in configurationIdsWithDiscounts)
        {
            var newSalePosition = new SalePosition
            {
                DiscountPercentage = discount, 
                WarrantyPeriod = Period.Zero, 
                Configuration = new ComputerConfiguration
                {
                    Id = id,
                    Name = string.Empty,
                    Components = new List<Component>(),
                    WarrantyPeriod = Period.Zero
                }
            };
            salePositions.Add(newSalePosition);
        }
        
        return salePositions;
    }
}
