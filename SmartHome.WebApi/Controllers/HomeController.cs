﻿using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.HomeDeviceModels.Out;
using SmartHome.WebApi.WebModels.HomeMemberModels.Out;
using SmartHome.WebApi.WebModels.HomeModels.In;
using SmartHome.WebApi.WebModels.HomeModels.Out;

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
    [HomeAuthorizationFilter(SeedDataConstants.ADD_DEVICES_TO_HOME_HOMEPERMISSION_ID)]
    [HttpPost("{homeId}/homeDevices")]
    public IActionResult AddDeviceToHome([FromRoute] Guid homeId, [FromBody] Guid deviceId)
    {
        var response = new HomeDeviceResponseModel(_homeLogic.AddDeviceToHome(homeId, deviceId));
        return CreatedAtAction("AddDeviceToHome", new { response.HardwareId }, response);
    }

    [AuthorizationFilter(SeedDataConstants.HOME_RELATED_PERMISSION_ID)]
    [HomeAuthorizationFilter(SeedDataConstants.ADD_MEMBER_TO_HOME_HOMEPERMISSION_ID)]
    [HttpPost("{homeId}/members")]
    public IActionResult AddHomeMemberToHome([FromRoute] Guid homeId, [FromBody] Guid userId)
    {
        var response = new HomeMemberResponseModel(_homeLogic.AddHomeMemberToHome(homeId, userId));
        return CreatedAtAction("AddHomeMemberToHome", new { response.HomeMemberId }, response);
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
    [HomeAuthorizationFilter(SeedDataConstants.LIST_DEVICES_HOMEPERMISSION_ID)]
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
