using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.AdminModels.In;
using SmartHome.WebApi.WebModels.AdminModels.Out;
using SmartHome.WebApi.WebModels.NotificationModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/v2/notifications")]
[ApiController]
[AuthenticationFilter]
public sealed class NotificationsController : ControllerBase
{
    private readonly INotificationLogic _notificationLogic;

    public NotificationsController(INotificationLogic notificationLogic)
    {
        _notificationLogic = notificationLogic;
    }

    [AuthorizationFilter(SeedDataConstants.CREATE_NOTIFICATION_PERMISSION_ID)]
    [HttpPost("{homeDeviceId}/movementDetection")]
    public IActionResult CreateMovementDetectionNotification([FromRoute] Guid homeDeviceId)
    {
        var createResponse = new NotificationResponseModel(_notificationLogic.CreateMovementDetectionNotification(homeDeviceId));
        return CreatedAtAction("CreateMovementDetectionNotification", new { createResponse.Id }, createResponse);
    }

    [AuthorizationFilter(SeedDataConstants.CREATE_NOTIFICATION_PERMISSION_ID)]
    [HttpPost("{homeDeviceId}/personDetection")]
    public IActionResult CreatePersonDetectionNotification([FromRoute] Guid homeDeviceId, [FromBody] Guid userId)
    {
        var createResponse = new NotificationResponseModel(_notificationLogic.CreatePersonDetectionNotification(homeDeviceId, userId));
        return CreatedAtAction("CreatePersonDetectionNotification", new { createResponse.Id }, createResponse);
    }

    [HttpPost("{homeDeviceId}/openClosedStatus")]
    public IActionResult CreateOpenCloseWindowNotification([FromRoute] Guid homeDeviceId)
    {
        var createResponse = new NotificationResponseModel(_notificationLogic.CreateOpenCloseWindowNotification(homeDeviceId));
        return CreatedAtAction("CreateOpenCloseWindowNotification", new { createResponse.Id }, createResponse);
    }
}
