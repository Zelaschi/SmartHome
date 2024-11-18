using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.HomeModels.Out;
using SmartHome.WebApi.WebModels.HomeModels.In;
using SmartHome.WebApi.Filters;
using System.Reflection.Metadata.Ecma335;
using SmartHome.BusinessLogic.Domain;
using SmartHome.WebApi.WebModels.HomeDeviceModels.Out;
using SmartHome.WebApi.WebModels.HomeMemberModels.Out;
using SmartHome.BusinessLogic.InitialSeedData;
using Microsoft.Identity.Client;
using SmartHome.BusinessLogic.Constants;
using SmartHome.WebApi.WebModels.UpdateNameModels.In;
using SmartHome.WebApi.WebModels.UserModels.Out;
using Microsoft.Extensions.Configuration.UserSecrets;
using SmartHome.WebApi.WebModels.AddUserToHome.In;

namespace SmartHome.WebApi.Controllers;

[Route("api/v2/homes")]
[ApiController]
[AuthenticationFilter]
[ExceptionFilter]
public sealed class HomesController : ControllerBase
{
    private readonly IHomeLogic _homeLogic;

    public HomesController(IHomeLogic homeLogic)
    {
        _homeLogic = homeLogic;
    }

    [AuthorizationFilter(SeedDataConstants.HOME_RELATED_PERMISSION_ID)]
    [HomeAuthorizationFilter(SeedDataConstants.ADD_DEVICES_TO_HOME_HOMEPERMISSION_ID)]
    [HttpPost("{homeId}/homeDevices")]
    public IActionResult AddDeviceToHome([FromRoute] Guid homeId, [FromBody] AddDeviceToHomeRequestModel request)
    {
        _homeLogic.AddDeviceToHome(homeId, request.DeviceId);
        return NoContent();
    }

    [AuthorizationFilter(SeedDataConstants.HOME_RELATED_PERMISSION_ID)]
    [HomeAuthorizationFilter(SeedDataConstants.ADD_MEMBER_TO_HOME_HOMEPERMISSION_ID)]
    [HttpGet("{homeId}/unRelatedHomeOwners")]
    public IActionResult UnRelatedHomeOwners([FromRoute] Guid homeId)
    {
        return Ok(_homeLogic.UnRelatedHomeOwners(homeId).Select(user => new UserResponseModel(user)).ToList());
    }

    [AuthorizationFilter(SeedDataConstants.HOME_RELATED_PERMISSION_ID)]
    [HomeAuthorizationFilter(SeedDataConstants.ADD_MEMBER_TO_HOME_HOMEPERMISSION_ID)]
    [HttpPost("{homeId}/members")]
    public IActionResult AddHomeMemberToHome([FromRoute] Guid homeId, [FromBody] AddUserToHomeRequestModel userId)
    {
        _homeLogic.AddHomeMemberToHome(homeId, userId.UserId);
        return NoContent();
    }

    [AuthorizationFilter(SeedDataConstants.CREATE_HOME_PERMISSION_ID)]
    [HttpPost]
    public IActionResult CreateHome([FromBody] CreateHomeRequestModel homeRequestModel)
    {
        var user = HttpContext.Items[UserStatic.User] as User;
        if (user == null)
        {
            return Unauthorized("UserId is missing");
        }

        var response = new HomeResponseModel(_homeLogic.CreateHome(homeRequestModel.ToEntity(), user.Id));
        return CreatedAtAction("CreateHome", new { response.Id }, response);
    }

    [AuthorizationFilter(SeedDataConstants.HOME_RELATED_PERMISSION_ID)]
    [HomeAuthorizationFilter(SeedDataConstants.LIST_DEVICES_HOMEPERMISSION_ID)]
    [HttpGet("{homeId}/homeDevices")]
    public IActionResult GetAllHomeDevices([FromRoute] Guid homeId, [FromQuery] string? room = null)
    {
        return Ok(_homeLogic.GetAllHomeDevices(homeId, room).Select(homeDevice => new HomeDeviceResponseModel(homeDevice)).ToList());
    }

    [AuthorizationFilter(SeedDataConstants.HOME_RELATED_PERMISSION_ID)]
    [HttpGet("{homeId}/members")]
    public IActionResult GetAllHomeMembers([FromRoute] Guid homeId)
    {
        return Ok(_homeLogic.GetAllHomeMembers(homeId).Select(homeMember => new HomeMemberResponseModel(homeMember)).ToList());
    }

    [AuthorizationFilter(SeedDataConstants.HOME_RELATED_PERMISSION_ID)]
    [HttpPatch("{homeDeviceId}/homeDeviceName")]

    public IActionResult UpdateHomeDeviceName([FromRoute] Guid homeDeviceId, [FromBody] UpdateNameRequestModel request)
    {
        _homeLogic.UpdateHomeDeviceName(homeDeviceId, request.NewName);
        return Ok();
    }

    [AuthorizationFilter(SeedDataConstants.HOME_RELATED_PERMISSION_ID)]
    [HttpPatch("{homeId}/homeName")]
    public IActionResult UpdateHomeName([FromRoute] Guid homeId, [FromBody] UpdateNameRequestModel request)
    {
        _homeLogic.UpdateHomeName(homeId, request.NewName);
        return Ok();
    }
}
