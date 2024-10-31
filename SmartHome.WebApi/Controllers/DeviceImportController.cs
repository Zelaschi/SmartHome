using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.DeviceImportModels.In;
using SmartHome.WebApi.WebModels.DeviceImportModels.Out;

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

        var response = deviceImportLogic.ImportDevices(deviceImportRequestModel.DllName, deviceImportRequestModel.FileName, user)
            .Select(b => new DeviceWithoutPhotosResponseModel(b)).ToList();
        return CreatedAtAction("ImportDevices", new { Id = string.Empty}, response);
    }
}
