using System.Data;
using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.DataAccess.Entities;
using ComputerEquipmentMS.Models.Domain;
using ComputerEquipmentMS.ViewModels;
using Mapster;
using NodaTime;
using static ComputerEquipmentMS.MappingService.ConfigurationRegisters.CommonFuncs;

namespace ComputerEquipmentMS.MappingService.ConfigurationRegisters;

// ReSharper disable once UnusedType.Global - will be used on startup while IRegisters scanning
public class SalePositionConfigurationRegister : IRegister
{
    private readonly IRepository<ComputerConfiguration, int> _configurationRepository;

    public SalePositionConfigurationRegister()
    {
        /*
         * Cannot use DI for repository because Mapster requires parameterless constructor 
         */
        var services = StaticServiceProvider.Services;
        if (services is null)
            throw new NoNullAllowedException("Service Provider was not registered as static");
        
        var configurationRepository = GetConfigurationRepository(services);

        _configurationRepository = configurationRepository;
    }
    
    
    
    public void Register(TypeAdapterConfig config)
    {
        // entity
        config
            .NewConfig<SalePositionEntity, SalePosition>()
            .Map(sp => sp.Configuration, entity => GetComputerConfigurationById(entity.ConfigurationId));
        config
            .NewConfig<SalePosition, SalePositionDetailsViewModel>()
            .Map(vm => vm.StartingCost, sp => sp.Cost)
            .Map(vm => vm.FinalCost, sp => CalculateFinalCost(sp.Cost, sp.DiscountPercentage));
        
        // viewModelBase & createViewModel
        config
            .NewConfig<SalePosition, SalePositionViewModelBase>()
            .Include<SalePosition, CreateSalePositionViewModel>()
            .Map(vm => vm.ComputerConfigurationId, sp => sp.Configuration.Id);
        
        config
            .NewConfig<SalePositionViewModelBase, SalePosition>()
            .Include<CreateSalePositionViewModel, SalePosition>()
            .Map(sp => sp.Configuration, vm => CreateEmptyConfigurationFromId(vm.ComputerConfigurationId));        

        // detailsViewModel
        config
            .NewConfig<SalePosition, SalePositionDetailsViewModel>()
            .Inherits<SalePosition, SalePositionViewModelBase>()
            .Map(vm => vm.ComputerConfigurationName, sp => sp.Configuration.Name)
            .Map(vm => vm.StartingCost, sp => sp.Cost)
            .Map(vm => vm.FinalCost, sp => CommonFuncs.CalculateFinalCost(sp.Cost, sp.DiscountPercentage));

        config
            .NewConfig<SalePositionDetailsViewModel, SalePosition>()
            .Inherits<SalePositionViewModelBase, SalePosition>()
            .Map(sp => sp.Configuration.Name, vm => vm.ComputerConfigurationName)
            .Map(sp => sp.Cost, vm => vm.StartingCost);
    }



    private static IRepository<ComputerConfiguration, int> GetConfigurationRepository(IServiceProvider services)
    {
        var configurationRepository = services.GetService<IRepository<ComputerConfiguration, int>>();
        if (configurationRepository is null)
            throw new NoNullAllowedException("Repository for ComputerConfiguration was not registered as DI service");

        return configurationRepository;
    }

    private ComputerConfiguration GetComputerConfigurationById(int id)
    {
        var configuration = _configurationRepository.GetById(id);
        if (configuration is null)
            throw new NoNullAllowedException($"could not get computer configuration with id = {id}");

        return configuration;
    }
    


    private static ComputerConfiguration CreateEmptyConfigurationFromId(int id) =>
        new()
        {
            Id = id,
            Name = string.Empty,
            WarrantyPeriod = Period.Zero,
            Components = Array.Empty<Component>()
        };
}
