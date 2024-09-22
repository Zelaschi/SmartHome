using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.DeviceModels.In;
using SmartHome.WebApi.WebModels.DeviceModels.In;
using SmartHome.WebApi.WebModels.DeviceModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeviceController : ControllerBase
{
    private readonly IDeviceLogic _deviceLogic;
    public DeviceController(IDeviceLogic deviceLogic)
    {
        _deviceLogic = deviceLogic ?? throw new ArgumentNullException(nameof(deviceLogic));
    }

    [HttpGet]
    public IActionResult GetAllDevices()
    {
        return Ok(_deviceLogic.GetAllDevices().Select(device => new DeviceResponseModel(device)).ToList());
    }
}
