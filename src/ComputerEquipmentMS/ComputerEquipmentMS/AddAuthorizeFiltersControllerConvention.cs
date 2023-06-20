using ComputerEquipmentMS.Controllers;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace ComputerEquipmentMS;

public class AddAuthorizeFiltersControllerConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        if (controller.ControllerName != nameof(AuthController))
            controller.Filters.Add(new AuthorizeFilter("defaultPolicy"));    
    }
}