using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.RoomModels.In;
using SmartHome.WebApi.WebModels.RoomModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/v2/rooms")]
[ApiController]
[AuthenticationFilter]
[ExceptionFilter]

public sealed class RoomsController : ControllerBase
{
    public readonly IRoomLogic _roomLogic;

    public RoomsController(IRoomLogic roomLogic)
    {
        _roomLogic = roomLogic;
    }

    [AuthorizationFilter(SeedDataConstants.CREATE_ROOM_PERMISSION_ID)]
    [HttpPost("{homeId}")]
    public IActionResult CreateRoom([FromBody] RoomRequestModel roomRequestModel, [FromRoute] Guid homeId)
    {
        throw new NotImplementedException();
    }
}
