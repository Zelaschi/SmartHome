using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.PaginationModels.Out;
using SmartHome.WebApi.WebModels.UserModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/v2/users")]
[ApiController]
[AuthenticationFilter]
public sealed class UsersController : ControllerBase
{
    private readonly IUsersLogic _usersLogic;
    public UsersController(IUsersLogic usersLogic)
    {
        _usersLogic = usersLogic ?? throw new ArgumentNullException(nameof(usersLogic));
    }

    [AuthorizationFilter(SeedDataConstants.LIST_ALL_ACCOUNTS_PERMISSION_ID)]
    [HttpGet]
    public IActionResult GetUsers(
    [FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null, [FromQuery] string? role = null, [FromQuery] string? fullName = null)
    {
        var query = _usersLogic.GetUsers(pageNumber, pageSize, role, fullName);
        var usersResponse = query.Select(user => new UserResponseModel(user)).ToList();
        if (pageNumber != null && pageSize != null)
        {
            return Ok(new PaginatedResponse<UserResponseModel>(usersResponse, query.Count(), (int)pageNumber, (int)pageSize));
        }
        else
        {
            return Ok(usersResponse);
        }
    }
}
