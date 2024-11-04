using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
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

    private string GetMessage(object value)
    {
        return value.GetType().GetProperty("ErrorMessage").GetValue(value).ToString();
    }
}
