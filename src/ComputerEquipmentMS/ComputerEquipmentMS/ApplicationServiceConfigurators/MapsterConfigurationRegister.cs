using System.Reflection;
using Mapster;

namespace ComputerEquipmentMS.ApplicationServiceConfigurators;

public static class MapsterConfigurationRegister
{
    public static void RegisterMapsterConfiguration(this WebApplication app)
    {
        StaticServiceProvider.Services = app.Services;
        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
}