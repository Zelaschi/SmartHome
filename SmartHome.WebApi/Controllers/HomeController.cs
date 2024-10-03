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

[Route("api/v1/[controller]")]
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

    [HttpPost("{homeId}/homeDevices")]
    public IActionResult AddDeviceToHome([FromRoute] Guid homeId, [FromBody] Guid deviceId)
    {
        _homeLogic.AddDeviceToHome(homeId, deviceId);
        return NoContent();
    }

    [HttpPost("{homeId}/members")]
    public IActionResult AddHomeMemberToHome([FromRoute] Guid homeId)
    {
        var userIdString = HttpContext.Items["UserId"] as string;
        if (userIdString == null)
        {
            return Unauthorized("UserId is missing");
        }

        var userId = Guid.Parse(userIdString);
        _homeLogic.AddHomeMemberToHome(homeId, userId);
        return NoContent();
    }

    [AuthorizationFilter(SeedDataConstants.CREATE_HOME_PERMISSION_ID)]
    [HttpPost]
    public IActionResult CreateHome([FromBody] CreateHomeRequestModel homeRequestModel)
    {
        var userIdString = HttpContext.Items["UserId"] as string;
        if (userIdString == null)
        {
            return Unauthorized("UserId is missing");
        }

        var userId = Guid.Parse(userIdString);
        var response = new HomeResponseModel(_homeLogic.CreateHome(homeRequestModel.ToEntity(), userId));
        return CreatedAtAction("CreateHome", new { response.Id }, response);
    }

    [HttpGet("{homeId}/homeDevices")]
    public IActionResult GetAllHomeDevices([FromRoute] Guid homeId)
    {
        return Ok(_homeLogic.GetAllHomeDevices(homeId).Select(homeDevice => new HomeDeviceResponseModel(homeDevice)).ToList());
    }

    [HttpGet("{homeId}/members")]
    public IActionResult GetAllHomeMembers([FromRoute] Guid homeId)
    {
        return Ok(_homeLogic.GetAllHomeMembers(homeId).Select(homeMember => new HomeMemberResponseModel(homeMember)).ToList());
    }

    [HttpGet("{userId}")]
    public IActionResult GetAllHomesByUserId([FromRoute] Guid userId)
    {
        return Ok(_homeLogic.GetAllHomesByUserId(userId).Select(home => new HomeResponseModel(home)).ToList());
    }
}
