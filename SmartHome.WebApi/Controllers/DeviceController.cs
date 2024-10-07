using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.DeviceModels.In;
using SmartHome.WebApi.WebModels.DeviceModels.Out;
using SmartHome.WebApi.Filters;
using SmartHome.BusinessLogic.InitialSeedData;
using Microsoft.Identity.Client;
using SmartHome.WebApi.WebModels.QueryParams;
using SmartHome.WebApi.WebModels.UserModels.Out;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[AuthenticationFilter]
[ExceptionFilter]
public sealed class DeviceController : ControllerBase
{
    private readonly IDeviceLogic _deviceLogic;
    public DeviceController(IDeviceLogic deviceLogic)
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
        var query = _deviceLogic.GetAllDevices();

        if (!string.IsNullOrEmpty(deviceName))
        {
            query = query.Where(d => d.Name.Contains(deviceName));
        }

        if (!string.IsNullOrEmpty(deviceModel))
        {
            query = query.Where(d => d.ModelNumber.Contains(deviceModel));
        }

        if (!string.IsNullOrEmpty(businessName))
        {
            query = query.Where(d => d.Business.Name.Contains(businessName));
        }

        if (!string.IsNullOrEmpty(deviceType))
        {
            query = query.Where(d => d.Type == deviceType);
        }

        var totalCount = query.Count();
        List<DeviceResponseModel> devicesResponse;

        if (pageNumber != null && pageSize != null)
        {
            var pagedData = query
                .Skip(((int)pageNumber - 1) * (int)pageSize)
                .Take((int)pageSize)
                .ToList();

            devicesResponse = pagedData.Select(device => new DeviceResponseModel(device)).ToList();
            return Ok(new
            {
                Data = devicesResponse,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            });
        }
        else
        {
            devicesResponse = query.Select(device => new DeviceResponseModel(device)).ToList();
            return Ok(devicesResponse);
        }
    }

    [AuthorizationFilter(SeedDataConstants.CREATE_DEVICE_PERMISSION_ID)]
    [HttpPost]
    public IActionResult CreateDevice([FromBody] CreateDeviceRequestModel deviceRequestModel)
    {
        var user = HttpContext.Items["User"] as User;

        if (user == null)
        {
            return Unauthorized("UserId is missing");
        }

        var response = new DeviceResponseModel(_deviceLogic.CreateDevice(deviceRequestModel.ToEntity(), user));
        return CreatedAtAction("CreateDevice", new {response.Id }, response);
    }
}
