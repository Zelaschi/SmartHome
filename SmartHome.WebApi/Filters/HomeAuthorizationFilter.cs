using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class HomeAuthorizationFilter : Attribute, IAuthorizationFilter
{
    private readonly string homePermissionId;

    public HomeAuthorizationFilter(string permission)
    {
        homePermissionId = permission;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.Result != null)
        {
            return;
        }

        if (string.IsNullOrEmpty(homePermissionId))
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Invalid permission",
                Message = "Permission id is required"
            })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
            return;
        }

        var homeId = (string)context.RouteData.Values["homeId"];
        if (homeId == null)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Invalid home id",
                Message = "Home id is required"
            })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
            return;
        }

        var parsedHomeId = Guid.Parse(homeId);

        var homePermissionService = context.HttpContext.RequestServices.GetRequiredService<IHomePermissionLogic>();
        var userLogged = context.HttpContext.Items["User"];
        if (userLogged == null)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Unauthenticated",
                Message = "You are not authenticated"
            })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            return;
        }

        var userLoggedMapped = (User)userLogged;
        var homePermissionIdGuid = Guid.Parse(homePermissionId);
        var userLoggedMappedId = (Guid)userLoggedMapped.Id;
        if (userLoggedMappedId == null)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Invalid user id",
                Message = "User id is required"
            })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
            return;
        }

        var hasPermission = homePermissionService.HasPermission(userLoggedMappedId, parsedHomeId, homePermissionIdGuid);
        if (!hasPermission)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Unauthorized",
                Message = "You are not authorized to perform this action"
            })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
        }
    }
}
