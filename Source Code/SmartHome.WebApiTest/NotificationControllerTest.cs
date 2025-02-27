﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.NotificationModels.Out;

namespace SmartHome.WebApi.Test;

[TestClass]
public class NotificationControllerTest
{
    private Mock<INotificationLogic>? _notificationLogicMock;
    private NotificationsController? _notificationController;

    [TestInitialize]
    public void TestInitialize()
    {
        _notificationLogicMock = new Mock<INotificationLogic>();
        _notificationController = new NotificationsController(_notificationLogicMock.Object);
    }

    [TestMethod]
    public void Create_MovementDetectionNotification_TestOk()
    {
        var businessOwnerRole = new Role
        {
            Name = "BusinessOwner"
        };
        var businessOwner = new User
        {
            Id = Guid.NewGuid(),
            Name = "a",
            Surname = "b",
            Password = "psw1",
            Email = "mail1@mail.com",
            Role = businessOwnerRole,
            CreationDate = DateTime.Today
        };
        var company = new Business
        {
            Id = Guid.NewGuid(),
            Name = "hikvision",
            Logo = "logo1",
            RUT = "rut1",
            BusinessOwner = businessOwner
        };
        var securityCamera = new SecurityCamera()
        {
            Name = "SecurityCamera",
            ModelNumber = "modelNumber",
            Description = "description",
            Photos = [],
            Indoor = true,
            Outdoor = false,
            MovementDetection = true,
            PersonDetection = true,
            Business = company,
        };

        var homeDevice = new HomeDevice
        {
            Id = Guid.NewGuid(),
            Device = securityCamera,
            Online = true,
            Name = securityCamera.Name
        };

        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            Event = "MovementDetection",
            Date = DateTime.Today,
            HomeDevice = homeDevice,
            Time = DateTime.Now
        };

        var notificationResponseModel = new NotificationResponseModel(notification);

        _notificationLogicMock.Setup(n => n.CreateMovementDetectionNotification(It.IsAny<Guid>()))
            .Returns(notification);

        var expected = new CreatedAtActionResult("CreateMovementDetectionNotification", "CreateMovementDetectionNotification", new { notificationResponseModel.Id }, notificationResponseModel);
        var result = _notificationController.CreateMovementDetectionNotification(homeDevice.Id) as CreatedAtActionResult;
        var objectResult = result.Value as NotificationResponseModel;

        _notificationLogicMock.VerifyAll();
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(result.StatusCode, expected.StatusCode);
        Assert.AreEqual(objectResult, notificationResponseModel);
    }

    [TestMethod]
    public void Create_PersonDetectionNotification_TestOk()
    {
        var businessOwnerRole = new Role
        {
            Name = "BusinessOwner"
        };
        var businessOwnerId = Guid.NewGuid();
        var businessOwner = new User
        {
            Id = businessOwnerId,
            Name = "a",
            Surname = "b",
            Password = "psw1",
            Email = "mail1@mail.com",
            Role = businessOwnerRole,
            CreationDate = DateTime.Today
        };
        var company = new Business
        {
            Id = Guid.NewGuid(),
            Name = "hikvision",
            Logo = "logo1",
            RUT = "rut1",
            BusinessOwner = businessOwner
        };
        var securityCamera = new SecurityCamera()
        {
            Name = "SecurityCamera",
            ModelNumber = "modelNumber",
            Description = "description",
            Photos = [],
            Indoor = true,
            Outdoor = false,
            MovementDetection = true,
            PersonDetection = true,
            Business = company,
        };

        var homeDevice = new HomeDevice
        {
            Id = Guid.NewGuid(),
            Device = securityCamera,
            Online = true,
            Name = securityCamera.Name
        };

        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            Event = "PersonDetection",
            Date = DateTime.Today,
            HomeDevice = homeDevice,
            Time = DateTime.Now
        };

        var notificationResponseModel = new NotificationResponseModel(notification);

        _notificationLogicMock.Setup(n => n.CreatePersonDetectionNotification(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .Returns(notification);

        var expected = new CreatedAtActionResult("CreatePersonDetectionNotification", "CreatePersonDetectionNotification", new { notificationResponseModel.Id }, notificationResponseModel);
        var result = _notificationController.CreatePersonDetectionNotification(homeDevice.Id, businessOwnerId) as CreatedAtActionResult;
        var objectResult = result.Value as NotificationResponseModel;

        _notificationLogicMock.VerifyAll();
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(result.StatusCode, expected.StatusCode);
        Assert.AreEqual(objectResult, notificationResponseModel);
    }

    [TestMethod]
    public void Create_OpenedWindowNotification_TestOk()
    {
        var businessOwnerRole = new Role
        {
            Name = "BusinessOwner"
        };
        var businessOwnerId = Guid.NewGuid();
        var businessOwner = new User
        {
            Id = businessOwnerId,
            Name = "a",
            Surname = "b",
            Password = "psw1",
            Email = "mail1@mail.com",
            Role = businessOwnerRole,
            CreationDate = DateTime.Today
        };
        var company = new Business
        {
            Id = Guid.NewGuid(),
            Name = "hikvision",
            Logo = "logo1",
            RUT = "rut1",
            BusinessOwner = businessOwner
        };
        var device = new Device()
        {
            Id = Guid.NewGuid(),
            Name = "WindowSensor",
            ModelNumber = "modelNumber",
            Description = "description",
            Photos = [],
            Business = company,
            Type = "Window Sensor"
        };

        var homeDevice = new HomeDevice
        {
            Id = Guid.NewGuid(),
            Device = device,
            Online = true,
            Name = device.Name
        };
        homeDevice.IsOpen = false;

        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            Event = "OpenedWindow",
            Date = DateTime.Today,
            HomeDevice = homeDevice,
            Time = DateTime.Now
        };

        var notificationResponseModel = new NotificationResponseModel(notification);

        _notificationLogicMock.Setup(n => n.CreateOpenCloseWindowNotification(It.IsAny<Guid>()))
            .Returns(notification);

        var expected = new CreatedAtActionResult("CreateOpenedWindowNotification", "CreateOpenedWindowNotification", new { notificationResponseModel.Id }, notificationResponseModel);
        var result = _notificationController.CreateOpenCloseWindowNotification(homeDevice.Id) as CreatedAtActionResult;
        var objectResult = result.Value as NotificationResponseModel;

        _notificationLogicMock.VerifyAll();
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(result.StatusCode, expected.StatusCode);
        Assert.AreEqual(objectResult, notificationResponseModel);
    }

    [TestMethod]
    public void Create_ClosedWindowNotification_TestOk()
    {
        var businessOwnerRole = new Role
        {
            Name = "BusinessOwner"
        };
        var businessOwnerId = Guid.NewGuid();
        var businessOwner = new User
        {
            Id = businessOwnerId,
            Name = "a",
            Surname = "b",
            Password = "psw1",
            Email = "mail1@mail.com",
            Role = businessOwnerRole,
            CreationDate = DateTime.Today
        };
        var company = new Business
        {
            Id = Guid.NewGuid(),
            Name = "hikvision",
            Logo = "logo1",
            RUT = "rut1",
            BusinessOwner = businessOwner
        };
        var device = new Device()
        {
            Id = Guid.NewGuid(),
            Name = "WindowSensor",
            ModelNumber = "modelNumber",
            Description = "description",
            Photos = [],
            Business = company,
            Type = "Window Sensor"
        };

        var homeDevice = new HomeDevice
        {
            Id = Guid.NewGuid(),
            Device = device,
            Online = true,
            Name = device.Name
        };
        homeDevice.IsOpen = true;

        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            Event = "ClosedWindow",
            Date = DateTime.Today,
            HomeDevice = homeDevice,
            Time = DateTime.Now
        };

        var notificationResponseModel = new NotificationResponseModel(notification);

        _notificationLogicMock.Setup(n => n.CreateOpenCloseWindowNotification(It.IsAny<Guid>()))
            .Returns(notification);

        var expected = new CreatedAtActionResult("CreateClosedWindowNotification", "CreateClosedWindowNotification", new { notificationResponseModel.Id }, notificationResponseModel);
        var result = _notificationController.CreateOpenCloseWindowNotification(homeDevice.Id) as CreatedAtActionResult;
        var objectResult = result.Value as NotificationResponseModel;

        _notificationLogicMock.VerifyAll();
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(result.StatusCode, expected.StatusCode);
        Assert.AreEqual(objectResult, notificationResponseModel);
    }
}
