using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.Models.Domain;
using ComputerEquipmentMS.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace ComputerEquipmentMS.Controllers;

public class ComponentCategoriesController : Controller
{
    private readonly IRepository<ComponentCategory, int> _componentCategoriesRepository;
    private readonly NpgsqlStoredFunctionsExecutor _executor;
    private readonly ILogger<ComponentCategoriesController> _logger;

    public ComponentCategoriesController(
        IRepository<ComponentCategory, int> componentCategoriesRepository,
        NpgsqlStoredFunctionsExecutor executor,
        ILogger<ComponentCategoriesController> logger)
    {
        _componentCategoriesRepository = componentCategoriesRepository;
        _executor = executor;
        _logger = logger;
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
        if (category is null) return NotFound();

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
        if (category is null) return NotFound();

        var categoryVm = category.Adapt<ComponentCategoryViewModel>();
        return View(categoryVm);
    }
    
    [HttpPost]
    public IActionResult Edit(ComponentCategoryViewModel categoryViewModel)
    {
        var category = categoryViewModel.Adapt<ComponentCategory>();
        var editResult = _componentCategoriesRepository.Edit(category);
        if (editResult) return RedirectToAction(nameof(Index));
        
        _logger.LogError("error while editing component category: component category with id {Id} was not edited", category.Id);
        return NotFound();
    }
    
    

    [HttpPost]
    public IActionResult Remove(int id)
    {
        var removeResult = _componentCategoriesRepository.Remove(id);
        if (removeResult) return RedirectToAction(nameof(Index));
        
        _logger.LogError("error while removing component category: component category with id {Id} was not removed", id);
        return NotFound();
    }
}