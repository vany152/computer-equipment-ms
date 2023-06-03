using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.Models.Domain;
using ComputerEquipmentMS.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace ComputerEquipmentMS.Controllers;

public class ConfigurationsController : Controller
{
    private readonly IRepository<ComputerConfiguration, int> _configurationRepository;
    private readonly IRepository<Component, int> _componentRepository;
    private readonly ILogger<ConfigurationsController> _logger;

    public ConfigurationsController(
        IRepository<ComputerConfiguration, int> configurationRepository,
        IRepository<Component, int> componentRepository,
        ILogger<ConfigurationsController> logger)
    {
        _configurationRepository = configurationRepository;
        _logger = logger;
        _componentRepository = componentRepository;
    }

    public IActionResult Index()
    {
        var configurations = _configurationRepository.GetAll();
        
        var configurationVms = configurations.Adapt<List<ComputerConfigurationDetailsViewModel>>();
        var configurationsVm = new ComputerConfigurationsViewModel(configurationVms);
        
        return View(configurationsVm);
    }



    [HttpGet]
    public IActionResult Create()
    {
        var components = _componentRepository.GetAll();
        var componentVms = components.Adapt<ComponentsViewModel>();
        ViewBag.Components = componentVms;
        
        return View();
    }
    
    [HttpPost]
    public IActionResult Create(CreateComputerConfigurationViewModel configurationViewModel)
    {
        var configuration = configurationViewModel.Adapt<ComputerConfiguration>(); // todo adapt
        _configurationRepository.Add(configuration);

        return RedirectToAction(nameof(Index));
    }

    
    
    [HttpGet]
    public IActionResult Details(int id)
    {
        var config = _configurationRepository.GetById(id);
        if (config is null)
            return NotFound();
        
        var configViewModel = config.Adapt<ComputerConfigurationDetailsViewModel>();
        return View(configViewModel);
    }

    
    
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var configuration = _configurationRepository.GetById(id);
        if (configuration is null)
            return NotFound();
        
        var components = _componentRepository.GetAll();
        var componentVms = components.Adapt<ComponentsViewModel>();
        ViewBag.Components = componentVms;
        
        var configurationVm = configuration.Adapt<EditComputerConfigurationViewModel>();
        return View(configurationVm);
    }

    [HttpPost]
    public IActionResult Edit(EditComputerConfigurationViewModel configViewModel)
    {
        var configuration = configViewModel.Adapt<ComputerConfiguration>();
        _configurationRepository.Edit(configuration);
        return RedirectToAction(nameof(Index));
    }



    public IActionResult Remove(int id)
    {
        var removeResult = _configurationRepository.Remove(id);
        if (removeResult)
            return RedirectToAction(nameof(Index));

        _logger.LogError("error while removing computer configuration: computer configuration with id {Id} was not removed", id);
        return NotFound();
    }
}