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
using SmartHome.WebApi.WebModels.HomeModels.Out;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace SmartHome.WebApiTest;
[TestClass]
public class MeControllerTest
{
    private Mock<INotificationLogic>? _notificationLogicMock;
    private  Mock<IHomeLogic>? _homeLogicMock;
    private MeController? _meController;
    private Role? homeOwner;

    [TestInitialize]
    public void TestInitialize()
    {
        _notificationLogicMock = new Mock<INotificationLogic>();
        _homeLogicMock = new Mock<IHomeLogic>();
        _meController = new MeController(_notificationLogicMock.Object, _homeLogicMock.Object);
        homeOwner = new Role() { Name = "HomeOwner" };
    }

    [TestMethod]
    public void GetNotificationsByHomeMemberIdTest_OK()
    {
        var companyOwner = new Role() { Name = "CompanyOwner" };
        var homeMemberId = Guid.NewGuid();
        var companyOwner1 = new BusinessLogic.Domain.User() { Id = Guid.NewGuid(), Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = companyOwner, CreationDate = DateTime.Today };
        var user1 = new BusinessLogic.Domain.User() { Id = Guid.NewGuid(), Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var home = new Home() { Id = Guid.NewGuid(), MainStreet = "Cuareim", DoorNumber = "1234", Latitude = "12", Longitude = "34", MaxMembers = 5, Owner = user1 };
        var company = new Business() { Id = Guid.NewGuid(), BusinessOwner = companyOwner1, Logo = "logo", Name = "hikvision", RUT = "1234" };
        var device1 = new BusinessLogic.Domain.Device() { Id = Guid.NewGuid(), Name = "Device1", Type = "Type1", Business = company, Description = "description", ModelNumber = "1234", Photos = "photos" };
        var homeDevice = new HomeDevice() { Id = Guid.NewGuid(), Device = device1, Online = true };

        var notifications = new List<Notification>
        {
            new Notification() { Id = Guid.NewGuid(), Event = "Event1", Date = DateTime.Today, HomeDevice = homeDevice, Time = DateTime.Now },
            new Notification() { Id = Guid.NewGuid(), Event = "Event2", Date = DateTime.Today, HomeDevice = homeDevice, Time = DateTime.Now },
            new Notification() { Id = Guid.NewGuid(), Event = "Event3", Date = DateTime.Today, HomeDevice = homeDevice, Time = DateTime.Now }
        };

        var homeMember = new HomeMember(user1) { HomeMemberId = homeMemberId, Notifications = notifications, HomePermissions = new List<HomePermission>() };

        _notificationLogicMock.Setup(n => n.GetUsersNotifications(user1)).Returns(notifications);

        var expected = new OkObjectResult(new List<Notification>
        {
            notifications.First(),
            notifications.ElementAt(1),
            notifications.Last()
        });

        HttpContext httpContext = new DefaultHttpContext();
        httpContext.Items.Add("User", user1);

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        _meController = new MeController(_notificationLogicMock.Object, _homeLogicMock.Object) { ControllerContext = controllerContext };

        var result = _meController.GetUsersNotifications() as OkObjectResult;

        var objectResult = result.Value as List<Notification>;

        _notificationLogicMock.VerifyAll();

        Assert.IsNotNull(objectResult);

        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode) && objectResult.First().Equals(notifications.First()));
    }

    [TestMethod]
    public void GetAllHomesByUserIdTest_Ok()
    {
        var user1Id = Guid.NewGuid();
        var user1 = new BusinessLogic.Domain.User() { Id = user1Id, Name = "a", Surname = "b", Password = "psw1", Email = "user1@gmail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var user2 = new BusinessLogic.Domain.User() { Id = Guid.NewGuid(), Name = "c", Surname = "d", Password = "psw2", Email = "user2@hotmail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var home1 = new Home() { Id = Guid.NewGuid(), MainStreet = "Cuareim", DoorNumber = "1234", Latitude = "12", Longitude = "34", MaxMembers = 5, Owner = user1 };
        var home2 = new Home() { Id = Guid.NewGuid(), MainStreet = "18 de Julio", DoorNumber = "5678", Latitude = "56", Longitude = "78", MaxMembers = 10, Owner = user2 };
        var homes = new List<Home>() { home1, home2 };
        user1.Houses = homes;

        IUsersLogic usersLogicMock = new Mock<IUsersLogic>().Object;
        _homeLogicMock.Setup(h => h.GetAllHomesByUserId(It.IsAny<Guid>())).Returns(homes);

        var expected = new OkObjectResult(new List<HomeResponseModel>
        {
            new HomeResponseModel(homes.First()),
            new HomeResponseModel(homes.Last())
        });
        HttpContext httpContext = new DefaultHttpContext();
        httpContext.Items.Add("User", user1);

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        _meController = new MeController(_notificationLogicMock.Object, _homeLogicMock.Object) { ControllerContext = controllerContext };
        var result = _meController.GetAllHomesByUserId() as OkObjectResult;
        var objectResult = (result.Value as List<HomeResponseModel>)!;

        var expectedObject = (expected.Value as List<HomeResponseModel>)!;
        _homeLogicMock.VerifyAll();

        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode) && expectedObject.First().Equals(objectResult.First()));
    }
}
