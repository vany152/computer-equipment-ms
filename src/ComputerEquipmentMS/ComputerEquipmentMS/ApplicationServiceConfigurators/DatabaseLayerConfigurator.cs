using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.Models.Domain;

namespace ComputerEquipmentMS.ApplicationServiceConfigurators;

public static class DatabaseLayerConfigurator
{
    public static void AddConnectionString(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var connectionString = AddDefaultDbConnectionString(builder);
        services.AddSingleton(connectionString);
    }

    public static void AddNpgsqlRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IRepository<Customer, int>, CustomersNpgsqlRepository>();
        services.AddSingleton<IRepository<Sale, int>, SalesNpgsqlRepository>();
        services.AddSingleton<IRepository<SalePosition, int>, SalePositionsNpgsqlRepository>();
        services.AddSingleton<IRepository<ComputerConfiguration, int>, ComputerConfigurationsNpgsqlRepository>();
        services.AddSingleton<IRepository<Component, int>, ComponentsNpgsqlRepository>();
        services.AddSingleton<IRepository<ComponentCategory, int>, ComponentCategoriesNpgsqlRepository>();
        services.AddSingleton<IRepository<ComponentManufacturer, int>, ComponentManufacturersNpgsqlRepository>();
    }

    public static void AddNpgsqlStoredFunctionsExecutor(this IServiceCollection services) => 
        services.AddSingleton<NpgsqlStoredFunctionsExecutor>();



    private static IDbConnectionString AddDefaultDbConnectionString(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultDbConnection");
        if (connectionString is null)
            throw new NullReferenceException("Default database is not configured for this application");

        return new DbConnectionString(connectionString);
    }
}