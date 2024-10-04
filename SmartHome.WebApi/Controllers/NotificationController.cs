using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.AdminModels.In;
using SmartHome.WebApi.WebModels.AdminModels.Out;
using SmartHome.WebApi.WebModels.NotificationModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[AuthenticationFilter]
[ExceptionFilter]
public sealed class NotificationController : ControllerBase
{
    private readonly INotificationLogic _notificationLogic;

    public NotificationController(INotificationLogic notificationLogic)
    {
        _notificationLogic = notificationLogic;
    }

    [HttpPost("{homeDeviceId}/movementDetection")]
    public IActionResult CreateMovementDetectionNotification([FromRoute] Guid homeDeviceId)
    {
        var createResponse = new NotificationResponseModel(_notificationLogic.CreateMovementDetectionNotification(homeDeviceId));
        return CreatedAtAction("CreateMovementDetectionNotification", new { createResponse.Id }, createResponse);
    }

    [HttpPost("{homeDeviceId}/personDetection")]
    public IActionResult CreatePersonDetectionNotification([FromRoute] Guid homeDeviceId, Guid userId)
    {
        var createResponse = new NotificationResponseModel(_notificationLogic.CreatePersonDetectionNotification(homeDeviceId, userId));
        return CreatedAtAction("CreatePersonDetectionNotification", new { createResponse.Id }, createResponse);
    }
}
