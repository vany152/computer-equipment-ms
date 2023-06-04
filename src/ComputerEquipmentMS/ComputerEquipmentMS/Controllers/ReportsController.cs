using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.Models.Auxiliary;
using ComputerEquipmentMS.Models.Domain;
using ComputerEquipmentMS.ViewModels.ComputerConfigurations;
using ComputerEquipmentMS.ViewModels.SalePositions;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using NodaTime;

namespace ComputerEquipmentMS.Controllers;

public class ReportsController : Controller
{
    private readonly IRepository<ComputerConfiguration, int> _configurationsRepository;
    private readonly NpgsqlStoredFunctionsExecutor _executor;

    public ReportsController(
        IRepository<ComputerConfiguration, int> configurationsRepository,
        NpgsqlStoredFunctionsExecutor executor)
    {
        _configurationsRepository = configurationsRepository;
        _executor = executor;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult GetComputerConfigurationsSales(int configurationId, DateOnly from, DateOnly until)
    {
        if (from == DateOnly.MinValue) from = GetToday();
        if (until == DateOnly.MinValue) until = GetToday();
        
        var timeInterval = new TimeInterval(DateOnlyToInstant(from), DateOnlyToInstant(until));
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
    
    public IActionResult GetCustomersSales(DateOnly from, DateOnly to)
    {
        throw new NotImplementedException();
    }



    private static Instant DateOnlyToInstant(DateOnly d) => 
        Instant.FromUtc(d.Year, d.Month, d.Day, 0, 0);
    
    private static DateOnly GetToday() =>
        DateOnly.FromDateTime(DateTime.Today);
}
