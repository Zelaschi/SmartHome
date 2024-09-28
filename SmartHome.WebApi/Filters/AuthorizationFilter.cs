using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.WebApi.Filters;

public sealed class AuthorizationFilter : Attribute, IAuthorizationFilter
{
    private List<string>? Roles { get; set; }

    public AuthorizationFilter(string[] roles)
    {
        Roles = new List<string>(roles);
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context != null)
        {
            var tokenString = context.HttpContext.Request.Headers["Authorization"].ToString();
            if (!String.IsNullOrEmpty(tokenString))
            {
                var token = Guid.Parse(tokenString);
                if (Roles.Count > 0)
                {
                    var authenticationService = (IAuthorizationLogic)context.HttpContext.RequestServices.GetService(typeof(IAuthorizationLogic));
                    var role = authenticationService.GetUserRoleByToken(token);
                    var userId = authenticationService.GetUserIdByToken(token);
                    if (Roles.Contains(role))
                    {
                        context.HttpContext.Items.Add("UserId", userId);
                    }
                    else
                    {
                        context.Result = new UnauthorizedResult();
                    }
                }
                else
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
            }
        }
        else
        {
            context.Result = new UnauthorizedResult();
        }
    }
}
