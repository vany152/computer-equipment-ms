using System.Globalization;
using System.Reflection;
using ComputerEquipmentMS;
using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.Models.Domain;
using Mapster;
using Microsoft.AspNetCore.Authentication.Cookies;

SetPointAsNumberDecimalSeparator();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var builderServices = builder.Services; 
builderServices.AddControllersWithViews(options => options.Conventions.Add(new AddAuthorizeFiltersControllerConvention()));
AddAuth();
AddRepositories();
AddStoredFunctionsExecutor();

var app = builder.Build();

RegisterServiceProviderAsStatic();
RegisterMapsterConfiguration();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

MapControllers();

app.Run();



void SetPointAsNumberDecimalSeparator()
{
    var cultureInfo = (CultureInfo) CultureInfo.CurrentCulture.Clone();
    cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
    CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
}

void AddAuth()
{
    builderServices.AddAuthorization(options => options.AddPolicy("defaultPolicy", policy => policy.RequireAuthenticatedUser()));
    builderServices
        .AddAuthentication(options => options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options => options.LoginPath = "/Auth/Login");
}

void AddRepositories()
{
    AddDefaultDbConnectionString();
    builderServices.AddSingleton<IRepository<Customer, int>, CustomersNpgsqlRepository>();
    builderServices.AddSingleton<IRepository<Sale, int>, SalesNpgsqlRepository>();
    builderServices.AddSingleton<IRepository<SalePosition, int>, SalePositionsNpgsqlRepository>();
    builderServices.AddSingleton<IRepository<ComputerConfiguration, int>, ComputerConfigurationsNpgsqlRepository>();
    builderServices.AddSingleton<IRepository<Component, int>, ComponentsNpgsqlRepository>();
    builderServices.AddSingleton<IRepository<ComponentCategory, int>, ComponentCategoriesNpgsqlRepository>();
    builderServices.AddSingleton<IRepository<ComponentManufacturer, int>, ComponentManufacturersNpgsqlRepository>();
}

void AddDefaultDbConnectionString()
{
    var connectionString = DefaultDbConnectionString();
    builderServices.AddSingleton(connectionString);
}

IDbConnectionString DefaultDbConnectionString()
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultDbConnection");
    if (connectionString is null)
        throw new NullReferenceException("Default database is not configured for this application");

    return new DbConnectionString(connectionString);
}

void AddStoredFunctionsExecutor() => 
    builderServices.AddSingleton<NpgsqlStoredFunctionsExecutor>();

void RegisterServiceProviderAsStatic() => 
    StaticServiceProvider.Services = app.Services;

void RegisterMapsterConfiguration() =>
    TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

void MapControllers()
{
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Customers}/{action=Index}/{id?}/");
    app.MapControllerRoute(
        name: "sales",
        pattern: "{controller=Sales}/{action=Index}/{id?}/");
    app.MapControllerRoute(
        name: "configurations",
        pattern: "{controller=Configurations}/{action=Index}/{id?}/");
    app.MapControllerRoute(
        name: "components",
        pattern: "{controller=Components}/{action=Index}/{id?}/");
    app.MapControllerRoute(
        name: "auth",
        pattern: "{controller=Auth}/{action=Login}");
}
