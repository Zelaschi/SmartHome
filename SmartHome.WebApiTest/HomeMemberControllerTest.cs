using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.HomeMemberModels.In;
using SmartHome.WebApi.WebModels.HomeMemberModels.Out;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace SmartHome.WebApiTest;

[TestClass]
public class HomeMemberControllerTest
{
    private Mock<IHomeMemberLogic>? homeMemberLogicMock;
    private HomeMembersController? homeMemberController;
    private readonly Role homeOwner = new Role() { Name = "HomeOwner" };

    [TestInitialize]
    public void TestInitialize()
    {
        homeMemberLogicMock = new Mock<IHomeMemberLogic>(MockBehavior.Strict);
        homeMemberController = new HomeMembersController(homeMemberLogicMock.Object);
    }

    [TestMethod]
    public void AddHomePermissionsToHomeMemberTest_Ok()
    {
        // ARRANGE
        var user1 = new BusinessLogic.Domain.User() { Id = Guid.NewGuid(), Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var home = new Home() {Id = Guid.NewGuid(), MainStreet = "Elm Street", DoorNumber = "4567", Latitude = "10", Longitude = "20", Owner = user1, MaxMembers = 4, Name = "Home Name" };
        var homeMember = new HomeMember(user1) {HomeMemberId = Guid.NewGuid(), HomePermissions = new List<HomePermission>(), Notifications = new List<Notification>()};
        var homeMemberPermissionsModel = new HomeMemberPermissions()
        {
            AddMemberPermission = true,
            AddDevicePermission = true,
            ListDevicesPermission = false,
            NotificationsPermission = true
        };

        homeMemberLogicMock.Setup(h => h.AddHomePermissionsToHomeMember(It.IsAny<Guid>(), It.IsAny<List<HomePermission>>()));

        // ACT
        var expected = new NoContentResult();
        var result = homeMemberController.AddHomePermissionsToHomeMember(homeMember.HomeMemberId, homeMemberPermissionsModel) as NoContentResult;

        // ASSERT
        homeMemberLogicMock.VerifyAll();
        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode));
    }

    [TestMethod]
    public void UpdateHomePermissionsOfHomeMemberTest_Ok()
    {
        // ARRANGE
        var user1 = new BusinessLogic.Domain.User() { Id = Guid.NewGuid(), Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var home = new Home() { Id = Guid.NewGuid(), MainStreet = "Elm Street", DoorNumber = "4567", Latitude = "10", Longitude = "20", Owner = user1, MaxMembers = 4, Name = "Home Name" };
        var homeMember = new HomeMember(user1) { HomeMemberId = Guid.NewGuid(), HomePermissions = new List<HomePermission>(), Notifications = new List<Notification>() };
        var homeMemberPermissionsModel = new HomeMemberPermissions()
        {
            AddMemberPermission = true,
            AddDevicePermission = true,
            ListDevicesPermission = false,
            NotificationsPermission = true
        };

        homeMemberLogicMock.Setup(h => h.UpdateHomePermissionsOfHomeMember(It.IsAny<Guid>(), It.IsAny<List<HomePermission>>()));

        // ACT
        var expected = new NoContentResult();
        var result = homeMemberController.UpdateHomeMemberPermissions(homeMember.HomeMemberId, homeMemberPermissionsModel) as NoContentResult;

        // ASSERT
        homeMemberLogicMock.VerifyAll();
        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode));
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
}
