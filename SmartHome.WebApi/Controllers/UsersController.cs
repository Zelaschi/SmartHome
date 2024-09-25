using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.UserModels.Out;
using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/users")]
[ApiController]
[ExceptionFilter]
public class UsersController : ControllerBase
{
    private readonly IUsersLogic _usersLogic;
    public UsersController(IUsersLogic usersLogic)
    {
        _usersLogic = usersLogic ?? throw new ArgumentNullException(nameof(usersLogic));
    }

    [AuthorizationFilter(["Admin"])]
    public IActionResult GetAllUsers()
    {
        return Ok(_usersLogic.GetAllUsers().Select(user => new UserResponseModel(user)).ToList());
    }
}
