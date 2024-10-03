using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthorizationFilter : Attribute, IAuthorizationFilter
{
    private readonly string permissionId;
    public AuthorizationFilter(string permission)
    {
        permissionId = permission;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.Result != null) // IF IT ALREADY HAS A RESULT/ERROR
        {
            return;
        }

        if (string.IsNullOrEmpty(permissionId))
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

        var sessionService = context.HttpContext.RequestServices.GetRequiredService<ISystemPermissionLogic>();
        var roleService = context.HttpContext.RequestServices.GetRequiredService<IRoleLogic>();
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
        var permissionIdGuid = Guid.Parse(permissionId);
        var systemPermission = sessionService.GetSystemPermissionById(permissionIdGuid);
        var hasPermission = roleService.HasPermission(userLoggedMapped.Role.Id, systemPermission.Id);
        if (!hasPermission)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Forbidden",
                Message = $"Missing permission {systemPermission.Name}"
            })
            {
                StatusCode = (int)HttpStatusCode.Forbidden
            };
        }
    }
}
