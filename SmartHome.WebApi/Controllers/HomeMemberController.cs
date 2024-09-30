using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.DeviceModels.Out;
using SmartHome.WebApi.WebModels.HomeMemberModels.In;
using SmartHome.WebApi.WebModels.HomeMemberModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/homeMembers")]
[ApiController]
public sealed class HomeMemberController : ControllerBase
{
    private readonly IHomeMemberLogic _homeMemberLogic;

    public HomeMemberController(IHomeMemberLogic homeMemberLogic)
    {
        _homeMemberLogic = homeMemberLogic ?? throw new ArgumentNullException(nameof(homeMemberLogic));
    }

    [HttpPost("{homeMemberId}/permissions")]
    public IActionResult AddHomePermissionsToHomeMember([FromRoute] Guid homeMemberId, [FromRoute] Guid? memberId,[FromBody] HomeMemberPermissions permissions)
    {
        var memberPermissions = ConvertHomeMemberPermissionsToList(permissions);
        _homeMemberLogic.AddHomePermissionsToHomeMember(homeMemberId, memberId, memberPermissions);
        return NoContent();
    }

    private List<HomePermission> ConvertHomeMemberPermissionsToList(HomeMemberPermissions permissions)
    {
        var homePermissions = new List<HomePermission>();
        if (permissions.AddMemberPermission)
            homePermissions.Add(new HomePermission { Name = "AddMemberPermission" });
        if (permissions.AddDevicePermission)
            homePermissions.Add(new HomePermission { Name = "AddDevicesPermission" });
        if (permissions.ListDevicesPermission)
            homePermissions.Add(new HomePermission { Name = "ListDevicesPermission" });
        if (permissions.NotificationsPermission)
            homePermissions.Add(new HomePermission { Name = "NotificationsPermission" });

        return homePermissions;
    }
}
