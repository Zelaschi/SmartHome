using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.DataAccess.CustomExceptions;

namespace SmartHome.WebApi.Filters;

public sealed class ExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        if (exception is UnauthorizedAccessException)
        {
            context.Result = new ObjectResult(new { ErrorMessage = context.Exception.Message }) { StatusCode = (int)HttpStatusCode.Unauthorized };
        }
        else if (exception is SessionException)
        {
            context.Result = new ObjectResult(new { ErrorMessage = context.Exception.Message }) { StatusCode = (int)HttpStatusCode.BadRequest };
        }
        else if (exception is DeviceException)
        {
            context.Result = new ObjectResult(new { ErrorMessage = context.Exception.Message }) { StatusCode = (int)HttpStatusCode.BadRequest };
        }
        else if (exception is UserException)
        {
            context.Result = new ObjectResult(new { ErrorMessage = context.Exception.Message }) { StatusCode = (int)HttpStatusCode.BadRequest };
        }
        else if (exception is HomeArgumentException)
        {
            context.Result = new ObjectResult(new { ErrorMessage = context.Exception.Message }) { StatusCode = (int)HttpStatusCode.BadRequest };
        }
        else if (exception is HomeException)
        {
            context.Result = new ObjectResult(new { ErrorMessage = context.Exception.Message }) { StatusCode = (int)HttpStatusCode.BadRequest };
        }
        else if (exception is RoleException)
        {
            context.Result = new ObjectResult(new { ErrorMessage = context.Exception.Message }) { StatusCode = (int)HttpStatusCode.BadRequest };
        }
        else if (exception is HomeDeviceException)
        {
            context.Result = new ObjectResult(new { ErrorMessage = context.Exception.Message }) { StatusCode = (int)HttpStatusCode.InternalServerError };
        }
        else if (exception is DatabaseException)
        {
            context.Result = new ObjectResult(new { ErrorMessage = context.Exception.Message }) { StatusCode = (int)HttpStatusCode.BadRequest };
        }
        else if (exception is ArgumentNullException)
        {
            context.Result = new ObjectResult(new { ErrorMessage = context.Exception.Message }) { StatusCode = (int)HttpStatusCode.BadRequest };
        }
        else if (exception is ArgumentException)
        {
            context.Result = new ObjectResult(new { ErrorMessage = context.Exception.Message }) { StatusCode = (int)HttpStatusCode.BadRequest };
        }
        else if (exception is NullReferenceException)
        {
            context.Result = new ObjectResult(new { ErrorMessage = context.Exception.Message }) { StatusCode = (int)HttpStatusCode.BadRequest };
        }
        else if (exception is Exception)
        {
            context.Result = new ObjectResult(new { ErrorMessage = $"Something went wrong. See: {context.Exception.GetType()} {context.Exception.Message}" }) { StatusCode = (int)HttpStatusCode.InternalServerError };
        }
    }
}
