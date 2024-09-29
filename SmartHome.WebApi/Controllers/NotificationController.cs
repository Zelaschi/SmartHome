using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.AdminModels.In;
using SmartHome.WebApi.WebModels.AdminModels.Out;
using SmartHome.WebApi.WebModels.NotificationModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public sealed class NotificationController : ControllerBase
{
    private readonly INotificationLogic _notificationLogic;

    public NotificationController(INotificationLogic notificationLogic)
    {
        _notificationLogic = notificationLogic;
    }

    [HttpGet("{homeMemberId}")]
    public IActionResult GetNotificationsByHomeMemberId([FromRoute] Guid homeMemberId)
    {
        return Ok(_notificationLogic.GetNotificationsByHomeMemberId(homeMemberId));
    }

    [HttpPost("{homeDeviceId}")]
    public IActionResult CreateMovementDetectionNotification([FromRoute] Guid homeDeviceId)
    {
        throw new NotImplementedException();
    }
}
