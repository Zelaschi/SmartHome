using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using SmartHome.BusinessLogic.Services;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthenticationFilter
        : Attribute,
        IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var authorizationHeader = context.HttpContext.Request.Headers[HeaderNames.Authorization];
        var sessionService = context.HttpContext.RequestServices.GetRequiredService<ISessionLogic>();
        if (string.IsNullOrEmpty(authorizationHeader))
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

        Guid authHeaderGuid;
        if (!Guid.TryParse(authorizationHeader, out authHeaderGuid))
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Invalid authorization token",
                Message = "Invalid authorization token format"
            })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            return;
        }

        var isSessionValid = sessionService.IsSessionValid(authHeaderGuid);
        if (!isSessionValid)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Not valid session",
                Message = "The token does not correspond to a session"
            })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            return;
        }

        try
        {
            var userOfAuthorization = sessionService.GetUserOfSession(authHeaderGuid);
            context.HttpContext.Items.Add("User", userOfAuthorization);
        }
        catch (Exception)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Invalid session",
                Message = "The token does not correspond to a valid session"
            })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
        }
    }
}
