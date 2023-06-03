using System.Data;
using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.DataAccess.Entities;
using ComputerEquipmentMS.Models.Domain;
using Mapster;

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
        config
            .NewConfig<ComputerConfigurationEntity, ComputerConfiguration>()
            .Map(cc => cc.Components, entity => GetComponentsByIds(entity.ComponentIds));
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
}