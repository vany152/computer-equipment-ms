using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComputerEquipmentMS.Controllers;

[AllowAnonymous]
public class ErrorController : Controller
{
    [HttpGet, Route("Error")]
    public IActionResult Error(string? errorMessage)
    {
        return View(errorMessage);
    }
}
