using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.DeviceModels.Out;
using SmartHome.WebApi.WebModels.HomeMemberModels.In;
using SmartHome.WebApi.WebModels.HomeMemberModels.Out;
using SmartHome.WebApi.WebModels.HomePermissionModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/v2/homeMembers")]
[AuthenticationFilter]
[ExceptionFilter]
[ApiController]
public sealed class HomeMembersController : ControllerBase
{
    private readonly IHomeMemberLogic _homeMemberLogic;

    public HomeMembersController(IHomeMemberLogic homeMemberLogic)
    {
        _homeMemberLogic = homeMemberLogic ?? throw new ArgumentNullException(nameof(homeMemberLogic));
    }

    [AuthorizationFilter(SeedDataConstants.CREATE_HOME_PERMISSION_ID)]
    [HttpPost("{homeMemberId}/permissions")]
    public IActionResult AddHomePermissionsToHomeMember([FromRoute] Guid homeMemberId, [FromBody] HomeMemberPermissions permissions)
    {
        _homeMemberLogic.AddHomePermissionsToHomeMember(homeMemberId, permissions.ToHomePermissionList());
        return NoContent();
    }

    [AuthorizationFilter(SeedDataConstants.CREATE_HOME_PERMISSION_ID)]
    [HttpPut("{homeMemberId}/permissions")]
    public IActionResult UpdateHomeMemberPermissions([FromRoute] Guid homeMemberId, [FromBody] HomeMemberPermissions permissions)
    {
        _homeMemberLogic.UpdateHomePermissionsOfHomeMember(homeMemberId, permissions.ToHomePermissionList());
        return NoContent();
    }

    [HttpGet("homePermissions")]
    public IActionResult GetAllHomePermissions()
    {
        return Ok(_homeMemberLogic.GetAllHomePermissions().Select(homePermission => new HomePermissionResponseModel(homePermission)).ToList());
    }
}
