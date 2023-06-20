using System.Data;
using System.Text;
using System.Text.Json;
using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.DataAccess.Entities;
using ComputerEquipmentMS.Models.Auxiliary;
using ComputerEquipmentMS.Models.Domain;
using ComputerEquipmentMS.ViewModels.Components;
using Mapster;
using static ComputerEquipmentMS.MappingService.ConfigurationRegisters.CommonFuncs;

namespace ComputerEquipmentMS.MappingService.ConfigurationRegisters;

// ReSharper disable once UnusedType.Global - will be used on startup while IRegisters scanning
public class ComponentConfigurationRegister : IRegister
{
    private readonly IRepository<ComponentManufacturer, int> _manufacturersRepository;
    private readonly IRepository<ComponentCategory, int> _categoriesRepository;

    public ComponentConfigurationRegister()
    {
        /*
         * Cannot use DI for repository because Mapster requires parameterless constructor 
         */
        
        var services = StaticServiceProvider.Services;
        if (services is null)
            throw new NoNullAllowedException("Service Provider was not registered as static");

        var categoriesRepository = GetCategoriesRepository(services);
        var manufacturersRepository = GetManufacturersRepository(services);

        _categoriesRepository = categoriesRepository;
        _manufacturersRepository = manufacturersRepository;
    }
    
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<ComponentEntity, Component>()
            .Map(c => c.ComponentCategory, entity => GetComponentCategoryById(entity.ComponentCategoryId))
            .Map(c => c.ComponentManufacturer, entity => GetComponentManufacturerById(entity.ComponentManufacturerId));

        config
            .NewConfig<Component, ComponentEntity>()
            .Map(entity => entity.ComponentCategoryId, c => c.ComponentCategory.Id)
            .Map(entity => entity.ComponentManufacturerId, c => c.ComponentManufacturer.Id);

        config
            .NewConfig<Component, ComponentViewModel>()
            .Map(vm => vm.ComponentCategoryId, c => c.ComponentCategory.Id)
            .Map(vm => vm.ComponentCategory, c => c.ComponentCategory.Name)
            .Map(vm => vm.ComponentManufacturerId, c => c.ComponentManufacturer.Id)
            .Map(vm => vm.ComponentManufacturer, c => c.ComponentManufacturer.Name)
            .Map(vm => vm.WarrantyPeriodMonths, c => WarrantyPeriodToMonths(c.WarrantyPeriod))
            .Map(vm => vm.Specifications, c => SerializeSpecifications(c.Specifications));

        config
            .NewConfig<ComponentViewModel, Component>()
            .Map(c => c.ComponentCategory, vm => CreateComponentCategory(vm.ComponentCategoryId, vm.ComponentCategory))
            .Map(c => c.ComponentManufacturer, vm => CreateComponentManufacturer(vm.ComponentManufacturerId, vm.ComponentManufacturer))
            .Map(c => c.WarrantyPeriod, vm => WarrantyPeriodFromMonths(vm.WarrantyPeriodMonths))
            .Map(c => c.Specifications, vm => DeserializeSpecifications(vm.Specifications));
    }
    
    
    
    private static IRepository<ComponentCategory, int> GetCategoriesRepository(IServiceProvider services)
    {
        var categoriesRepository = services.GetService<IRepository<ComponentCategory, int>>();
        if (categoriesRepository is null)
            throw new NoNullAllowedException("Repository for ComponentCategory was not registered as DI service");

        return categoriesRepository;
    }
    
    private static IRepository<ComponentManufacturer, int> GetManufacturersRepository(IServiceProvider services)
    {
        var manufacturersRepository = services.GetService<IRepository<ComponentManufacturer, int>>();
        if (manufacturersRepository is null)
            throw new NoNullAllowedException("Repository for ComponentManufacturers was not registered as DI service");

        return manufacturersRepository;
    }

    private ComponentCategory GetComponentCategoryById(int id)
    {
        var category = _categoriesRepository.GetById(id);
        if (category is null)
            throw new NoNullAllowedException($"could not get component category with id = {id} from repository");

        return category;
    }
    
    private ComponentManufacturer GetComponentManufacturerById(int id)
    {
        var manufacturer = _manufacturersRepository.GetById(id);
        if (manufacturer is null)
            throw new NoNullAllowedException($"could not get component manufacturer with id = {id} from repository");

        return manufacturer;
    }

    private static ComponentManufacturer CreateComponentManufacturer(int id, string name) =>
        new() { Id = id, Name = name };
    
    private static ComponentCategory CreateComponentCategory(int id, string name) => 
        new() { Id = id, Name = name };

    private static string SerializeSpecifications(ComponentSpecifications specifications)
    {
        var str = new StringBuilder();
        foreach (var (type, value) in specifications)
            str.AppendLine($"{type}: {value}");

        return str.ToString();
    }

    private static ComponentSpecifications DeserializeSpecifications(string specificationsJson) =>
        JsonSerializer.Deserialize<ComponentSpecifications>(specificationsJson) ?? new ComponentSpecifications();
}