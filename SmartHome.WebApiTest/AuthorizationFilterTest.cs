using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SmartHome.BusinessLogic.Constants;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Test;

[TestClass]
public class AuthorizationFilterTests
{
    private Mock<HttpContext>? _httpContextMock;
    private Mock<ISystemPermissionLogic>? _permissionServiceMock;
    private Mock<IRoleLogic>? _roleServiceMock;
    private AuthorizationFilterContext? _context;
    private AuthorizationFilter? _attribute;

    [TestInitialize]
    public void Initialize()
    {
        _httpContextMock = new Mock<HttpContext>();
        _permissionServiceMock = new Mock<ISystemPermissionLogic>();
        _roleServiceMock = new Mock<IRoleLogic>();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(_permissionServiceMock.Object);
        serviceCollection.AddSingleton(_roleServiceMock.Object);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        _httpContextMock.Setup(h => h.RequestServices).Returns(serviceProvider);

        _context = new AuthorizationFilterContext(
            new ActionContext(
                _httpContextMock.Object,
                new RouteData(),
                new ActionDescriptor()
            ),
            new List<IFilterMetadata>()
        );

        _attribute = new AuthorizationFilter("test-permission-id");
    }

    [TestMethod]
    public void OnAuthorization_AlreadyHasResult_DoesNothing()
    {
        _context!.Result = new ObjectResult("Existing Error")
        {
            StatusCode = 400
        };

        _attribute!.OnAuthorization(_context);

        Assert.AreEqual(400, ((ObjectResult)_context.Result).StatusCode);
        Assert.AreEqual("Existing Error", ((ObjectResult)_context.Result).Value);
    }
}

////    [TestMethod]
////    public void OnAuthorization_MissingPermissionId_ReturnsBadRequest()
////    {
////        // Arrange
////        var filter = new CustomAuthorizationFilter(null);

////        // Act
////        filter.OnAuthorization(_context);

////        // Assert
////        Assert.IsInstanceOfType(_context.Result, typeof(ObjectResult));
////        var result = (ObjectResult)_context.Result;

////        Assert.AreEqual(400, result.StatusCode);
////        Assert.AreEqual("Invalid permission", ((dynamic)result.Value).InnerCode);
////        Assert.AreEqual("Permission id is required", ((dynamic)result.Value).Message);
////    }

////    [TestMethod]
////    public void OnAuthorization_UserNotAuthenticated_ReturnsUnauthorized()
////    {
////        // Arrange
////        var filter = new CustomAuthorizationFilter("valid-permission-id");

////        // Act
////        filter.OnAuthorization(_context);

////        // Assert
////        Assert.IsInstanceOfType(_context.Result, typeof(ObjectResult));
////        var result = (ObjectResult)_context.Result;

////        Assert.AreEqual(401, result.StatusCode);
////        Assert.AreEqual("Unauthenticated", ((dynamic)result.Value).InnerCode);
////        Assert.AreEqual("You are not authenticated", ((dynamic)result.Value).Message);
////    }

////    [TestMethod]
////    public void OnAuthorization_MissingPermission_ReturnsForbidden()
////    {
////        // Arrange
////        var user = new User
////        {
////            Id = Guid.NewGuid(),
////            Role = new Role { Id = Guid.NewGuid() }
////        };
////        _httpContext.Items[UserStatic.User] = user;

////        var permissionId = Guid.NewGuid();
////        var permission = new Permission
////        {
////            Id = permissionId,
////            Name = "Test Permission"
////        };

////        _permissionServiceMock
////            .Setup(x => x.GetSystemPermissionById(permissionId))
////            .Returns(permission);

////        _roleServiceMock
////            .Setup(x => x.HasPermission(user.Role.Id, permission.Id))
////            .Returns(false);

////        var filter = new CustomAuthorizationFilter(permissionId.ToString());

////        // Act
////        filter.OnAuthorization(_context);

////        // Assert
////        Assert.IsInstanceOfType(_context.Result, typeof(ObjectResult));
////        var result = (ObjectResult)_context.Result;

////        Assert.AreEqual(403, result.StatusCode);
////        Assert.AreEqual("Forbidden", ((dynamic)result.Value).InnerCode);
////        Assert.AreEqual($"Missing permission {permission.Name}", ((dynamic)result.Value).Message);

////        _permissionServiceMock.VerifyAll();
////        _roleServiceMock.VerifyAll();
////    }

////    [TestMethod]
////    public void OnAuthorization_ExceptionInRoleService_ReturnsBadRequest()
////    {
////        // Arrange
////        var user = new User
////        {
////            Id = Guid.NewGuid(),
////            Role = new Role { Id = Guid.NewGuid() }
////        };
////        _httpContext.Items[UserStatic.User] = user;

////        var permissionId = Guid.NewGuid();
////        var permission = new Permission
////        {
////            Id = permissionId,
////            Name = "Test Permission"
////        };

////        _permissionServiceMock
////            .Setup(x => x.GetSystemPermissionById(permissionId))
////            .Returns(permission);

////        _roleServiceMock
////            .Setup(x => x.HasPermission(user.Role.Id, permission.Id))
////            .Throws(new Exception());

////        var filter = new CustomAuthorizationFilter(permissionId.ToString());

////        // Act
////        filter.OnAuthorization(_context);

////        // Assert
////        Assert.IsInstanceOfType(_context.Result, typeof(ObjectResult));
////        var result = (ObjectResult)_context.Result;

////        Assert.AreEqual(400, result.StatusCode);
////        Assert.AreEqual("Forbidden", ((dynamic)result.Value).InnerCode);
////        Assert.AreEqual("HomeId does not exist", ((dynamic)result.Value).Message);

////        _permissionServiceMock.VerifyAll();
////        _roleServiceMock.VerifyAll();
////    }
////}
