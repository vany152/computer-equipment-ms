using System.Data;
using System.Text.Json;
using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.DataAccess.Entities;
using ComputerEquipmentMS.Models.Domain;
using ComputerEquipmentMS.ViewModels;
using Mapster;
using static ComputerEquipmentMS.MappingService.ConfigurationRegisters.CommonFuncs;

namespace ComputerEquipmentMS.MappingService.ConfigurationRegisters;

// ReSharper disable once UnusedType.Global - will be used on startup while IRegisters scanning
public class ComputerConfigurationConfigurationRegister : IRegister
{
    private readonly IRepository<Component, int> _componentsRepository;

    public ComputerConfigurationConfigurationRegister()
    {
        /*
         * Cannot use DI for repository because Mapster requires parameterless constructor 
         */
        
        var services = StaticServiceProvider.Services;
        if (services is null)
            throw new NoNullAllowedException("Service Provider was not registered as static");
        
        var componentsRepository = GetComponentsRepository(services);
        _componentsRepository = componentsRepository;
    }

    public void Register(TypeAdapterConfig config)
    {
        // entity
        config
            .NewConfig<ComputerConfigurationEntity, ComputerConfiguration>()
            .Map(cc => cc.Components, entity => GetComponentsByIds(entity.ComponentIds));

        config
            .NewConfig<ComputerConfiguration, ComputerConfigurationEntity>()
            .Map(entity => entity.ComponentIds, cc => GetComponentIds(cc.Components));

        // infoViewModel
        config
            .NewConfig<ComputerConfiguration, ComputerConfigurationInfoViewModel>()
            .Map(vm => vm.WarrantyPeriodMonths, cc => WarrantyPeriodToMonths(cc.WarrantyPeriod));
        
        config
            .NewConfig<ComputerConfigurationInfoViewModel, ComputerConfiguration>()
            .Map(cc => cc.WarrantyPeriod, vm => WarrantyPeriodFromMonths(vm.WarrantyPeriodMonths));

        // create / edit ViewModel
        config
            .NewConfig<ComputerConfiguration, CreateComputerConfigurationViewModel>()
            .Include<ComputerConfiguration, EditComputerConfigurationViewModel>()
            .Inherits<ComputerConfiguration, ComputerConfigurationInfoViewModel>()
            .Map(vm => vm.ComponentIdsJson, cc => GetComponentIdsJson(cc.Components));

        config
            .NewConfig<CreateComputerConfigurationViewModel, ComputerConfiguration>()
            .Include<EditComputerConfigurationViewModel, ComputerConfiguration>()
            .Inherits<ComputerConfigurationInfoViewModel, ComputerConfiguration>()
            .Map(cc => cc.Components, vm => GetComponentsFromIdsJson(vm.ComponentIdsJson));

        // detailsViewModel
        config
            .NewConfig<ComputerConfiguration, ComputerConfigurationDetailsViewModel>()
            .Inherits<ComputerConfiguration, ComputerConfigurationInfoViewModel>();

        config
            .NewConfig<ComputerConfigurationDetailsViewModel, ComputerConfiguration>()
            .Inherits<ComputerConfigurationInfoViewModel, ComputerConfiguration>();
    }
    
    
    
    private static IRepository<Component, int> GetComponentsRepository(IServiceProvider services)
    {
        var componentsRepository = services.GetService<IRepository<Component, int>>();
        if (componentsRepository is null)
            throw new NoNullAllowedException("Repository for Component was not registered as DI service");

        return componentsRepository;
    }
    
    private ICollection<Component> GetComponentsByIds(ICollection<int> componentIds)
    {
        var components = new List<Component>();

        if (!componentIds.Any())
            return components;
        
        foreach (var id in componentIds)
        {
            var component = _componentsRepository.GetById(id);
            if (component is null)
                throw new NoNullAllowedException($"could not get component with id = {id} from repository");
            
            components.Add(component);
        }

        return components;
    }

    private static ICollection<int> GetComponentIds(ICollection<Component> components) =>
        components.Select(c => c.Id).ToList();

    private static string GetComponentIdsJson(ICollection<Component> components)
    {
        var ids = GetComponentIds(components);
        var json = JsonSerializer.Serialize(ids);

        return json;
    }

    private ICollection<Component> GetComponentsFromIdsJson(string json)
    {
        var ids = JsonSerializer.Deserialize<ICollection<int>>(json);
        if (ids is null)
            throw new InvalidOperationException("cannot parse json: json is not a collection of int");
        
        var components = GetComponentsByIds(ids);
        return components;
    }
}
