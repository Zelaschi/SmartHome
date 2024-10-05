using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.HomeModels.Out;

namespace SmartHome.WebApi.Controllers;
[Route("api/v1/me")]
[ApiController]
[AuthenticationFilter]
[ExceptionFilter]
public class MeController : ControllerBase
{
    private readonly INotificationLogic _notificationLogic;
    private readonly IHomeLogic _homeLogic;
    public MeController(INotificationLogic notificationLogic, IHomeLogic homeLogic)
    {
        _notificationLogic = notificationLogic;
        _homeLogic = homeLogic;
    }

    [HttpGet("/notifications")]
    public IActionResult GetUsersNotifications()
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null)
        {
            return Unauthorized("UserId is missing");
        }

        return Ok(_notificationLogic.GetUsersNotifications(user));
    }

    [HttpGet("/homes")]
    public IActionResult GetAllHomesByUserId([FromRoute] Guid userId)
    {
        return Ok(_homeLogic.GetAllHomesByUserId(userId).Select(home => new HomeResponseModel(home)).ToList());
    }
}
