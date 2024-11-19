using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.DeviceModels.Out;
using SmartHome.WebApi.Filters;
using SmartHome.BusinessLogic.InitialSeedData;
using Microsoft.Identity.Client;
using SmartHome.WebApi.WebModels.UserModels.Out;
using SmartHome.BusinessLogic.Domain;
using SmartHome.WebApi.WebModels.Businesses.Out;
using SmartHome.WebApi.WebModels.PaginationModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/v2/devices")]
[ApiController]
[AuthenticationFilter]
public sealed class DevicesController : ControllerBase
{
    private readonly IDeviceLogic _deviceLogic;
    public DevicesController(IDeviceLogic deviceLogic)
    {
        _deviceLogic = deviceLogic ?? throw new ArgumentNullException(nameof(deviceLogic));
    }

    [AuthorizationFilter(SeedDataConstants.LIST_ALL_DEVICES_PERMISSION_ID)]
    [HttpGet]
    public IActionResult GetAllDevices(
    [FromQuery] int? pageNumber,
    [FromQuery] int? pageSize,
    [FromQuery] string? deviceName = null,
    [FromQuery] string? deviceModel = null,
    [FromQuery] string? businessName = null,
    [FromQuery] string? deviceType = null)
    {
        var query = _deviceLogic.GetDevices(pageNumber, pageSize, deviceName, deviceModel, businessName, deviceType);
        var devicesResponse = query.Select(device => new DeviceResponseModel(device)).ToList();
        if (pageNumber != null && pageSize != null)
        {
            return Ok(new PaginatedResponse<DeviceResponseModel>(devicesResponse, query.Count(), (int)pageNumber, (int)pageSize));
        }
        else
        {
            return Ok(devicesResponse);
        }
    }
}
