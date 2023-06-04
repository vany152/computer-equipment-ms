using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.Models.Domain;
using ComputerEquipmentMS.ViewModels.ComponentCategories;
using ComputerEquipmentMS.ViewModels.ComponentManufacturers;
using ComputerEquipmentMS.ViewModels.Components;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace ComputerEquipmentMS.Controllers;

public class ComponentsController : Controller
{
    private readonly IRepository<Component, int> _componentsRepository;
    private readonly IRepository<ComponentCategory, int> _componentCategoriesRepository;
    private readonly IRepository<ComponentManufacturer, int> _componentManufacturersRepository;
    private readonly ILogger<ComponentsController> _logger;

    public ComponentsController(
        IRepository<Component, int> componentsRepository, 
        IRepository<ComponentCategory, int> componentCategoriesRepository, 
        IRepository<ComponentManufacturer, int> componentManufacturersRepository, 
        ILogger<ComponentsController> logger)
    {
        _componentsRepository = componentsRepository;
        _componentCategoriesRepository = componentCategoriesRepository;
        _componentManufacturersRepository = componentManufacturersRepository;
        _logger = logger;
    }

    public IActionResult Index()
    {
        var components = _componentsRepository.GetAll();
        
        var componentVms = components.Adapt<List<ComponentViewModel>>();
        var componentsVm = new ComponentsViewModel(componentVms);
        
        return View(componentsVm);
    }

    
    
    [HttpGet]
    public IActionResult Create()
    {
        var componentCategories = _componentCategoriesRepository.GetAll();
        var componentManufacturers = _componentManufacturersRepository.GetAll();

        var componentCategoryVms = componentCategories.Adapt<List<ComponentCategoryViewModel>>();
        var componentManufacturerVms = componentManufacturers.Adapt<List<ComponentManufacturerViewModel>>();
        
        ViewBag.ComponentCategories = componentCategoryVms;
        ViewBag.ComponentManufacturers = componentManufacturerVms;
        
        return View();
    }

    [HttpPost]
    public IActionResult Create(ComponentViewModel componentVm)
    {
        var component = componentVm.Adapt<Component>();

        _componentsRepository.Add(component);

        return RedirectToAction(nameof(Index));
    }



    [HttpGet]
    public IActionResult Edit(int id)
    {
        var component = _componentsRepository.GetById(id);
        if (component is null)
            return NotFound();

        ViewBag.Specifications = component.Specifications;
        var componentVm = component.Adapt<ComponentViewModel>();

        var componentCategories = _componentCategoriesRepository.GetAll();
        var componentManufacturers = _componentManufacturersRepository.GetAll();

        var componentCategoryVms = componentCategories.Adapt<List<ComponentCategoryViewModel>>();
        var componentManufacturerVms = componentManufacturers.Adapt<List<ComponentManufacturerViewModel>>();
        
        ViewBag.ComponentCategories = componentCategoryVms;
        ViewBag.ComponentManufacturers = componentManufacturerVms;
        
        return View(componentVm);
    }
    
    [HttpPost]
    public IActionResult Edit(ComponentViewModel componentViewModel)
    {
        var component = componentViewModel.Adapt<Component>();

        var editResult = _componentsRepository.Edit(component);
        if (editResult)
            return RedirectToAction(nameof(Index));
        
        _logger.LogError("error while updating component: component with id {Id} was not updated", component.Id);
        return NotFound();
    }
    
    

    [HttpPost]
    public IActionResult Remove(int id)
    {
        var result = _componentsRepository.Remove(id);
        if (result)
            return RedirectToAction(nameof(Index));
        
        _logger.LogError("error while removing component: component with id {Id} was not removed", id);
        return NotFound();
    }
}