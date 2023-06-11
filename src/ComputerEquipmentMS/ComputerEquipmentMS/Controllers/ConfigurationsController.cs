using ComputerEquipmentMS.Constants;
using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.Models.Domain;
using ComputerEquipmentMS.ViewModels.Components;
using ComputerEquipmentMS.ViewModels.ComputerConfigurations;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComputerEquipmentMS.Controllers;

public class ConfigurationsController : ControllerBase
{
    private readonly IRepository<ComputerConfiguration, int> _configurationRepository;
    private readonly IRepository<Component, int> _componentRepository;

    public ConfigurationsController(
        IRepository<ComputerConfiguration, int> configurationRepository,
        IRepository<Component, int> componentRepository,
        ILogger<ConfigurationsController> logger) : base(logger)
    {
        _configurationRepository = configurationRepository;
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
        var configuration = configurationViewModel.Adapt<ComputerConfiguration>();
        _configurationRepository.Add(configuration);

        return RedirectToAction(nameof(Index));
    }

    
    
    [HttpGet]
    public IActionResult Details(int id)
    {
        var config = _configurationRepository.GetById(id);
        if (config is null) return HandleError($"cannot find {nameof(ComputerConfiguration)} category with id = {id}");
        
        var configViewModel = config.Adapt<ComputerConfigurationDetailsViewModel>();
        return View(configViewModel);
    }

    
    
    [HttpGet]
    [Authorize(Roles = RoleNames.Admin)]
    public IActionResult Edit(int id)
    {
        var configuration = _configurationRepository.GetById(id);
        if (configuration is null) return HandleError($"error while updating {nameof(ComputerConfiguration)}: element with id {id} was not updated");

        var components = _componentRepository.GetAll();
        var componentVms = components.Adapt<ComponentsViewModel>();
        ViewBag.Components = componentVms;
        
        var configurationVm = configuration.Adapt<EditComputerConfigurationViewModel>();
        return View(configurationVm);
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.Admin)]
    public IActionResult Edit(EditComputerConfigurationViewModel configViewModel)
    {
        var configuration = configViewModel.Adapt<ComputerConfiguration>();
        var editResult = _configurationRepository.Edit(configuration);

        return editResult
            ? RedirectToAction(nameof(Index))
            : HandleError($"error while editing {nameof(ComputerConfiguration)}: element with id {configuration.Id} was not edited");
    }


    [HttpPost]
    [Authorize(Roles = RoleNames.Admin)]
    public IActionResult Remove(int id)
    {
        var removeResult = _configurationRepository.Remove(id);
        return removeResult 
            ? RedirectToAction(nameof(Index)) 
            : HandleError($"error while removing {nameof(ComputerConfiguration)}: element with id {id} was not removed");
    }
}