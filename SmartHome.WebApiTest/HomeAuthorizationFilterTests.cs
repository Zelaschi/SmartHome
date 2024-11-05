using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SmartHome.WebApi.Filters;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Domain;
using System.Net;
using SmartHome.BusinessLogic.Constants;

namespace SmartHome.WebApi.Test;

[TestClass]
public class HomeAuthorizationFilterTests
{
    private Mock<HttpContext>? _httpContextMock;
    private Mock<IHomePermissionLogic>? _homePermissionServiceMock;
    private AuthorizationFilterContext? _context;
    private HomeAuthorizationFilter? _attribute;

    [TestInitialize]
    public void Initialize()
    {
        _httpContextMock = new Mock<HttpContext>();
        _homePermissionServiceMock = new Mock<IHomePermissionLogic>();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(_homePermissionServiceMock.Object);
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
    public void OnAuthorization_WhenHomePermissionIdIsEmpty_ShouldReturnInvalidPermissionResponse()
    {
        _attribute = new HomeAuthorizationFilter(string.Empty);
        Assert.IsNotNull(_context);

        _attribute.OnAuthorization(_context);

        var response = _context.Result;
        Assert.IsNotNull(response, "Response should not be null.");
        var concreteResponse = response as ObjectResult;
        Assert.IsNotNull(concreteResponse.Value);
        Assert.IsNotNull(concreteResponse, "Response should be of type ObjectResult.");
        Assert.AreEqual((int)HttpStatusCode.BadRequest, concreteResponse.StatusCode, "Status code should be 400 Bad Request.");
        Assert.AreEqual("Invalid permission", GetInnerCode(concreteResponse.Value), "InnerCode should be 'Invalid permission'.");
        Assert.AreEqual("Permission id is required", GetMessage(concreteResponse.Value), "Message should be 'Permission id is required'.");
    }

    [TestMethod]
    public void OnAuthorization_WhenHomeIdIsMissing_ShouldReturnInvalidHomeIdResponse()
    {
        _attribute = new HomeAuthorizationFilter("some-permission-id");
        Assert.IsNotNull(_context);
        _attribute.OnAuthorization(_context);

        var response = _context.Result;
        Assert.IsNotNull(response, "Response should not be null.");
        var concreteResponse = response as ObjectResult;
        Assert.IsNotNull(concreteResponse.Value);
        Assert.IsNotNull(concreteResponse, "Response should be of type ObjectResult.");
        Assert.AreEqual((int)HttpStatusCode.BadRequest, concreteResponse.StatusCode, "Status code should be 400 Bad Request.");
        Assert.AreEqual("Invalid home id", GetInnerCode(concreteResponse.Value), "InnerCode should be 'Invalid home id'.");
        Assert.AreEqual("Home id is required", GetMessage(concreteResponse.Value), "Message should be 'Home id is required'.");
    }

    [TestMethod]
    public void OnAuthorization_WhenUserIsNotAuthenticated_ShouldReturnUnauthenticatedResponse()
    {
        // Arrange
        var permissionGuid = Guid.NewGuid();
        _attribute = new HomeAuthorizationFilter(permissionGuid.ToString());
        _httpContextMock.Setup(h => h.Items[UserStatic.User]).Returns(new User { Id = Guid.NewGuid(), Email = "email@mail.com", Name = "Pedro", Password = "Password1", Surname = "Azambuja" });

        var homeGuid = Guid.NewGuid();
        _context.RouteData.Values["homeId"] = homeGuid.ToString();

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
        Assert.AreEqual("Unauthorized", GetInnerCode(concreteResponse.Value), "InnerCode should be 'Unauthenticated'.");
    }

    [TestMethod]
    public void OnAuthorization_WhenUserIdIsInvalid_ShouldReturnInvalidUserIdResponse()
    {
        // Arrange
        var permissionGuid = Guid.NewGuid();
        _attribute = new HomeAuthorizationFilter(permissionGuid.ToString());
        var homeGuid = Guid.NewGuid();
        _context.RouteData.Values["homeId"] = homeGuid.ToString();
        _httpContextMock.Setup(h => h.Items[UserStatic.User]).Returns(new User { Id = Guid.NewGuid(), Email = "email@mail.com", Name = "Pedro", Password = "Password1", Surname = "Azambuja" });

        Assert.IsNotNull(_context);

        // Act
        _attribute.OnAuthorization(_context);

        // Assert
        var response = _context.Result;
        Assert.IsNotNull(response, "Response should not be null.");
        var concreteResponse = response as ObjectResult;
        Assert.IsNotNull(concreteResponse, "Response should be of type ObjectResult.");
        Assert.IsNotNull(concreteResponse.Value, "Response value should not be null.");
        Assert.AreEqual((int)HttpStatusCode.Unauthorized, concreteResponse.StatusCode, "Status code should be 401 Unauthorized.");
        Assert.AreEqual("Unauthorized", GetInnerCode(concreteResponse.Value), "InnerCode should be 'Unauthorized'.");
    }

    [TestMethod]
    public void OnAuthorization_WhenUserDoesNotHavePermission_ShouldReturnUnauthorizedResponse()
    {
        _attribute = new HomeAuthorizationFilter(Guid.NewGuid().ToString());
        var guid = Guid.NewGuid();
        _context.RouteData.Values["homeId"] = guid.ToString();
        _httpContextMock.Setup(h => h.Items[UserStatic.User]).Returns(new User { Id = Guid.NewGuid(), Email = "email@mail.com", Name = "Pedro", Password = "Password1", Surname = "Azambuja" });

        _homePermissionServiceMock.Setup(s => s.HasPermission(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(false);

        Assert.IsNotNull(_context);
        _attribute.OnAuthorization(_context);

        var response = _context.Result;
        Assert.IsNotNull(response, "Response should not be null.");
        var concreteResponse = response as ObjectResult;
        Assert.IsNotNull(concreteResponse.Value);
        Assert.IsNotNull(concreteResponse, "Response should be of type ObjectResult.");
        Assert.AreEqual((int)HttpStatusCode.Unauthorized, concreteResponse.StatusCode, "Status code should be 401 Unauthorized.");
        Assert.AreEqual("Unauthorized", GetInnerCode(concreteResponse.Value), "InnerCode should be 'Unauthorized'.");
        Assert.AreEqual("You are not authorized to perform this action", GetMessage(concreteResponse.Value), "Message should be 'You are not authorized to perform this action'.");
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
