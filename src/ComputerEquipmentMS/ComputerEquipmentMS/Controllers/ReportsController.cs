using ComputerEquipmentMS.Constants;
using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.Models.Auxiliary;
using ComputerEquipmentMS.Models.Domain;
using ComputerEquipmentMS.ViewModels.ComputerConfigurations;
using ComputerEquipmentMS.ViewModels.Customers;
using ComputerEquipmentMS.ViewModels.SalePositions;
using ComputerEquipmentMS.ViewModels.Sales;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NodaTime;

namespace ComputerEquipmentMS.Controllers;

public class ReportsController : Controller
{
    private readonly IRepository<ComputerConfiguration, int> _configurationsRepository;
    private readonly IRepository<Customer, int> _customersRepository;
    private readonly NpgsqlStoredFunctionsExecutor _executor;

    public ReportsController(
        IRepository<ComputerConfiguration, int> configurationsRepository,
        IRepository<Customer, int> customersRepository,
        NpgsqlStoredFunctionsExecutor executor)
    {
        _configurationsRepository = configurationsRepository;
        _customersRepository = customersRepository;
        _executor = executor;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    [Authorize(Roles = RoleNames.Admin)]
    public IActionResult GetComputerConfigurationsSales(int configurationId, DateOnly from, DateOnly until)
    {
        SetTodayIfDatesAreMinValue(ref from, ref until);        
        
        var timeInterval = CreateTimeInterval(from, until);
        var salePositions = _executor.GetSalesOfConfigurationForTimeInterval(configurationId, timeInterval);
        var salePositionsVm = salePositions.Adapt<List<SalePositionInfoViewModel>>();

        ViewBag.From = from;
        ViewBag.Until = until;
        ViewBag.CurrentConfigurationId = configurationId;
        
        var configurations = _configurationsRepository.GetAll();
        var configurationsVm = configurations.Adapt<ComputerConfigurationsViewModel>();
        ViewBag.Configurations = configurationsVm;
        
        return View(salePositionsVm);
    }
    
    public IActionResult GetCustomersPurchases(int customerId, DateOnly from, DateOnly until)
    {
        SetTodayIfDatesAreMinValue(ref from, ref until); 
        
        var timeInterval = CreateTimeInterval(from, until);
        var sales = _executor.GetCustomersPurchases(customerId, timeInterval);
        var salesVm = sales.Adapt<SalesViewModel>();

        ViewBag.From = from;
        ViewBag.Until = until;
        ViewBag.CurrentCustomerId = customerId;
        
        var customers = _customersRepository.GetAll();
        var customersVm = customers.Adapt<CustomersViewModel>();
        ViewBag.Customers = customersVm;
        
        return View(salesVm);
    }



    private static TimeInterval CreateTimeInterval(DateOnly from, DateOnly until) =>
        new (DateOnlyToInstant(from, new TimeOnly(0, 0)), DateOnlyToInstant(until, new TimeOnly(23, 59)));

    private static Instant DateOnlyToInstant(DateOnly d, TimeOnly maxTime) => 
        Instant.FromUtc(d.Year, d.Month, d.Day, maxTime.Hour, maxTime.Minute);

    private static void SetTodayIfDatesAreMinValue(ref DateOnly from, ref DateOnly until)
    {
        if (from == DateOnly.MinValue) from = GetToday();
        if (until == DateOnly.MinValue) until = GetToday();
    }
    
    private static DateOnly GetToday() =>
        DateOnly.FromDateTime(DateTime.Today);
}
