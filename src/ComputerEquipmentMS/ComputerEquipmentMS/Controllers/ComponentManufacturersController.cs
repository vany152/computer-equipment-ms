using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.Models.Domain;
using ComputerEquipmentMS.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace ComputerEquipmentMS.Controllers;

public class ComponentManufacturersController : Controller
{
    private readonly IRepository<ComponentManufacturer, int> _componentManufacturersRepository;
    private readonly NpgsqlStoredFunctionsExecutor _executor;
    private readonly ILogger<ComponentManufacturersController> _logger;

    public ComponentManufacturersController(
        IRepository<ComponentManufacturer, int> componentManufacturersRepository,
        NpgsqlStoredFunctionsExecutor executor,
        ILogger<ComponentManufacturersController> logger)
    {
        _componentManufacturersRepository = componentManufacturersRepository;
        _executor = executor;
        _logger = logger;
    }
    
    

    [HttpGet]
    public IActionResult Index()
    {
        var manufacturers = _componentManufacturersRepository.GetAll();
        var manufacturersVm = manufacturers.Adapt<ComponentManufacturersViewModel>(); 
        
        return View(manufacturersVm);
    }

    
    
    [HttpGet]
    public IActionResult Details(int id)
    {
        var manufacturer = _componentManufacturersRepository.GetById(id);
        if (manufacturer is null) return NotFound();

        var components = _executor.GetComponentsByManufacturer(id);
        var componentsVm = components.Adapt<ComponentsViewModel>();
        ViewBag.Components = componentsVm; 
        
        var manufacturerVm = manufacturer.Adapt<ComponentCategoryViewModel>();
        return View(manufacturerVm);
    }
    
    
    
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Create(ComponentManufacturerViewModel manufacturerViewModel)
    {
        var manufacturer = manufacturerViewModel.Adapt<ComponentManufacturer>();
        _componentManufacturersRepository.Add(manufacturer);

        return RedirectToAction(nameof(Index));
    }



    [HttpGet]
    public IActionResult Edit(int id)
    {
        var manufacturer = _componentManufacturersRepository.GetById(id);
        if (manufacturer is null) return NotFound();

        var manufacturerVm = manufacturer.Adapt<ComponentManufacturerViewModel>();
        return View(manufacturerVm);
    }
    
    [HttpPost]
    public IActionResult Edit(ComponentManufacturerViewModel manufacturerViewModel)
    {
        var manufacturer = manufacturerViewModel.Adapt<ComponentManufacturer>();
        var editResult = _componentManufacturersRepository.Edit(manufacturer);
        if (editResult) return RedirectToAction(nameof(Index));
        
        _logger.LogError("error while editing component manufacturer: component manufacturer with id {Id} was not edited", manufacturer.Id);
        return NotFound();
    }
    
    

    [HttpPost]
    public IActionResult Remove(int id)
    {
        var removeResult = _componentManufacturersRepository.Remove(id);
        if (removeResult) return RedirectToAction(nameof(Index));
        
        _logger.LogError("error while removing component manufacturer: component manufacturer with id {Id} was not removed", id);
        return NotFound();
    }
}