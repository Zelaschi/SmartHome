using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.DeviceModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/v2/deviceTypes")]
[ApiController]
[AuthenticationFilter]
[ExceptionFilter]
public sealed class DeviceTypesController : ControllerBase
{
    private readonly IDeviceLogic _deviceLogic;
    public DeviceTypesController(IDeviceLogic deviceLogic)
    {
        _deviceLogic = deviceLogic ?? throw new ArgumentNullException(nameof(deviceLogic));
    }

    [AuthorizationFilter(SeedDataConstants.LIST_ALL_DEVICES_TYPES_PERMISSION_ID)]
    [HttpGet]
    public IActionResult GetAllDeviceTypes()
    {
        return Ok(_deviceLogic.GetAllDeviceTypes().Select(deviceType => new DeviceTypesResponseModel(deviceType)).ToList());
    }
}
