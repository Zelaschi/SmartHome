using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SmartHome.WebApi.Filters;

public class ExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        if (exception is UnauthorizedAccessException)
        {
            context.Result = new ObjectResult(new { ErrorMessage = context.Exception.Message }) { StatusCode = 401 };
        }
        else if (exception is ArgumentNullException)
        {
            context.Result = new ObjectResult(new { ErrorMessage = context.Exception.Message }) { StatusCode = 400 };
        }
        else if (exception is ArgumentException)
        {
            context.Result = new ObjectResult(new { ErrorMessage = context.Exception.Message }) { StatusCode = 404 };
        }
        else if (exception is NullReferenceException)
        {
            context.Result = new ObjectResult(new { ErrorMessage = context.Exception.Message }) { StatusCode = 204 };
        }
        else if (exception is Exception)
        {
            context.Result = new ObjectResult(new { ErrorMessage = $"Something went wrong. See: {context.Exception.GetType()} {context.Exception.Message}" }) { StatusCode = 500 };
        }
    }
}
