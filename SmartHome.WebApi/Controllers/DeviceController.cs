﻿using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.DeviceModels.In;
using SmartHome.WebApi.WebModels.DeviceModels.Out;
using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[ExceptionFilter]
public sealed class DeviceController : ControllerBase
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

    [HttpPost]
    public IActionResult CreateDevice([FromBody] CreateDeviceRequestModel deviceRequestModel)
    {
        var response = new DeviceResponseModel(_deviceLogic.CreateDevice(deviceRequestModel.ToEntity()));
        return CreatedAtAction("CreateDevice", new {response.Id }, response);
    }
}
