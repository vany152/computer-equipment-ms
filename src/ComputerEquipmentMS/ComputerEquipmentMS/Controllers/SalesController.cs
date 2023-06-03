using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.Models.Domain;
using ComputerEquipmentMS.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace ComputerEquipmentMS.Controllers;

public class SalesController : Controller
{
    private readonly IRepository<Sale, int> _salesRepository;
    private readonly ILogger<SalesController> _logger;

    public SalesController(IRepository<Sale, int> salesRepository, ILogger<SalesController> logger)
    {
        _salesRepository = salesRepository;
        _logger = logger;
    }
    
    public IActionResult Index()
    {
        var sales = _salesRepository.GetAll();
        
        var saleVms = sales.Adapt<List<SaleViewModel>>();
        var salesVm = new SalesViewModel(saleVms);
        
        return View(salesVm);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var sale = _salesRepository.GetById(id);
        if (sale is null)
            return NotFound();

        var saleVm = sale.Adapt<SaleViewModel>();
        return View(saleVm);
    }

    public IActionResult Create()
    {
        throw new NotImplementedException();
    }
}