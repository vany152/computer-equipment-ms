using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.Models.Domain;
using ComputerEquipmentMS.ViewModels.ComponentCategories;
using ComputerEquipmentMS.ViewModels.ComponentManufacturers;
using ComputerEquipmentMS.ViewModels.Components;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace ComputerEquipmentMS.Controllers;

public class ComponentsController : ControllerBase
{
    private readonly IRepository<Component, int> _componentsRepository;
    private readonly IRepository<ComponentCategory, int> _componentCategoriesRepository;
    private readonly IRepository<ComponentManufacturer, int> _componentManufacturersRepository;

    public ComponentsController(
        IRepository<Component, int> componentsRepository, 
        IRepository<ComponentCategory, int> componentCategoriesRepository, 
        IRepository<ComponentManufacturer, int> componentManufacturersRepository, 
        ILogger<ComponentsController> logger) : base(logger)
    {
        _componentsRepository = componentsRepository;
        _componentCategoriesRepository = componentCategoriesRepository;
        _componentManufacturersRepository = componentManufacturersRepository;
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
        if (component is null) return HandleError($"cannot find {nameof(Component)} with id = {id}");

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
        return editResult 
            ? RedirectToAction(nameof(Index)) 
            : HandleError($"error while updating {nameof(Component)}: element with id {component.Id} was not updated");
    }
    
    

    [HttpPost]
    public IActionResult Remove(int id)
    {
        var result = _componentsRepository.Remove(id);
        return result 
            ? RedirectToAction(nameof(Index)) 
            : HandleError($"error while removing {nameof(Component)}: element with id {id} was not removing");
    }
}