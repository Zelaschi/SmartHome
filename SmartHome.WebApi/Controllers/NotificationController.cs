using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly INotificationLogic _notificationLogic;

    public NotificationController(INotificationLogic notificationLogic)
    {
        _notificationLogic = notificationLogic;
    }

    [HttpGet("{homeMemberId}")]
    public IActionResult GetNotificationsByHomeMemberId([FromRoute] Guid homeMemberId)
    {
        throw new NotImplementedException();
    }
}
