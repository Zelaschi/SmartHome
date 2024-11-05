using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Constants;
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
    private readonly ICreateDeviceLogic _createDeviceLogic;
    public WindowSensorsController(ICreateDeviceLogic createDeviceLogic)
    {
        _createDeviceLogic = createDeviceLogic ?? throw new ArgumentNullException(nameof(createDeviceLogic));
    }

    [AuthorizationFilter(SeedDataConstants.CREATE_DEVICE_PERMISSION_ID)]
    [HttpPost]
    public IActionResult CreateWindowSensor([FromBody]WindowSensorRequestModel deviceRequestModel)
    {
        var user = HttpContext.Items[UserStatic.User] as User;

        if (user == null)
        {
            return Unauthorized("UserId is missing");
        }

        var result = new WindowSensorResponseModel(_createDeviceLogic.CreateDevice(deviceRequestModel.ToEntity(), user, DeviceTypesStatic.WindowSensor));
        return CreatedAtAction("CreateWindowSensor", new { result.Id }, result);
    }
}
