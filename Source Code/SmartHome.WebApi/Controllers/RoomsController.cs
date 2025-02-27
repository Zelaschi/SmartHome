﻿using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.HomeDeviceModels.Out;
using SmartHome.WebApi.WebModels.RoomModels.In;
using SmartHome.WebApi.WebModels.RoomModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/v2/rooms")]
[ApiController]
[AuthenticationFilter]

public sealed class RoomsController : ControllerBase
{
    public readonly IRoomLogic _roomLogic;

    public RoomsController(IRoomLogic roomLogic)
    {
        _roomLogic = roomLogic;
    }

    [HomeAuthorizationFilter(SeedDataConstants.CREATE_ROOM_PERMISSION_ID)]
    [HttpPost("{homeId}")]
    public IActionResult CreateRoom([FromBody] RoomRequestModel roomRequestModel, [FromRoute] Guid homeId)
    {
        var createResponse = new RoomResponseModel(_roomLogic.CreateRoom(roomRequestModel.ToEntity(), homeId));
        return CreatedAtAction("CreateRoom", new { createResponse.Id }, createResponse);
    }

    [HttpPost("{roomId}/homeDevices")]
    public IActionResult AddDevicesToRoom([FromBody] HomeDeviceIdRequestModel homeDeviceId, [FromRoute] Guid roomId)
    {
        var createResponse = new HomeDeviceResponseModel(_roomLogic.AddDevicesToRoom(homeDeviceId.HomeDeviceId, roomId));
        return CreatedAtAction("AddDevicesToRoom", new { createResponse.HardwardId }, createResponse);
    }

    [AuthorizationFilter(SeedDataConstants.HOME_RELATED_PERMISSION_ID)]
    [HttpGet("{homeId}")]
    public IActionResult GetAllRoomsFromHome([FromRoute] Guid homeId)
    {
        return Ok(_roomLogic.GetAllRoomsFromHome(homeId).Select(room => new RoomResponseModel(room)).ToList());
    }
}
