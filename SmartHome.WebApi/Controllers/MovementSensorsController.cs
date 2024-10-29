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
    private readonly ICreateDeviceLogic _createDeviceLogic;
    public MovementSensorsController(ICreateDeviceLogic createDeviceLogic)
    {
        _createDeviceLogic = createDeviceLogic ?? throw new ArgumentNullException(nameof(createDeviceLogic));
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

        var result = new MovementSensorResponseModel(_createDeviceLogic.CreateDevice(deviceRequestModel.ToEntity(), user, "Movement Sensor"));
        return CreatedAtAction("CreateMovementSensor", new { result.Id }, result);
    }
}
