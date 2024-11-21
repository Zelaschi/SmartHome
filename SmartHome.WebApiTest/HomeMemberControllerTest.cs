using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Constants;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.HomeMemberModels.In;
using SmartHome.WebApi.WebModels.HomePermissionModels.Out;

namespace SmartHome.WebApi.Test;

[TestClass]
public class HomeMemberControllerTest
{
    private Mock<IHomeMemberLogic>? homeMemberLogicMock;
    private HomeMembersController? homeMemberController;
    private readonly Role homeOwner = new Role
    {
        Name = "HomeOwner"
    };

    [TestInitialize]
    public void TestInitialize()
    {
        homeMemberLogicMock = new Mock<IHomeMemberLogic>(MockBehavior.Strict);
        homeMemberController = new HomeMembersController(homeMemberLogicMock.Object);
    }

    [TestMethod]
    public void UpdateHomePermissionsOfHomeMemberTest_Ok()
    {
        var user1 = new BusinessLogic.Domain.User
        {
            Id = Guid.NewGuid(),
            Name = "a",
            Surname = "b",
            Password = "psw1",
            Email = "mail@mail.com",
            Role = homeOwner,
            CreationDate = DateTime.Today
        };

        homeMemberController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        homeMemberController.ControllerContext.HttpContext.Items[UserStatic.User] = user1;

        var homeMemberPermissionsModel = new HomeMemberPermissions
        {
            AddDevicePermission = true,
            AddMemberPermission = true,
            ListDevicesPermission = false,
            NotificationsPermission = true
        };

        homeMemberLogicMock.Setup(h =>
            h.UpdateHomePermissionsOfHomeMember(
                It.IsAny<Guid>(),
                It.IsAny<List<HomePermission>>(),
                It.IsAny<Guid>())
        );

        var result = homeMemberController.UpdateHomeMemberPermissions(
            Guid.NewGuid(),
            homeMemberPermissionsModel) as NoContentResult;

        homeMemberLogicMock.VerifyAll();
        Assert.IsNotNull(result);
        Assert.AreEqual(204, result.StatusCode);
    }

    [TestMethod]
    public void HomeMembersController_NullHomeMemberLogic_ThrowsArgumentNullException()
    {
        try
        {
            var controller = new HomeMembersController(null);
        }
        catch (ArgumentNullException ex)
        {
            Assert.AreEqual("homeMemberLogic", ex.ParamName);
        }
    }

    [TestMethod]
    public void GetAllHomePermissionsTest_Ok()
    {
        var user1 = new User
        {
            Id = Guid.NewGuid(),
            Name = "a",
            Surname = "b",
            Password = "psw1",
            Email = "mail1@mail.com",
            Role = homeOwner,
            CreationDate = DateTime.Today
        };
        var homePermission1 = new HomePermission
        {
            Id = Guid.NewGuid(),
            Name = "AddMemberPermission"
        };
        var homePermission2 = new HomePermission
        {
            Id = Guid.NewGuid(),
            Name = "AddDevicePermission"
        };
        var homePermissionsList = new List<HomePermission>
        {
            homePermission1,
            homePermission2
        };

        homeMemberLogicMock.Setup(h => h.GetAllHomePermissions())
            .Returns(homePermissionsList);

        var result = homeMemberController.GetAllHomePermissions() as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);

        var response = result.Value as List<HomePermissionResponseModel>;
        Assert.IsNotNull(response);
        Assert.AreEqual(2, response.Count);
        Assert.AreEqual(homePermission1.Name, response[0].Name);
        Assert.AreEqual(homePermission2.Name, response[1].Name);

        homeMemberLogicMock.Verify(h => h.GetAllHomePermissions(), Times.Once);
    }

    [TestMethod]
    public void UpdateHomeMemberPermissions_UserMissing_ReturnsUnauthorized()
    {
        var httpContext = new DefaultHttpContext();
        homeMemberController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        var homeMemberId = Guid.NewGuid();
        var permissions = new HomeMemberPermissions();

        var result = homeMemberController.UpdateHomeMemberPermissions(homeMemberId, permissions);

        Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
        var unauthorizedResult = result as UnauthorizedObjectResult;
        Assert.AreEqual("User is missing", unauthorizedResult?.Value);
    }
}
