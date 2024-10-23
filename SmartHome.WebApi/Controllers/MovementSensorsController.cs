using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.MovementSensorModels.In;
using SmartHome.WebApi.WebModels.MovementSensorModels.Out;
using SmartHome.WebApi.WebModels.WindowSensorModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/v2/movementSensors")]
[ApiController]
[AuthenticationFilter]
[ExceptionFilter]

public class MovementSensorsController : ControllerBase
{
    private readonly IMovementSensorLogic _movementSensorLogic;
    public MovementSensorsController(IMovementSensorLogic movementSensorLogic)
    {
        _movementSensorLogic = movementSensorLogic ?? throw new ArgumentNullException(nameof(movementSensorLogic));
    }

    [AuthorizationFilter(SeedDataConstants.CREATE_DEVICE_PERMISSION_ID)]
    [HttpPost]
    public IActionResult CreateMovementSensor([FromBody]MovementSensorRequestModel deviceRequestModel)
    {
        var user = HttpContext.Items["User"] as User;

        if (user == null)
        {
            return Unauthorized("UserId is missing");
        }

        var result = new MovementSensorResponseModel(_movementSensorLogic.CreateMovementSensor(deviceRequestModel.ToEntity(), user));
        return CreatedAtAction("CreateMovementSensor", new { result.Id }, result);
    }
}
