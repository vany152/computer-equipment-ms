using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.Models.Domain;
using ComputerEquipmentMS.ViewModels.ComponentCategories;
using ComputerEquipmentMS.ViewModels.ComponentManufacturers;
using ComputerEquipmentMS.ViewModels.Components;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace ComputerEquipmentMS.Controllers;

public class ComponentManufacturersController : ControllerBase
{
    private readonly IRepository<ComponentManufacturer, int> _componentManufacturersRepository;
    private readonly NpgsqlStoredFunctionsExecutor _executor;

    public ComponentManufacturersController(
        IRepository<ComponentManufacturer, int> componentManufacturersRepository,
        NpgsqlStoredFunctionsExecutor executor,
        ILogger<ComponentManufacturersController> logger) : base(logger)
    {
        _componentManufacturersRepository = componentManufacturersRepository;
        _executor = executor;
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
        if (manufacturer is null) return HandleError($"cannot find {nameof(ComponentManufacturer)} with id = {id}");

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
        return editResult 
            ? RedirectToAction(nameof(Index)) 
            : HandleError($"error while editing {nameof(ComponentManufacturer)}: element with id {manufacturer.Id} was not edited");
    }
    
    

    [HttpPost]
    public IActionResult Remove(int id)
    {
        var removeResult = _componentManufacturersRepository.Remove(id);
        return removeResult 
            ? RedirectToAction(nameof(Index)) 
            : HandleError($"error while removing {nameof(ComponentManufacturer)}: element with id {id} was not removing");
    }
}