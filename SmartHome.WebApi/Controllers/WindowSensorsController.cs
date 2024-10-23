using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.WindowSensorModels.In;
using SmartHome.WebApi.WebModels.WindowSensorModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/v2/windowSensors")]
[ApiController]
[AuthenticationFilter]
[ExceptionFilter]
public class WindowSensorsController : ControllerBase
{
    private readonly IWindowSensorLogic _windowSensorLogic;
    public WindowSensorsController(IWindowSensorLogic windowSensorLogic)
    {
        _windowSensorLogic = windowSensorLogic ?? throw new ArgumentNullException(nameof(windowSensorLogic));
    }

    [AuthorizationFilter(SeedDataConstants.CREATE_DEVICE_PERMISSION_ID)]
    [HttpPost]
    public IActionResult CreateWindowSensor([FromBody]WindowSensorRequestModel deviceRequestModel)
    {
        var user = HttpContext.Items["User"] as User;

        if (user == null)
        {
            return Unauthorized("UserId is missing");
        }

        var result = new WindowSensorResponseModel(_windowSensorLogic.CreateWindowSensor(deviceRequestModel.ToEntity(), user));
        return CreatedAtAction("CreateWindowSensor", new { result.Id }, result);
    }
}
