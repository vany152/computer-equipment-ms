using System.Globalization;
using ComputerEquipmentMS;
using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.MappingService;
using ComputerEquipmentMS.Models.Domain;

SetPointAsNumberDecimalSeparator();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var builderServices = builder.Services; 
builderServices.AddControllersWithViews();
AddRepositories();
AddStoredFunctionsExecutor();
// builderServices.RegisterMapsterConfiguration();

var app = builder.Build();

RegisterServiceProviderAsStatic();
app.RegisterMapsterConfiguration();

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

app.Run();



void SetPointAsNumberDecimalSeparator()
{
    var cultureInfo = (CultureInfo) CultureInfo.CurrentCulture.Clone();
    // var cultureInfo = new CultureInfo("ru-ru");
    cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
    CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
}

void AddRepositories()
{
    AddDbConnectionString();
    builderServices.AddSingleton<IRepository<Customer, int>, CustomersNpgsqlRepository>();
    builderServices.AddSingleton<IRepository<Sale, int>, SalesNpgsqlRepository>();
    builderServices.AddSingleton<IRepository<SalePosition, int>, SalePositionsNpgsqlRepository>();
    builderServices.AddSingleton<IRepository<ComputerConfiguration, int>, ComputerConfigurationsNpgsqlRepository>();
    builderServices.AddSingleton<IRepository<Component, int>, ComponentsNpgsqlRepository>();
    builderServices.AddSingleton<IRepository<ComponentCategory, int>, ComponentCategoriesNpgsqlRepository>();
    builderServices.AddSingleton<IRepository<ComponentManufacturer, int>, ComponentManufacturersNpgsqlRepository>();
}

void AddDbConnectionString()
{
    var connectionString = GetDbConnectionString();
    builderServices.AddSingleton<IDbConnectionString>(connectionString);
}

DbConnectionString GetDbConnectionString()
{
    var connectionString = builder.Configuration.GetConnectionString("DbConnectionString");
    if (connectionString is null)
        throw new NullReferenceException("Database is not configured for this application");

    return new DbConnectionString(connectionString);
}

void AddStoredFunctionsExecutor() => 
    builderServices.AddSingleton<NpgsqlStoredFunctionsExecutor>();

void RegisterServiceProviderAsStatic() => 
    StaticServiceProvider.Services = app.Services;
