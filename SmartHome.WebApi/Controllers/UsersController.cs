using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.UserModels.Out;
using SmartHome.WebApi.Filters;
using SmartHome.BusinessLogic.InitialSeedData;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/users")]
[ApiController]
[AuthenticationFilter]
[ExceptionFilter]
public sealed class UsersController : ControllerBase
{
    private readonly IUsersLogic _usersLogic;
    public UsersController(IUsersLogic usersLogic)
    {
        _usersLogic = usersLogic ?? throw new ArgumentNullException(nameof(usersLogic));
    }

    [AuthorizationFilter(SeedDataConstants.LIST_ALL_ACCOUNTS_PERMISSION_ID)]
    [HttpGet]
    public IActionResult GetAllUsers()
    {
        return Ok(_usersLogic.GetAllUsers().Select(user => new UserResponseModel(user)).ToList());
    }
}
