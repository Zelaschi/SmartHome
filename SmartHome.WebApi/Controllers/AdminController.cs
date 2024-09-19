using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Interfaces;
using SmartHome.WebModels.UserModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IAdminLogic _adminLogic;
    public AdminController(IAdminLogic adminLogic)
    {
        _adminLogic = adminLogic ?? throw new ArgumentNullException(nameof(adminLogic));
    }

    public IActionResult GetAllUsers()
    {
        return Ok(_adminLogic.GetAllUsers().Select(user => new UserResponseModel(user)).ToList());
    }
}
