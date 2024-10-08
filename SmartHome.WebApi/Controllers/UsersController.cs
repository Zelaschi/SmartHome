using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.UserModels.Out;
using SmartHome.WebApi.Filters;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.WebApi.WebModels.Businesses.Out;
using SmartHome.WebApi.WebModels.PaginationModels.Out;

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
    public IActionResult GetAllUsers(
    [FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null, [FromQuery] string? role = null, [FromQuery] string? fullName = null)
    {
        var query = _usersLogic.GetAllUsers();

        if (!string.IsNullOrEmpty(role))
        {
            query = query.Where(u => u.Role.Name == role);
        }

        if (!string.IsNullOrEmpty(fullName))
        {
            var searchTerm = fullName.ToLower();
            query = query.Where(u =>
                (u.Name.ToLower() + " " + u.Surname.ToLower()).Contains(searchTerm) ||
                (u.Surname.ToLower() + " " + u.Name.ToLower()).Contains(searchTerm)
            );
        }

        var totalCount = query.Count();
        List<UserResponseModel> usersResponse;

        if (pageNumber != null && pageSize != null)
        {
            var pagedData = query
            .Skip(((int)pageNumber - 1) * (int)pageSize)
            .Take((int)pageSize)
            .ToList();

            usersResponse = pagedData.Select(user => new UserResponseModel(user)).ToList();
            return Ok(new PaginatedResponse<UserResponseModel>(usersResponse, totalCount, (int)pageNumber, (int)pageSize));
        }
        else
        {
            usersResponse = query.Select(user => new UserResponseModel(user)).ToList();
            return Ok(usersResponse);
        }
    }
}
