using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.UserModels.Out;
using SmartHome.WebApi.Filters;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.WebApi.WebModels.QueryParams;

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
        [FromQuery] Pagination paginationParams, [FromQuery] string? role = null, [FromQuery] string? fullName = null)
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

        var pagedData = query
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToList();

        var usersResponse = pagedData.Select(user => new UserResponseModel(user)).ToList();

        return Ok(new
        {
            Data = usersResponse,
            TotalCount = totalCount,
            PageNumber = paginationParams.PageNumber,
            PageSize = paginationParams.PageSize
        });
    }
}
