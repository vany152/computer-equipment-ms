using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.Models.Domain;
using ComputerEquipmentMS.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace ComputerEquipmentMS.Controllers;

public class ConfigurationsController : Controller
{
    private readonly IRepository<ComputerConfiguration, int> _configurationRepository;

    public ConfigurationsController(IRepository<ComputerConfiguration, int> configurationRepository)
    {
        _configurationRepository = configurationRepository;
    }

    public IActionResult Index()
    {
        var configurations = _configurationRepository.GetAll();
        
        var configurationVms = configurations.Adapt<List<ComputerConfigurationViewModel>>();
        var configurationsVm = new ComputerConfigurationsViewModel(configurationVms);
        
        return View(configurationsVm);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var config = _configurationRepository.GetById(id);
        if (config is null)
            return NotFound();
        
        var configViewModel = config.Adapt<ComputerConfigurationViewModel>();
        return View(configViewModel);
    }

    public IActionResult Create()
    {
        throw new NotImplementedException();
    }
}