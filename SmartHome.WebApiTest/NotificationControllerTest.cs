using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.NotificationModels.Out;
using SmartHome.WebApi.WebModels.SecurityCameraModels.In;

namespace SmartHome.WebApiTest;

[TestClass]
public class NotificationControllerTest
{
    private Mock<INotificationLogic>? _notificationLogicMock;
    private NotificationController? _notificationController;
    private readonly Role homeOwner = new Role() { Name = "HomeOwner" };
    private readonly Role companyOwner = new Role() { Name = "CompanyOwner" };

    [TestInitialize]
    public void TestInitialize()
    {
        _notificationLogicMock = new Mock<INotificationLogic>();
        _notificationController = new NotificationController(_notificationLogicMock.Object);
    }

    [TestMethod]
    public void GetNotificationsByHomeMemberIdTest_OK()
    {
        var homeMemberId = Guid.NewGuid();
        var companyOwner1 = new User() { Id = Guid.NewGuid(), Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = companyOwner, CreationDate = DateTime.Today };
        var user1 = new User() { Id = Guid.NewGuid(), Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var home = new Home() { Id = Guid.NewGuid(), MainStreet = "Cuareim", DoorNumber = "1234", Latitude = "12", Longitude = "34", MaxMembers = 5, Owner = user1 };
        var company = new Business() { Id = Guid.NewGuid(), BusinessOwner = companyOwner1, Logo = "logo", Name = "hikvision", RUT = "1234" };
        var device1 = new Device() { Id = Guid.NewGuid(), Name = "Device1", Type = "Type1", Business = company, Description = "description", ModelNumber = "1234", Photos = "photos" };
        var homeDevice = new HomeDevice() { Id = Guid.NewGuid(), Device = device1, Online = true };

        var notifications = new List<Notification>
        {
            new Notification() { Id = Guid.NewGuid(), Event = "Event1", Date = DateTime.Today, HomeDevice = homeDevice, Time = "19:00" },
            new Notification() { Id = Guid.NewGuid(), Event = "Event2", Date = DateTime.Today, HomeDevice = homeDevice, Time = "19:00" },
            new Notification() { Id = Guid.NewGuid(), Event = "Event3", Date = DateTime.Today, HomeDevice = homeDevice, Time = "19:00" }
        };

        var homeMember = new HomeMember(user1) { HomeMemberId = homeMemberId, Notifications = notifications, HomePermissions = new List<HomePermission>() };

        _notificationLogicMock.Setup(n => n.GetNotificationsByHomeMemberId(homeMemberId)).Returns(notifications);

        var expected = new OkObjectResult(new List<Notification>
        {
            notifications.First(),
            notifications.ElementAt(1),
            notifications.Last()
        });

        var result = _notificationController.GetNotificationsByHomeMemberId(homeMemberId) as OkObjectResult;

        var objectResult = result.Value as List<Notification>;

        _notificationLogicMock.VerifyAll();

        Assert.IsNotNull(objectResult);

        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode) && objectResult.First().Equals(notifications.First()));
    }

    [TestMethod]

    public void Create_MovementDetectionNotification_TestOk()
    {
        var businessOwnerRole = new Role() { Name = "BusinessOwner" };
        var businessOwner = new User() { Id = Guid.NewGuid(), Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = businessOwnerRole, CreationDate = DateTime.Today };
        var company = new Business() { Id = Guid.NewGuid(), Name = "hikvision", Logo = "logo1", RUT = "rut1", BusinessOwner = businessOwner };
        var securityCamera = new SecurityCamera()
        {
            Name = "SecurityCamera",
            ModelNumber = "modelNumber",
            Description = "description",
            Photos = "photoPath",
            Indoor = true,
            Outdoor = false,
            MovementDetection = true,
            PersonDetection = true,
            Business = company,
        };

        var homeDevice = new HomeDevice() { Id = Guid.NewGuid(), Device = securityCamera, Online = true };

        var notification = new Notification() { Id = Guid.NewGuid(), Event = "MovementDetection", Date = DateTime.Today, HomeDevice = homeDevice, Time = "19:00" };

        var notificationResponseModel = new NotificationResponseModel(notification);

        _notificationLogicMock.Setup(n => n.CreateMovementDetectionNotification(It.IsAny<Guid>())).Returns(notification);

        var expected = new CreatedAtActionResult("CreateMovementDetectionNotification", "CreateMovementDetectionNotification", new { notificationResponseModel.Id }, notificationResponseModel);
        var result = _notificationController.CreateMovementDetectionNotification(homeDevice.Id) as CreatedAtActionResult;
        var objectResult = result.Value as NotificationResponseModel;

        _notificationLogicMock.VerifyAll();

        Assert.IsNotNull(objectResult);

        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode) && objectResult.Equals(notificationResponseModel));
    }

    [TestMethod]

    public void Create_PersonDetectionNotification_TestOk()
    {
        var businessOwnerRole = new Role() { Name = "BusinessOwner" };
        var businessOwner = new User() { Id = Guid.NewGuid(), Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = businessOwnerRole, CreationDate = DateTime.Today };
        var company = new Business() { Id = Guid.NewGuid(), Name = "hikvision", Logo = "logo1", RUT = "rut1", BusinessOwner = businessOwner };
        var securityCamera = new SecurityCamera()
        {
            Name = "SecurityCamera",
            ModelNumber = "modelNumber",
            Description = "description",
            Photos = "photoPath",
            Indoor = true,
            Outdoor = false,
            MovementDetection = true,
            PersonDetection = true,
            Business = company,
        };

        var homeDevice = new HomeDevice() { Id = Guid.NewGuid(), Device = securityCamera, Online = true };

        var notification = new Notification() { Id = Guid.NewGuid(), Event = "PersonDetection", Date = DateTime.Today, HomeDevice = homeDevice, Time = "19:00" };

        var notificationResponseModel = new NotificationResponseModel(notification);

        _notificationLogicMock.Setup(n => n.CreatePersonDetectionNotification(It.IsAny<Guid>())).Returns(notification);

        var expected = new CreatedAtActionResult("CreatePersonDetectionNotification", "CreatePersonDetectionNotification", new { notificationResponseModel.Id }, notificationResponseModel);
        var result = _notificationController.CreatePersonDetectionNotification(homeDevice.Id) as CreatedAtActionResult;
        var objectResult = result.Value as NotificationResponseModel;

        _notificationLogicMock.VerifyAll();

        Assert.IsNotNull(objectResult);

        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode) && objectResult.Equals(notificationResponseModel));
    }
}
