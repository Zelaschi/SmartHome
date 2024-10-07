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

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/homes")]
[ApiController]
[AuthenticationFilter]
[ExceptionFilter]
public sealed class HomeController : ControllerBase
{
    private readonly IHomeLogic _homeLogic;

    public HomeController(IHomeLogic homeLogic)
    {
        _homeLogic = homeLogic;
    }

    [AuthorizationFilter(SeedDataConstants.HOME_RELATED_PERMISSION_ID)]
    [HttpPost("{homeId}/homeDevices")]
    public IActionResult AddDeviceToHome([FromRoute] Guid homeId, [FromBody] Guid deviceId)
    {
        _homeLogic.AddDeviceToHome(homeId, deviceId);
        return NoContent();
    }

    [AuthorizationFilter(SeedDataConstants.HOME_RELATED_PERMISSION_ID)]
    [HttpPost("{homeId}/members")]
    public IActionResult AddHomeMemberToHome([FromRoute] Guid homeId, [FromBody] Guid userId)
    {
        _homeLogic.AddHomeMemberToHome(homeId, userId);
        return NoContent();
    }

    [AuthorizationFilter(SeedDataConstants.CREATE_HOME_PERMISSION_ID)]
    [HttpPost]
    public IActionResult CreateHome([FromBody] CreateHomeRequestModel homeRequestModel)
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null)
        {
            return Unauthorized("UserId is missing");
        }

        var response = new HomeResponseModel(_homeLogic.CreateHome(homeRequestModel.ToEntity(), user.Id));
        return CreatedAtAction("CreateHome", new { response.Id }, response);
    }

    [AuthorizationFilter(SeedDataConstants.HOME_RELATED_PERMISSION_ID)]
    [HttpGet("{homeId}/homeDevices")]
    public IActionResult GetAllHomeDevices([FromRoute] Guid homeId)
    {
        return Ok(_homeLogic.GetAllHomeDevices(homeId).Select(homeDevice => new HomeDeviceResponseModel(homeDevice)).ToList());
    }

    [AuthorizationFilter(SeedDataConstants.HOME_RELATED_PERMISSION_ID)]
    [HttpGet("{homeId}/members")]
    public IActionResult GetAllHomeMembers([FromRoute] Guid homeId)
    {
        return Ok(_homeLogic.GetAllHomeMembers(homeId).Select(homeMember => new HomeMemberResponseModel(homeMember)).ToList());
    }
}
