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
using SmartHome.BusinessLogic.Domain;
using Microsoft.AspNetCore.Mvc.Controllers;
using SmartHome.BusinessLogic.Constants;
using SmartHome.BusinessLogic.Services;

namespace SmartHome.WebApi.Test;

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

    [TestMethod]
    public void OnAuthorization_WhenExceptionInGetUserOfSession_ShouldReturnUnauthorizedResponse()
    {
        // Arrange
        var authToken = Guid.NewGuid().ToString();
        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
    {
        { HeaderNames.Authorization, authToken }
    }));

        // Configura el mock para que `GetUserOfSession` lance una excepción
        _sessionServiceMock.Setup(s => s.IsSessionValid(It.IsAny<Guid>())).Returns(true);
        _sessionServiceMock.Setup(s => s.GetUserOfSession(It.IsAny<Guid>())).Throws(new Exception());

        Assert.IsNotNull(_context);

        // Act
        _attribute.OnAuthorization(_context);

        // Assert
        var response = _context.Result;
        Assert.IsNotNull(response, "Response should not be null.");
        var concreteResponse = response as ObjectResult;
        Assert.IsNotNull(concreteResponse.Value);
        Assert.IsNotNull(concreteResponse, "Response should be of type ObjectResult.");
        Assert.AreEqual((int)HttpStatusCode.Unauthorized, concreteResponse.StatusCode, "Status code should be 401 Unauthorized.");
        Assert.AreEqual("Invalid session", GetInnerCode(concreteResponse.Value), "InnerCode should be 'Invalid session'.");
        Assert.AreEqual("The token does not correspond to a valid session", GetMessage(concreteResponse.Value), "Message should be 'The token does not correspond to a valid session'.");
    }

    [TestMethod]
    public void OnAuthorization_WhenSessionIsValid_ShouldAddUserToContext()
    {
        var authToken = Guid.NewGuid();

        var sp = new SystemPermission
        {
            Name = "Test Permission",
            Description = "Test Description"
        };

        var role = new Role
        {
            Name = "Test Role",
            SystemPermissions = new List<SystemPermission> { sp }
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Test User",
            Surname = "Test Surname",
            Password = "TestPassword123",
            Email = "testuser@example.com",
            Role = role
        };

        var services = new ServiceCollection();
        services.AddSingleton<ISessionLogic>(_sessionServiceMock.Object);
        var serviceProvider = services.BuildServiceProvider();

        var httpContext = new DefaultHttpContext
        {
            RequestServices = serviceProvider
        };
        httpContext.Request.Headers[HeaderNames.Authorization] = authToken.ToString();

        _sessionServiceMock.Setup(s => s.IsSessionValid(authToken)).Returns(true);
        _sessionServiceMock.Setup(s => s.GetUserOfSession(authToken)).Returns(user);

        var routeData = new RouteData();
        var actionDescriptor = new ActionDescriptor();

        var actionContext = new ActionContext(httpContext, routeData, actionDescriptor);
        _context = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());

        _attribute.OnAuthorization(_context);

        Assert.IsTrue(httpContext.Items.ContainsKey(UserStatic.User), "User should be added to the context.");
        Assert.AreEqual(user, httpContext.Items[UserStatic.User], "The user in the context should be the same as returned by GetUserOfSession.");
    }
}
