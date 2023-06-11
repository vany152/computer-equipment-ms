using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.Models.Domain;
using ComputerEquipmentMS.ViewModels.ComputerConfigurations;
using ComputerEquipmentMS.ViewModels.Customers;
using ComputerEquipmentMS.ViewModels.Sales;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using NodaTime;

namespace ComputerEquipmentMS.Controllers;

public class SalesController : ControllerBase
{
    private readonly IRepository<Sale, int> _salesRepository;
    private readonly IRepository<ComputerConfiguration, int> _configurationsRepository;
    private readonly IRepository<Customer, int> _customersRepository;
    private readonly NpgsqlStoredFunctionsExecutor _executor;

    public SalesController(
        IRepository<Sale, int> salesRepository,
        IRepository<ComputerConfiguration, int> configurationsRepository,
        IRepository<Customer, int> customersRepository,
        NpgsqlStoredFunctionsExecutor executor,
        ILogger<SalesController> logger) : base(logger)
    {
        _salesRepository = salesRepository;
        _configurationsRepository = configurationsRepository;
        _customersRepository = customersRepository;
        _executor = executor;
    }
    
    public IActionResult Index()
    {
        var sales = _salesRepository.GetAll();
        
        var saleVms = sales.Adapt<List<SaleDetailsViewModel>>();
        var salesVm = new SalesViewModel(saleVms);
        
        return View(salesVm);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var sale = _salesRepository.GetById(id);
        if (sale is null) return HandleError($"cannot find {nameof(Sale)} with id = {id}");

            var saleVm = sale.Adapt<SaleDetailsViewModel>();
        return View(saleVm);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var computerConfigurations = _configurationsRepository.GetAll();
        var computerConfigurationsInfo = computerConfigurations.Adapt<List<ComputerConfigurationInfoViewModel>>(); 
        ViewBag.ComputerConfigurations = computerConfigurationsInfo;

        var customers = _customersRepository.GetAll();
        var customersVm = customers.Adapt<CustomersViewModel>();
        ViewBag.Customers = customersVm;
        
        return View();
    }

    [HttpPost]
    public IActionResult Create(CreateSaleViewModel saleViewModel)
    {
        var sale = saleViewModel.Adapt<Sale>();
        sale.SaleMoment = Instant.FromDateTimeUtc(DateTime.UtcNow);
        _executor.CreateSale(sale);
            
        return RedirectToAction(nameof(Index));
    }
}
