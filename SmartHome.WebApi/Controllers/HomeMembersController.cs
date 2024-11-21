using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Constants;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.HomeMemberModels.In;
using SmartHome.WebApi.WebModels.HomePermissionModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/v2/homeMembers")]
[AuthenticationFilter]
[ApiController]
public sealed class HomeMembersController : ControllerBase
{
    private readonly IHomeMemberLogic _homeMemberLogic;

    public HomeMembersController(IHomeMemberLogic homeMemberLogic)
    {
        _homeMemberLogic = homeMemberLogic ?? throw new ArgumentNullException(nameof(homeMemberLogic));
    }

    [HttpPut("{homeMemberId}/permissions")]
    public IActionResult UpdateHomeMemberPermissions([FromRoute] Guid homeMemberId, [FromBody] HomeMemberPermissions permissions)
    {
        var user = HttpContext.Items[UserStatic.User] as User;
        if (user == null)
        {
            return Unauthorized("User is missing");
        }

        if (user.Id == null)
        {
            return Unauthorized("UserId is missing");
        }

        _homeMemberLogic.UpdateHomePermissionsOfHomeMember(homeMemberId, permissions.ToHomePermissionList(), user.Id);
        return NoContent();
    }

    [HttpGet("homePermissions")]
    public IActionResult GetAllHomePermissions()
    {
        return Ok(_homeMemberLogic.GetAllHomePermissions().Select(homePermission => new HomePermissionResponseModel(homePermission)).ToList());
    }
}
