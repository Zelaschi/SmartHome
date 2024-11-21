using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Constants;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.HomeModels.Out;
using SmartHome.WebApi.WebModels.NotificationModels.Out;

namespace SmartHome.WebApi.Controllers;
[Route("api/v2/me")]
[ApiController]
[AuthenticationFilter]
public class MeController : ControllerBase
{
    private readonly INotificationLogic _notificationLogic;
    private readonly IHomeLogic _homeLogic;
    public MeController(INotificationLogic notificationLogic, IHomeLogic homeLogic)
    {
        _notificationLogic = notificationLogic;
        _homeLogic = homeLogic;
    }

    [AuthorizationFilter(SeedDataConstants.LIST_ALL_USERS_NOTIFICATIONS_PERMISSION_ID)]
    [HttpGet("notifications")]
    public IActionResult GetUsersNotifications()
    {
        var user = HttpContext.Items[UserStatic.User] as User;
        if (user == null)
        {
            return Unauthorized("UserId is missing");
        }

        return Ok(_notificationLogic.GetUsersNotifications(user).Select(notification => new MeNotificationResponseModel(notification)).ToList());
    }

    [AuthorizationFilter(SeedDataConstants.LIST_ALL_USERS_HOMES_PERMISSION_ID)]
    [HttpGet("homes")]
    public IActionResult GetAllHomesByUserId()
    {
        var user = HttpContext.Items[UserStatic.User] as User;
        if (user == null)
        {
            return Unauthorized("UserId is missing");
        }

        if (user.Id == null)
        {
            return Unauthorized("UserId is missing");
        }

        return Ok(_homeLogic.GetAllHomesByUserId((Guid)user.Id).Select(home => new HomeResponseModel(home)).ToList());
    }
}
