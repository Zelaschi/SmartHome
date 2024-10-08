using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.HomeMemberModels.In;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/homeMembers")]
[AuthenticationFilter]
[ExceptionFilter]
[ApiController]
public sealed class HomeMemberController : ControllerBase
{
    private readonly IHomeMemberLogic _homeMemberLogic;

    public HomeMemberController(IHomeMemberLogic homeMemberLogic)
    {
        _homeMemberLogic = homeMemberLogic ?? throw new ArgumentNullException(nameof(homeMemberLogic));
    }

    [AuthorizationFilter(SeedDataConstants.CREATE_HOME_PERMISSION_ID)]
    [HttpPost("{homeMemberId}/permissions")]
    public IActionResult AddHomePermissionsToHomeMember([FromRoute] Guid homeMemberId, [FromBody] HomeMemberPermissions permissions)
    {
        _homeMemberLogic.UpdateHomePermissionsOfHomeMember(homeMemberId, permissions.ToHomePermissionList());
        return NoContent();
    }

    [AuthorizationFilter(SeedDataConstants.CREATE_HOME_PERMISSION_ID)]
    [HttpPut("{homeMemberId}/permissions")]
    public IActionResult UpdateHomeMemberPermissions([FromRoute] Guid homeMemberId, [FromBody] HomeMemberPermissions permissions)
    {
        _homeMemberLogic.UpdateHomePermissionsOfHomeMember(homeMemberId, permissions.ToHomePermissionList());
        return NoContent();
    }
}
