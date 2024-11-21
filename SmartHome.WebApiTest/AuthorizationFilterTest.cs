using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
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

    [TestMethod]
    public void OnAuthorization_MissingPermissionId_ReturnsBadRequest()
    {
        _attribute = new AuthorizationFilter(null);

        _attribute.OnAuthorization(_context!);

        Assert.IsInstanceOfType(_context.Result, typeof(ObjectResult));
        var result = (ObjectResult)_context.Result;

        Assert.AreEqual(400, result.StatusCode);
    }

    [TestMethod]
    public void OnAuthorization_UserNotAuthenticated_ReturnsUnauthorized()
    {
        var items = new Dictionary<object, object>
        {
            { UserStatic.User, null }
        };
        _httpContextMock.Setup(h => h.Items).Returns(items);

        _attribute!.OnAuthorization(_context!);

        Assert.IsInstanceOfType(_context.Result, typeof(ObjectResult));
        var result = (ObjectResult)_context.Result;

        Assert.AreEqual(401, result.StatusCode);
    }

    [TestMethod]
    public void OnAuthorization_UserDoesNotHavePermission_ReturnsForbidden()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Role = new Role { Name = "TestRole" },
            Name = "TestUser",
            Email = "t@Test.Com",
            Password = "TestPassword",
            Surname = "TestSurname"
        };

        var sysPermission = new SystemPermission
        {
            Id = Guid.NewGuid(),
            Name = "TestPermission",
            Description = "TestDescription"
        };

        var permissionId = sysPermission.Id;
        _attribute = new AuthorizationFilter(permissionId.ToString());

        var items = new Dictionary<object, object>
        {
            { UserStatic.User, user }
        };
        _httpContextMock.Setup(h => h.Items).Returns(items);

        var permissionLogicMock = new Mock<ISystemPermissionLogic>();
        permissionLogicMock.Setup(p => p.GetSystemPermissionById(permissionId))
            .Returns(sysPermission);

        var roleLogicMock = new Mock<IRoleLogic>();
        roleLogicMock.Setup(r => r.HasPermission(user.Role.Id, permissionId))
            .Returns(false);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(permissionLogicMock.Object);
        serviceCollection.AddSingleton(roleLogicMock.Object);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        _httpContextMock.Setup(h => h.RequestServices)
            .Returns(serviceProvider);

        _attribute.OnAuthorization(_context!);

        Assert.IsInstanceOfType(_context.Result, typeof(ObjectResult));
        var result = (ObjectResult)_context.Result;

        Assert.AreEqual(403, result.StatusCode);
    }

    [TestMethod]
    public void OnAuthorization_UserHasPermission_ProceedsSuccessfully()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Role = new Role { Name = "TestRole" },
            Name = "TestUser",
            Email = "t@Test.Com",
            Password = "TestPassword",
            Surname = "TestSurname"
        };

        var sysPermission = new SystemPermission
        {
            Id = Guid.NewGuid(),
            Name = "TestPermission",
            Description = "TestDescription"
        };

        var permissionId = sysPermission.Id;
        _attribute = new AuthorizationFilter(permissionId.ToString());

        var items = new Dictionary<object, object>
        {
            { UserStatic.User, user }
        };
        _httpContextMock.Setup(h => h.Items).Returns(items);

        var permissionLogicMock = new Mock<ISystemPermissionLogic>();
        permissionLogicMock.Setup(p => p.GetSystemPermissionById(permissionId))
            .Returns(sysPermission);

        var roleLogicMock = new Mock<IRoleLogic>();
        roleLogicMock.Setup(r => r.HasPermission(user.Role.Id, permissionId))
            .Returns(true);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(permissionLogicMock.Object);
        serviceCollection.AddSingleton(roleLogicMock.Object);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        _httpContextMock.Setup(h => h.RequestServices).Returns(serviceProvider);

        _attribute.OnAuthorization(_context!);

        Assert.IsNull(_context.Result);
    }

    [TestMethod]
    public void OnAuthorization_ErrorGettingPermission_ReturnsBadRequest()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Role = new Role { Name = "TestRole" },
            Name = "TestUser",
            Email = "t@Test.Com",
            Password = "TestPassword",
            Surname = "TestSurname"
        };

        var permissionId = Guid.NewGuid();
        _attribute = new AuthorizationFilter(permissionId.ToString());

        var items = new Dictionary<object, object>
    {
        { UserStatic.User, user }
    };
        _httpContextMock.Setup(h => h.Items).Returns(items);

        _permissionServiceMock!.Setup(p => p.GetSystemPermissionById(permissionId))
            .Throws(new Exception("Database error"));

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(_permissionServiceMock.Object);
        serviceCollection.AddSingleton(_roleServiceMock!.Object);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        _httpContextMock.Setup(h => h.RequestServices).Returns(serviceProvider);

        try
        {
            _attribute!.OnAuthorization(_context!);
        }
        catch (Exception ex)
        {
            Assert.AreEqual("Database error", ex.Message);
        }
    }
}
