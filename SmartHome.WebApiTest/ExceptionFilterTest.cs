using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.DataAccess.CustomExceptions;
using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Test;

[TestClass]
public class ExceptionFilterTests
{
    private ExceptionContext? _context;
    private readonly ExceptionFilter _attribute;

    public ExceptionFilterTests()
    {
        _attribute = new ExceptionFilter();
    }

    [TestInitialize]
    public void Initialize()
    {
        _context = new ExceptionContext(
            new ActionContext(
                new Mock<HttpContext>().Object,
                new RouteData(),
                new ActionDescriptor()),
            new List<IFilterMetadata>());
    }

    [TestMethod]
    public void OnException_WhenExceptionIsNotRegistered_ShouldResponseInternalError()
    {
        _context.Exception = new Exception("Not registered");

        _attribute.OnException(_context);

        var response = _context.Result;

        Assert.IsNotNull(response, "The response should not be null.");

        var concreteResponse = response as ObjectResult;
        Assert.IsNotNull(concreteResponse, "The response should be of type ObjectResult.");

        Assert.IsNotNull(concreteResponse.Value, "The value should not be null.");

        Assert.AreEqual((int)HttpStatusCode.InternalServerError, concreteResponse.StatusCode,
            "The status code should be 500 Internal Server Error.");

        Assert.AreEqual("Something went wrong. See: System.Exception Not registered", GetMessage(concreteResponse.Value));
    }

    [TestMethod]
    public void OnException_WhenUnauthorizedAccessException_ShouldResponseUnauthorized()
    {
        _context.Exception = new UnauthorizedAccessException("Unauthorized access");

        _attribute.OnException(_context);

        var response = _context.Result;

        Assert.IsNotNull(response, "The response should not be null.");

        var concreteResponse = response as ObjectResult;
        Assert.IsNotNull(concreteResponse, "The response should be of type ObjectResult.");

        Assert.AreEqual((int)HttpStatusCode.Unauthorized, concreteResponse.StatusCode,
            "The status code should be 401 Unauthorized.");

        Assert.AreEqual("Unauthorized access", GetMessage(concreteResponse!.Value!));
    }

    [TestMethod]
    public void OnException_WhenDeviceImporterException_ShouldResponseBadRequest()
    {
        _context.Exception = new DeviceImporterException("Device importer error");

        _attribute.OnException(_context);

        var response = _context.Result;

        Assert.IsNotNull(response, "The response should not be null.");

        var concreteResponse = response as ObjectResult;
        Assert.IsNotNull(concreteResponse, "The response should be of type ObjectResult.");

        Assert.AreEqual((int)HttpStatusCode.BadRequest, concreteResponse.StatusCode,
            "The status code should be 400 Bad Request.");

        Assert.AreEqual("Device importer error", GetMessage(concreteResponse!.Value!));
    }

    [TestMethod]
    public void OnException_WhenSessionException_ShouldResponseBadRequest()
    {
        _context.Exception = new SessionException("Session error");

        _attribute.OnException(_context);

        var response = _context.Result;

        Assert.IsNotNull(response, "The response should not be null.");

        var concreteResponse = response as ObjectResult;
        Assert.IsNotNull(concreteResponse, "The response should be of type ObjectResult.");

        Assert.AreEqual((int)HttpStatusCode.BadRequest, concreteResponse.StatusCode,
            "The status code should be 400 Bad Request.");

        Assert.AreEqual("Session error", GetMessage(concreteResponse!.Value!));
    }

    [TestMethod]
    public void OnException_WhenArgumentNullException_ShouldResponseBadRequest()
    {
        _context.Exception = new ArgumentNullException("Parameter name", "Argument cannot be null");

        _attribute.OnException(_context);

        var response = _context.Result;

        Assert.IsNotNull(response, "The response should not be null.");

        var concreteResponse = response as ObjectResult;
        Assert.IsNotNull(concreteResponse, "The response should be of type ObjectResult.");

        Assert.AreEqual((int)HttpStatusCode.BadRequest, concreteResponse.StatusCode,
            "The status code should be 400 Bad Request.");

        Assert.AreEqual("Argument cannot be null (Parameter 'Parameter name')", GetMessage(concreteResponse!.Value!));
    }

    [TestMethod]
    public void OnException_WhenArgumentException_ShouldResponseBadRequest()
    {
        _context.Exception = new ArgumentException("Argument cannot be null", "Parameter name");

        _attribute.OnException(_context);

        var response = _context.Result;

        Assert.IsNotNull(response, "The response should not be null.");

        var concreteResponse = response as ObjectResult;
        Assert.IsNotNull(concreteResponse, "The response should be of type ObjectResult.");

        Assert.AreEqual((int)HttpStatusCode.BadRequest, concreteResponse.StatusCode,
            "The status code should be 400 Bad Request.");

        Assert.AreEqual("Argument cannot be null (Parameter 'Parameter name')", GetMessage(concreteResponse!.Value!));
    }

    [TestMethod]
    public void OnException_WhenNullReferenceException_ShouldResponseBadRequest()
    {
        _context.Exception = new NullReferenceException("Cannot refere to null");

        _attribute.OnException(_context);

        var response = _context.Result;

        Assert.IsNotNull(response, "The response should not be null.");

        var concreteResponse = response as ObjectResult;
        Assert.IsNotNull(concreteResponse, "The response should be of type ObjectResult.");

        Assert.AreEqual((int)HttpStatusCode.BadRequest, concreteResponse.StatusCode,
            "The status code should be 400 Bad Request.");

        Assert.AreEqual("Cannot refere to null", GetMessage(concreteResponse!.Value!));
    }

    [TestMethod]
    public void OnException_WhenDatabaseException_ShouldResponseBadRequest()
    {
        _context.Exception = new DatabaseException("Data Base Exception");

        _attribute.OnException(_context);

        var response = _context.Result;

        Assert.IsNotNull(response, "The response should not be null.");

        var concreteResponse = response as ObjectResult;
        Assert.IsNotNull(concreteResponse, "The response should be of type ObjectResult.");

        Assert.AreEqual((int)HttpStatusCode.BadRequest, concreteResponse.StatusCode,
            "The status code should be 400 Bad Request.");

        Assert.AreEqual("Data Base Exception", GetMessage(concreteResponse!.Value!));
    }

    [TestMethod]
    public void OnException_WhenDeviceException_ShouldResponseBadRequest()
    {
        _context.Exception = new DeviceException("Device Exception");

        _attribute.OnException(_context);

        var response = _context.Result;

        Assert.IsNotNull(response, "The response should not be null.");

        var concreteResponse = response as ObjectResult;
        Assert.IsNotNull(concreteResponse, "The response should be of type ObjectResult.");

        Assert.AreEqual((int)HttpStatusCode.BadRequest, concreteResponse.StatusCode,
            "The status code should be 400 Bad Request.");

        Assert.AreEqual("Device Exception", GetMessage(concreteResponse!.Value!));
    }

    private string GetMessage(object value)
    {
        return value.GetType().GetProperty("ErrorMessage").GetValue(value).ToString();
    }
}
