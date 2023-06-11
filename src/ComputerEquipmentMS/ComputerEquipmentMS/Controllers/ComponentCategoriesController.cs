using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.Models.Domain;
using ComputerEquipmentMS.ViewModels.ComponentCategories;
using ComputerEquipmentMS.ViewModels.Components;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace ComputerEquipmentMS.Controllers;

public class ComponentCategoriesController : ControllerBase
{
    private readonly IRepository<ComponentCategory, int> _componentCategoriesRepository;
    private readonly NpgsqlStoredFunctionsExecutor _executor;

    public ComponentCategoriesController(
        IRepository<ComponentCategory, int> componentCategoriesRepository,
        NpgsqlStoredFunctionsExecutor executor,
        ILogger<ComponentCategoriesController> logger) : base(logger)
    {
        _componentCategoriesRepository = componentCategoriesRepository;
        _executor = executor;
    }
    
    

    [HttpGet]
    public IActionResult Index()
    {
        var categories = _componentCategoriesRepository.GetAll();
        var categoriesVm = categories.Adapt<ComponentCategoriesViewModel>(); 
        
        return View(categoriesVm);
    }

    
    
    [HttpGet]
    public IActionResult Details(int id)
    {
        var category = _componentCategoriesRepository.GetById(id);
        if (category is null) return HandleError($"cannot find {nameof(ComponentCategory)} with id = {id}");

        var components = _executor.GetComponentsByCategory(id);
        var componentsVm = components.Adapt<ComponentsViewModel>();
        ViewBag.Components = componentsVm; 
        
        var categoryVm = category.Adapt<ComponentCategoryViewModel>();
        return View(categoryVm);
    }
    
    
    
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Create(ComponentCategoryViewModel categoryViewModel)
    {
        var category = categoryViewModel.Adapt<ComponentCategory>();
        _componentCategoriesRepository.Add(category);

        return RedirectToAction(nameof(Index));
    }



    [HttpGet]
    public IActionResult Edit(int id)
    {
        var category = _componentCategoriesRepository.GetById(id);
        if (category is null) return HandleError($"cannot find {nameof(ComponentCategory)} with id = {id}");

        var categoryVm = category.Adapt<ComponentCategoryViewModel>();
        return View(categoryVm);
    }
    
    [HttpPost]
    public IActionResult Edit(ComponentCategoryViewModel categoryViewModel)
    {
        var category = categoryViewModel.Adapt<ComponentCategory>();
        var editResult = _componentCategoriesRepository.Edit(category);
        return editResult 
            ? RedirectToAction(nameof(Index)) 
            : HandleError($"error while editing {nameof(ComponentCategory)}: element with id {category.Id} was not edited");
    }
    
    

    [HttpPost]
    public IActionResult Remove(int id)
    {
        var removeResult = _componentCategoriesRepository.Remove(id);
        return removeResult 
            ? RedirectToAction(nameof(Index)) 
            : HandleError($"error while removing {nameof(ComponentCategory)}: element with id {id} was not removed");
    }
}