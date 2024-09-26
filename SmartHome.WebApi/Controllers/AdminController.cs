using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.AdminModels.In;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/admins")]
[ApiController]
[ExceptionFilter]
public sealed class AdminController : ControllerBase
{
    private readonly IAdminLogic _adminLogic;
    public AdminController(IAdminLogic adminLogic)
    {
        _adminLogic = adminLogic;
    }

    public CreatedAtActionResult CreateAdmin(AdminRequestModel adminRequestModel)
    {
        throw new NotImplementedException();
    }
}
