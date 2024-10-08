using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SmartHome.WebApi.Filters;
using SmartHome.BusinessLogic.Interfaces;
using System.Net;

namespace SmartHome.WebApi.UnitTests;

[TestClass]
public class AuthenticationFilterTests
{
    private Mock<HttpContext>? _httpContextMock;
    private readonly Mock<ISessionLogic>? _sessionServiceMock;
    private AuthorizationFilterContext? _context;
    private readonly AuthenticationFilter? _attribute;

    public AuthenticationFilterTests()
    {
        _attribute = new AuthenticationFilter();
        _sessionServiceMock = new Mock<ISessionLogic>();
    }

    [TestInitialize]
    public void Initialize()
    {
        _httpContextMock = new Mock<HttpContext>();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(_sessionServiceMock.Object);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        _httpContextMock.Setup(h => h.RequestServices).Returns(serviceProvider);

        _context = new AuthorizationFilterContext(
            new ActionContext(
                _httpContextMock.Object,
                new RouteData(),
                new ActionDescriptor()),
            new List<IFilterMetadata>()
        );
    }

    #region Error Tests

    [TestMethod]
    public void OnAuthorization_WhenEmptyHeaders_ShouldReturnUnauthenticatedResponse()
    {
        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary());
        Assert.IsNotNull(_context);
        _attribute.OnAuthorization(_context);

        var response = _context.Result;
        Assert.IsNotNull(response, "Response should not be null.");
        var concreteResponse = response as ObjectResult;
        Assert.IsNotNull(concreteResponse.Value);
        Assert.IsNotNull(concreteResponse, "Response should be of type ObjectResult.");
        Assert.AreEqual((int)HttpStatusCode.Unauthorized, concreteResponse.StatusCode, "Status code should be 401 Unauthorized.");
        Assert.AreEqual("Unauthenticated", GetInnerCode(concreteResponse.Value), "InnerCode should be 'Unauthenticated'.");
        Assert.AreEqual("You are not authenticated", GetMessage(concreteResponse.Value), "Message should be 'You are not authenticated'.");
    }

    [TestMethod]
    public void OnAuthorization_WhenAuthorizationIsEmpty_ShouldReturnUnauthenticatedResponse()
    {
        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
        {
            { HeaderNames.Authorization, string.Empty }
        }));
        Assert.IsNotNull(_context);

        _attribute.OnAuthorization(_context);

        var response = _context.Result;
        Assert.IsNotNull(response, "Response should not be null.");
        var concreteResponse = response as ObjectResult;
        Assert.IsNotNull(concreteResponse.Value);
        Assert.IsNotNull(concreteResponse, "Response should be of type ObjectResult.");
        Assert.AreEqual((int)HttpStatusCode.Unauthorized, concreteResponse.StatusCode, "Status code should be 401 Unauthorized.");
        Assert.AreEqual("Unauthenticated", GetInnerCode(concreteResponse.Value), "InnerCode should be 'Unauthenticated'.");
        Assert.AreEqual("You are not authenticated", GetMessage(concreteResponse.Value), "Message should be 'You are not authenticated'.");
    }

    [TestMethod]
    public void OnAuthorization_WhenInvalidAuthorizationToken_ShouldReturnUnauthorizedResponse()
    {
        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
        {
            { HeaderNames.Authorization, "invalid-token" }
        }));
        Assert.IsNotNull(_context);
        _attribute.OnAuthorization(_context);

        var response = _context.Result;
        Assert.IsNotNull(response, "Response should not be null.");
        var concreteResponse = response as ObjectResult;
        Assert.IsNotNull(concreteResponse.Value);
        Assert.IsNotNull(concreteResponse, "Response should be of type ObjectResult.");
        Assert.AreEqual((int)HttpStatusCode.Unauthorized, concreteResponse.StatusCode, "Status code should be 401 Unauthorized.");
        Assert.AreEqual("Invalid authorization token", GetInnerCode(concreteResponse.Value), "InnerCode should be 'Invalid authorization token'.");
        Assert.AreEqual("Invalid authorization token format", GetMessage(concreteResponse.Value), "Message should be 'Invalid authorization token format'.");
    }

    [TestMethod]
    public void OnAuthorization_WhenSessionIsNotValid_ShouldReturnUnauthorizedResponse()
    {
        var authToken = Guid.NewGuid().ToString();
        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
        {
            { HeaderNames.Authorization, authToken }
        }));

        _sessionServiceMock.Setup(s => s.IsSessionValid(It.IsAny<Guid>())).Returns(false);
        Assert.IsNotNull(_context);
        _attribute.OnAuthorization(_context);

        var response = _context.Result;
        Assert.IsNotNull(response, "Response should not be null.");
        var concreteResponse = response as ObjectResult;
        Assert.IsNotNull(concreteResponse.Value);
        Assert.IsNotNull(concreteResponse, "Response should be of type ObjectResult.");
        Assert.AreEqual((int)HttpStatusCode.Unauthorized, concreteResponse.StatusCode, "Status code should be 401 Unauthorized.");
        Assert.AreEqual("Not valid session", GetInnerCode(concreteResponse.Value), "InnerCode should be 'Not valid session'.");
        Assert.AreEqual("The token does not correspond to a session", GetMessage(concreteResponse.Value), "Message should be 'The token does not correspond to a session'.");
    }

    #endregion

    private string GetInnerCode(object value)
    {
        return value.GetType().GetProperty("InnerCode").GetValue(value).ToString();
    }

    private string GetMessage(object value)
    {
        return value.GetType().GetProperty("Message").GetValue(value).ToString();
    }
}
