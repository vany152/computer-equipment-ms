using System.Globalization;
using ComputerEquipmentMS;
using ComputerEquipmentMS.ApplicationServiceConfigurators;

SetPointAsNumberDecimalSeparator();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
services.AddControllersWithViews(options => options.Conventions.Add(new AddAuthorizeFiltersControllerConvention()));

services.AddConnectionString(builder); 
services.AddNpgsqlRepositories();
services.AddNpgsqlStoredFunctionsExecutor();

services.AddAuth();

var app = builder.Build();

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
app.MapControllerRoute(
    name: "auth",
    pattern: "{controller=Auth}/{action=Login}");

app.Run();



void SetPointAsNumberDecimalSeparator()
{
    var cultureInfo = (CultureInfo) CultureInfo.CurrentCulture.Clone();
    cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
    CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
}
