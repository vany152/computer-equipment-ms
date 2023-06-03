using System.Reflection;
using Mapster;

namespace ComputerEquipmentMS.MappingService;

public static class MapsterConfiguration
{
    public static void RegisterMapsterConfiguration(this WebApplication app) => 
        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
}