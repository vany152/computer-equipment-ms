using Microsoft.AspNetCore.Mvc;

namespace ComputerEquipmentMS.Controllers;

public class ControllerBase : Controller
{
    private readonly ILogger _logger;

    protected ControllerBase(ILogger logger)
    {
        _logger = logger;
    }

    protected IActionResult HandleError(string? errorMessage)
    {
        if (!string.IsNullOrEmpty(errorMessage))
            _logger.LogError(errorMessage);

        return RedirectToAction("Error", "Error");
    }
}