using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.AdminModels.In;
using SmartHome.WebApi.WebModels.AdminModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/admins")]
[ApiController]
[ExceptionFilter]
[AuthorizationFilter(["Admin"])]
public sealed class AdminController : ControllerBase
{
    private readonly IAdminLogic _adminLogic;
    public AdminController(IAdminLogic adminLogic)
    {
        _adminLogic = adminLogic;
    }

    [HttpPost]
    public IActionResult CreateAdmin(AdminRequestModel adminRequestModel)
    {
        var createResponse = new AdminResponseModel(_adminLogic.CreateAdmin(adminRequestModel.ToEntitiy()));
        return CreatedAtAction("CreateAdmin", new { createResponse.Id }, createResponse);
    }

    [HttpDelete("{adminId}")]
    public IActionResult DeleteAdmin(Guid adminId)
    {
        _adminLogic.DeleteAdmin(adminId);
        return NoContent();
    }
}
