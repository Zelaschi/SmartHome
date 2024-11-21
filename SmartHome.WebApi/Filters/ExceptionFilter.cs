using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.DataAccess.CustomExceptions;

namespace SmartHome.WebApi.Filters;

public sealed class ExceptionFilter : IExceptionFilter
{
    private readonly Dictionary<Type, Func<Exception, IActionResult>> _errors = new ()
    {
        {
            typeof(ValidatorException),
            exception => new ObjectResult(new { ErrorMessage = exception.Message })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            }
        },
        {
            typeof(RoomException),
            exception => new ObjectResult(new { ErrorMessage = exception.Message })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            }
        },
        {
            typeof(BusinessException),
            exception => new ObjectResult(new { ErrorMessage = exception.Message })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            }
        },
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
        var response = _errors.GetValueOrDefault(context.Exception.GetType());

        if (response == null)
        {
            context.Result =
                new ObjectResult(new
                {
                    ErrorMessage = "An error occurred"
                })
                { StatusCode = (int)HttpStatusCode.InternalServerError };
        }
        else
        {
            context.Result = response.Invoke(context.Exception);
        }
    }
}
