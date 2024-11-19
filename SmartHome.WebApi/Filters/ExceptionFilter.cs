using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.DataAccess.CustomExceptions;
using System.Net;

namespace SmartHome.WebApi.Filters;

public sealed class ExceptionFilter : IExceptionFilter
{
    private readonly Dictionary<Type, Func<Exception, IActionResult>> _exceptionHandlers = new ()
    {
        {
            typeof(UnauthorizedAccessException),
            exception => new ObjectResult(new { ErrorMessage = exception.Message })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            }
        },
        {
            typeof(DeviceImporterException),
            exception => new ObjectResult(new { ErrorMessage = exception.Message })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            }
        },
        {
            typeof(SessionException),
            exception => new ObjectResult(new { ErrorMessage = exception.Message })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            }
        },
        {
            typeof(DeviceException),
            exception => new ObjectResult(new { ErrorMessage = exception.Message })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            }
        },
        {
            typeof(UserException),
            exception => new ObjectResult(new { ErrorMessage = exception.Message })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            }
        },
        {
            typeof(HomeArgumentException),
            exception => new ObjectResult(new { ErrorMessage = exception.Message })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            }
        },
        {
            typeof(HomeException),
            exception => new ObjectResult(new { ErrorMessage = exception.Message })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            }
        },
        {
            typeof(RoleException),
            exception => new ObjectResult(new { ErrorMessage = exception.Message })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            }
        },
        {
            typeof(HomeDeviceException),
            exception => new ObjectResult(new { ErrorMessage = exception.Message })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            }
        },
        {
            typeof(DatabaseException),
            exception => new ObjectResult(new { ErrorMessage = exception.Message })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            }
        },
        {
            typeof(ArgumentNullException),
            exception => new ObjectResult(new { ErrorMessage = exception.Message })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            }
        },
        {
            typeof(ArgumentException),
            exception => new ObjectResult(new { ErrorMessage = exception.Message })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            }
        },
        {
            typeof(NullReferenceException),
            exception => new ObjectResult(new { ErrorMessage = exception.Message })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            }
        }
    };

    public void OnException(ExceptionContext context)
    {
        var exceptionType = context.Exception.GetType();
        if (_exceptionHandlers.TryGetValue(exceptionType, out var handler))
        {
            context.Result = handler(context.Exception);
        }
        else
        {
            context.Result = new ObjectResult(new
            {
                ErrorMessage = $"Something went wrong. See: {exceptionType} {context.Exception.Message}"
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }
}
