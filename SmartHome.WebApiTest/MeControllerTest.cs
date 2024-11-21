using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Constants;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.DTOs;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.HomeModels.Out;
using SmartHome.WebApi.WebModels.NotificationModels.Out;

namespace SmartHome.WebApi.Test;
[TestClass]
public class MeControllerTest
{
    private Mock<INotificationLogic>? _notificationLogicMock;
    private Mock<IHomeLogic>? _homeLogicMock;
    private MeController? _meController;
    private Role? homeOwner;

    [TestInitialize]
    public void TestInitialize()
    {
        _notificationLogicMock = new Mock<INotificationLogic>();
        _homeLogicMock = new Mock<IHomeLogic>();
        _meController = new MeController(_notificationLogicMock.Object, _homeLogicMock.Object);
        homeOwner = new Role()
        {
            Name = "HomeOwner"
        };
    }

    [TestMethod]
    public void GetNotificationsByHomeMemberIdTest_OK()
    {
        var companyOwner = new Role
        {
            Name = "CompanyOwner"
        };
        var homeMemberId = Guid.NewGuid();
        var companyOwner1 = new User
        {
            Id = Guid.NewGuid(),
            Name = "a",
            Surname = "b",
            Password = "psw1",
            Email = "mail1@mail.com",
            Role = companyOwner,
            CreationDate = DateTime.Today
        };
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
        var device1 = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Device1",
            Type = "Type1",
            Business = new Business()
            {
                Id = Guid.NewGuid(),
                BusinessOwner = companyOwner1,
                Logo = "logo",
                Name = "hikvision",
                RUT = "1234"
            },
            Description = "description",
            ModelNumber = "1234",
            Photos = new List<Photo>()
        };
        var homeDevice = new HomeDevice
        {
            Id = Guid.NewGuid(),
            Device = device1,
            Online = true,
            Name = device1.Name
        };

        var notifications = new List<Notification>
        {
            new Notification
            {
                Id = Guid.NewGuid(),
                Event = "Event1",
                Date = DateTime.Today,
                HomeDevice = homeDevice,
                Time = DateTime.Now
            },
            new Notification
            {
                Id = Guid.NewGuid(),
                Event = "Event2",
                Date = DateTime.Today,
                HomeDevice = homeDevice,
                Time = DateTime.Now
            },
            new Notification
            {
                Id = Guid.NewGuid(),
                Event = "Event3",
                Date = DateTime.Today,
                HomeDevice = homeDevice,
                Time = DateTime.Now
            }
        };

        var notificationsDTO = notifications
            .Select(notification => new DTONotification()
            {
                Notification = notification,
                Read = false
            })
            .ToList();

        _notificationLogicMock.Setup(n => n.GetUsersNotifications(It.IsAny<User>()))
            .Returns(notificationsDTO);

        HttpContext httpContext = new DefaultHttpContext();
        httpContext.Items.Add(UserStatic.User, user1);

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        _meController = new MeController(_notificationLogicMock.Object, _homeLogicMock.Object)
        {
            ControllerContext = controllerContext
        };

        var result = _meController.GetUsersNotifications() as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);

        var objectResult = result.Value as List<MeNotificationResponseModel>;

        Assert.IsNotNull(objectResult);
        Assert.AreEqual(notifications.Count, objectResult.Count);

        for (var i = 0; i < notifications.Count; i++)
        {
            Assert.AreEqual(notifications[i].Event, objectResult[i].Event, $"Mismatch in Event at index {i}.");
            Assert.AreEqual(notifications[i].Date, objectResult[i].Date, $"Mismatch in Date at index {i}.");
        }

        _notificationLogicMock.VerifyAll();
    }

    [TestMethod]
    public void GetAllHomesByUserIdTest_Ok()
    {
        var user1Id = Guid.NewGuid();
        var user1 = new User
        {
            Id = user1Id,
            Name = "a",
            Surname = "b",
            Password = "psw1",
            Email = "user1@gmail.com",
            Role = homeOwner,
            CreationDate = DateTime.Today
        };
        var user2 = new User
        {
            Id = Guid.NewGuid(),
            Name = "c",
            Surname = "d",
            Password = "psw2",
            Email = "user2@hotmail.com",
            Role = homeOwner,
            CreationDate = DateTime.Today
        };
        var home1 = new Home
        {
            Id = Guid.NewGuid(),
            MainStreet = "Cuareim",
            DoorNumber = "1234",
            Latitude = "12",
            Longitude = "34",
            MaxMembers = 5,
            Owner = user1,
            Name = "Home Name"
        };
        var home2 = new Home
        {
            Id = Guid.NewGuid(),
            MainStreet = "18 de Julio",
            DoorNumber = "5678",
            Latitude = "56",
            Longitude = "78",
            MaxMembers = 10,
            Owner = user2,
            Name = "Home Name"
        };
        var homes = new List<Home>
        {
            home1,
            home2
        };
        user1.Houses = homes;

        IUsersLogic usersLogicMock = new Mock<IUsersLogic>().Object;
        _homeLogicMock.Setup(h => h.GetAllHomesByUserId(It.IsAny<Guid>()))
            .Returns(homes);

        var expected = new OkObjectResult(new List<HomeResponseModel>
        {
            new HomeResponseModel(homes.First()),
            new HomeResponseModel(homes.Last())
        });
        HttpContext httpContext = new DefaultHttpContext();
        httpContext.Items.Add(UserStatic.User, user1);

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        _meController = new MeController(_notificationLogicMock.Object, _homeLogicMock.Object)
        {
            ControllerContext = controllerContext
        };
        var result = _meController.GetAllHomesByUserId() as OkObjectResult;
        var objectResult = (result.Value as List<HomeResponseModel>)!;

        var expectedObject = (expected.Value as List<HomeResponseModel>)!;
        _homeLogicMock.VerifyAll();

        Assert.AreEqual(result.StatusCode, expected.StatusCode);
        Assert.AreEqual(expectedObject.First(), objectResult.First());
    }

    [TestMethod]
    public void GetAllHomesByUserId_UserIsMissing_ReturnsUnauthorized()
    {
        HttpContext httpContext = new DefaultHttpContext();
        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        _meController = new MeController(_notificationLogicMock.Object, _homeLogicMock.Object)
        {
            ControllerContext = controllerContext
        };

        var result = _meController.GetAllHomesByUserId() as UnauthorizedObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(401, result.StatusCode);
        Assert.AreEqual("UserId is missing", result.Value);
    }

    [TestMethod]
    public void GetAllHomesByUserId_UserIdIsNull_ReturnsUnauthorized()
    {
        var userWithNullId = new User
        {
            Id = null,
            Name = "a",
            Surname = "b",
            Password = "psw1",
            Email = "user1@gmail.com",
            Role = homeOwner,
            CreationDate = DateTime.Today
        };

        HttpContext httpContext = new DefaultHttpContext();
        httpContext.Items.Add(UserStatic.User, userWithNullId);
        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        _meController = new MeController(_notificationLogicMock.Object, _homeLogicMock.Object)
        {
            ControllerContext = controllerContext
        };

        var result = _meController.GetAllHomesByUserId() as UnauthorizedObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(401, result.StatusCode);
        Assert.AreEqual("UserId is missing", result.Value);
    }

    [TestMethod]
    public void GetUsersNotifications_UserIsMissing_ReturnsUnauthorized()
    {
        HttpContext httpContext = new DefaultHttpContext();
        _meController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        var result = _meController.GetUsersNotifications() as UnauthorizedObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(401, result.StatusCode);
        Assert.AreEqual("UserId is missing", result.Value);
    }
}
