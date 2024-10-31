using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.DeviceImportModels.In;
using SmartHome.WebApi.WebModels.DeviceImportModels.Out;

[Route("api/v2/deviceImport")]
[ApiController]
[AuthenticationFilter]
[ExceptionFilter]
public sealed class DeviceImportController : ControllerBase
{
    private readonly IDeviceImportLogic deviceImportLogic;
    public DeviceImportController(IDeviceImportLogic iDeviceImportLogic)
    {
        deviceImportLogic = iDeviceImportLogic;
    }

    [HttpPost]
    public IActionResult ImportDevice(DeviceImportRequestModel deviceImportRequestModel)
    {
        var user = HttpContext.Items["User"] as User;

        if (user == null)
        {
            return Unauthorized("UserId is missing");
        }

        var response = deviceImportLogic.ImportDevices(deviceImportRequestModel.DllName, deviceImportRequestModel.FileName, user);
        return CreatedAtAction("ImportDevices", new { Id = string.Empty}, response);
    }
}
