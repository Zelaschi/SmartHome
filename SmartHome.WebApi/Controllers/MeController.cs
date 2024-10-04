using Microsoft.AspNetCore.Mvc;
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
    public IActionResult GetNotificationsByHomeMemberId([FromRoute] Guid homeMemberId)
    {
        return Ok(_notificationLogic.GetNotificationsByHomeMemberId(homeMemberId));
    }

    [HttpGet("/homes")]
    public IActionResult GetAllHomesByUserId([FromRoute] Guid userId)
    {
        return Ok(_homeLogic.GetAllHomesByUserId(userId).Select(home => new HomeResponseModel(home)).ToList());
    }
}
