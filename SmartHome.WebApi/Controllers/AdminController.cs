using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Interfaces;

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

    public OkObjectResult GetAllUsers()
    {
        throw new NotImplementedException();
    }
}
