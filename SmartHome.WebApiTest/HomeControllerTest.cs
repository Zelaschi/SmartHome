using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Constants;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Services;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.AddUserToHome.In;
using SmartHome.WebApi.WebModels.DeviceModels.Out;
using SmartHome.WebApi.WebModels.HomeDeviceModels.Out;
using SmartHome.WebApi.WebModels.HomeMemberModels.Out;
using SmartHome.WebApi.WebModels.HomeModels.In;
using SmartHome.WebApi.WebModels.HomeModels.Out;
using SmartHome.WebApi.WebModels.RoomModels.Out;
using SmartHome.WebApi.WebModels.UpdateNameModels.In;
using SmartHome.WebApi.WebModels.UserModels.Out;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using Device = SmartHome.BusinessLogic.Domain.Device;
using User = SmartHome.BusinessLogic.Domain.User;

namespace SmartHome.WebApi.Test;

[TestClass]

public class HomeControllerTest
{
    private Mock<IHomeLogic>? homeLogicMock;
    private HomesController? homeController;
    private readonly Role homeOwner = new Role() { Name = "HomeOwner" };

    [TestInitialize]
    public void TestInitialize()
    {
        homeLogicMock = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeController = new HomesController(homeLogicMock.Object);
    }

    [TestMethod]
    public void CreateHomeTest_Ok()
    {
        var user1Id = Guid.NewGuid();
        var user1 = new User() { Id = user1Id, Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var homeRequestModel = new CreateHomeRequestModel()
        {
            MainStreet = "Cuareim",
            DoorNumber = "1234",
            Latitude = "12",
            Longitude = "34",
            MaxMembers = 5,
            Name = "Home Name"
        };

        Home home = homeRequestModel.ToEntity();
        home.Id = Guid.NewGuid();

        HttpContext httpContext = new DefaultHttpContext();
        httpContext.Items.Add(UserStatic.User, user1);

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        homeController = new HomesController(homeLogicMock.Object) { ControllerContext = controllerContext };

        homeLogicMock.Setup(h => h.CreateHome(It.IsAny<Home>(), It.IsAny<Guid>())).Returns(home);
        var expectedResult = new HomeResponseModel(home);
        var expectedDeviceResult = new CreatedAtActionResult("CreateHome", "CreateHome", new { Id = home.Id }, expectedResult);

        var result = homeController.CreateHome(homeRequestModel) as CreatedAtActionResult;
        var homeResult = result.Value as HomeResponseModel;

        homeLogicMock.VerifyAll();
        Assert.IsTrue(expectedDeviceResult.StatusCode.Equals(result.StatusCode) && expectedResult.Equals(homeResult));
    }

    [TestMethod]
    public void AddDeviceToHomeTest_Ok()
    {
        var user1 = new User() { Id = Guid.NewGuid(), Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var homeRequestModel = new CreateHomeRequestModel()
        {
            MainStreet = "Cuareim",
            DoorNumber = "1234",
            Latitude = "12",
            Longitude = "34",
            MaxMembers = 5,
            Name = "Home Name"
        };
        Home home = homeRequestModel.ToEntity();
        home.Id = Guid.NewGuid();
        var company = new Business() { Id = Guid.NewGuid(), Name = "hikvision", Logo = "logo1", RUT = "rut1", BusinessOwner = user1 };
        var device = new Device() { Id = Guid.NewGuid(), Name = "device1", ModelNumber = "a", Description = "testDevice", Photos = [], Business = company };
        var homeDevice = new HomeDevice() { Id = Guid.NewGuid(), Online = true, Device = device, HomeId = home.Id, Name = device.Name };

        homeLogicMock.Setup(h => h.AddDeviceToHome(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(homeDevice);

        var deviceId = new AddDeviceToHomeRequestModel
        {
            DeviceId = device.Id
        };

        var expected = new NoContentResult();
        var result = homeController.AddDeviceToHome(home.Id, deviceId) as NoContentResult;

        homeLogicMock.VerifyAll();
        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode));
    }

    [TestMethod]
    public void GetAllHomeMembersTest_Ok()
    {
        // ARRANGE
        var user1Id = Guid.NewGuid();
        var user2Id = Guid.NewGuid();
        var user1 = new User() { Id = user1Id, Name = "a", Surname = "b", Password = "psw1", Email = "user1@gmail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var user2 = new User() { Id = user1Id, Name = "a", Surname = "b", Password = "psw1", Email = "user1@gmail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var homeMember1 = new HomeMember(user1) { HomeMemberId = Guid.NewGuid(), HomePermissions = new List<HomePermission>(), Notifications = new List<Notification>() };
        var homeMember2 = new HomeMember(user2) { HomeMemberId = Guid.NewGuid(), HomePermissions = new List<HomePermission>(), Notifications = new List<Notification>() };
        var homeMembers = new List<HomeMember>() { homeMember1, homeMember2 };
        var home = new Home() { Id = Guid.NewGuid(), MainStreet = "Cuareim", DoorNumber = "1234", Latitude = "12", Longitude = "34", MaxMembers = 5, Owner = user1, Name = "Home Name" };

        homeLogicMock.Setup(h => h.GetAllHomeMembers(home.Id)).Returns(homeMembers);

        var expected = new OkObjectResult(new List<HomeMemberResponseModel>
        {
            new HomeMemberResponseModel(homeMembers.First()),
            new HomeMemberResponseModel(homeMembers.Last())
        });

        List<HomeMemberResponseModel> expectedObject = (expected.Value as List<HomeMemberResponseModel>)!;

        // ACT
        var result = homeController.GetAllHomeMembers(home.Id) as OkObjectResult;
        var objectResult = (result.Value as List<HomeMemberResponseModel>)!;

        // ASSERT
        homeLogicMock.VerifyAll();
        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode) && expectedObject.First().HomeMemberId.Equals(objectResult.First().HomeMemberId));
    }

    [TestMethod]
    public void AddHomeMemberToHomeTest_OK()
    {
        // ARRANGE
        var userId = Guid.NewGuid();
        var addUserToHomeRequestModel = new AddUserToHomeRequestModel() { UserId = userId };
        var user = new User() { Id = userId, Name = "a", Surname = "b", Password = "psw1", Email = "user1@gmail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var homeMember = new HomeMember(user) { HomeMemberId = Guid.NewGuid(), HomePermissions = new List<HomePermission>(), Notifications = new List<Notification>() };
        var homeRequestModel = new CreateHomeRequestModel()
        {
            MainStreet = "Cuareim",
            DoorNumber = "1234",
            Latitude = "12",
            Longitude = "34",
            MaxMembers = 5,
            Name = "Home Name"
        };
        Home home = homeRequestModel.ToEntity();
        home.Id = Guid.NewGuid();

        HttpContext httpContext = new DefaultHttpContext();
        httpContext.Items.Add(UserStatic.User, user);

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        homeLogicMock.Setup(h => h.AddHomeMemberToHome(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(It.IsAny<HomeMember>());

        homeController = new HomesController(homeLogicMock.Object) { ControllerContext = controllerContext };

        // ACT
        var expected = new NoContentResult();
        var result = homeController.AddHomeMemberToHome(home.Id, addUserToHomeRequestModel) as NoContentResult;

        // ASSERT
        homeLogicMock.VerifyAll();
        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode));
    }

    [TestMethod]
    public void GetAllHomeDevicesTest_Ok()
    {
        var user1Id = Guid.NewGuid();
        var user1 = new User() { Id = user1Id, Name = "a", Surname = "b", Password = "psw1", Email = "user1@gmail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var business = new Business() { Id = Guid.NewGuid(), Name = "hikvision", Logo = "logo1", RUT = "rut1", BusinessOwner = user1 };
        var device1 = new Device() { Id = Guid.NewGuid(), Name = "A", Type = "A", ModelNumber = "1", Description = "A", Photos = [], Business = business };
        var device2 = new Device() { Id = Guid.NewGuid(), Name = "device1", ModelNumber = "a", Description = "testDevice", Photos = [], Business = business };
        var homeDevice1 = new HomeDevice() { Id = Guid.NewGuid(), Online = true, Device = device1, Name = device1.Name };
        var homeDevice2 = new HomeDevice() { Id = Guid.NewGuid(), Online = true, Device = device2, Name = device2.Name };
        var homeDevices = new List<HomeDevice>() { homeDevice1, homeDevice2 };
        var home1 = new Home() { Id = Guid.NewGuid(), MainStreet = "Cuareim", DoorNumber = "1234", Latitude = "12", Longitude = "34", MaxMembers = 5, Owner = user1, Name = "Home Name" };
        home1.Devices = homeDevices;
        homeLogicMock.Setup(h => h.GetAllHomeDevices(home1.Id, null)).Returns(homeDevices);

        var expected = new OkObjectResult(new List<HomeDeviceResponseModel>
        {
            new HomeDeviceResponseModel(homeDevices.First()),
            new HomeDeviceResponseModel(homeDevices.Last())
        });

        List<HomeDeviceResponseModel> expectedObject = (expected.Value as List<HomeDeviceResponseModel>)!;

        var result = homeController.GetAllHomeDevices(home1.Id) as OkObjectResult;
        var objectResult = (result.Value as List<HomeDeviceResponseModel>)!;

        homeLogicMock.VerifyAll();
        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode) && expectedObject.First().HardwardId.Equals(objectResult.First().HardwardId));
    }

    [TestMethod]

    public void UpdateHomeDeviceNameTest_Ok()
    {
        var user1 = new User() { Id = Guid.NewGuid(), Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var homeRequestModel = new CreateHomeRequestModel()
        {
            MainStreet = "Cuareim",
            DoorNumber = "1234",
            Latitude = "12",
            Longitude = "34",
            MaxMembers = 5,
            Name = "Home Name"
        };
        Home home = homeRequestModel.ToEntity();
        home.Id = Guid.NewGuid();
        var company = new Business() { Id = Guid.NewGuid(), Name = "hikvision", Logo = "logo1", RUT = "rut1", BusinessOwner = user1 };
        var device = new Device() { Id = Guid.NewGuid(), Name = "device1", ModelNumber = "a", Description = "testDevice", Photos = [], Business = company };
        var homeDevice = new HomeDevice() { Id = Guid.NewGuid(), Online = true, Device = device, HomeId = home.Id, Name = device.Name };

        homeLogicMock.Setup(h => h.UpdateHomeDeviceName(It.IsAny<Guid>(), It.IsAny<string>()));

        var expected = new OkResult();

        var newName = new UpdateNameRequestModel
        {
            NewName = "New Home Name"
        };

        var result = homeController.UpdateHomeDeviceName(device.Id, newName) as OkResult;

        homeLogicMock.VerifyAll();

        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode));
    }

    [TestMethod]

    public void UpdateHomeNameTest_Ok()
    {
        var user1 = new User() { Id = Guid.NewGuid(), Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var homeRequestModel = new CreateHomeRequestModel()
        {
            MainStreet = "Cuareim",
            DoorNumber = "1234",
            Latitude = "12",
            Longitude = "34",
            MaxMembers = 5,
            Name = "Home Name"
        };
        Home home = homeRequestModel.ToEntity();
        home.Id = Guid.NewGuid();

        homeLogicMock.Setup(h => h.UpdateHomeName(It.IsAny<Guid>(), It.IsAny<string>()));

        var expected = new OkResult();

        var newName = new UpdateNameRequestModel
        {
            NewName = "New Home Name"
        };

        var result = homeController.UpdateHomeName(home.Id, newName) as OkResult;

        homeLogicMock.VerifyAll();

        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode));
    }

    [TestMethod]
    public void CreateHome_UserIdMissing_ReturnsUnauthorized()
    {
        var homeRequestModel = new CreateHomeRequestModel
        {
            Name = "Test Home",
            MainStreet = "123 Main St",
            DoorNumber = "456",
            Latitude = "10.000",
            Longitude = "-10.000",
            MaxMembers = 5
        };

        HttpContext httpContext = new DefaultHttpContext();
        var controllerContext = new ControllerContext { HttpContext = httpContext };

        homeController.ControllerContext = controllerContext;

        var result = homeController.CreateHome(homeRequestModel) as UnauthorizedObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(401, result.StatusCode);
        Assert.AreEqual("UserId is missing", result.Value);
    }

    [TestMethod]
    public void GetAllHomeDevices_WithRoomFilter_ReturnsFilteredDevices()
    {
        var homeId = Guid.NewGuid();
        var owner = new User
        {
            Id = Guid.NewGuid(),
            Name = "Owner 1",
            Surname = "Surname 1",
            Email = "mail@mail.com",
            Password = "password",
            Role = new Role { Name = "Admin" }
        };

        var home = new Home
        {
            Id = homeId,
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name",
            Rooms = new List<Room>(),
            Devices = new List<HomeDevice>()
        };

        var room = new Room
        {
            Id = Guid.NewGuid(),
            Name = "Living Room",
            Home = home
        };

        var device1 = new HomeDevice
        {
            Online = true,
            Id = Guid.NewGuid(),
            Name = "Device 1",
            Device = new Device
            {
                Id = Guid.NewGuid(),
                Name = "Device 1",
                ModelNumber = "1234",
                Description = "Device 1 for home",
                Photos = new List<Photo>(),
                Business = new Business
                {
                    Id = Guid.NewGuid(),
                    Name = "Business 1",
                    Logo = "logo1.png",
                    RUT = "12345678-9",
                    BusinessOwner = owner
                }
            }
        };

        var device2 = new HomeDevice
        {
            Room = room,
            Online = true,
            Id = Guid.NewGuid(),
            Name = "Device 2",
            Device = new Device
            {
                Id = Guid.NewGuid(),
                Name = "Device 2",
                ModelNumber = "1234",
                Description = "Device 2 for home",
                Photos = new List<Photo>(),
                Business = new Business
                {
                    Id = Guid.NewGuid(),
                    Name = "Business 2",
                    Logo = "logo2.png",
                    RUT = "12345678-9",
                    BusinessOwner = owner
                }
            }
        };

        home.Devices = new List<HomeDevice> { device1, device2 };

        homeLogicMock.Setup(h => h.GetAllHomeDevices(home.Id, room.Name)).Returns(new List<HomeDevice>() { device1 });

        var result = homeController.GetAllHomeDevices(homeId, room.Name) as OkObjectResult;

        Assert.IsNotNull(result);
        var resultValue = result.Value as List<HomeDeviceResponseModel>;
        Assert.IsNotNull(resultValue);
        Assert.AreEqual(1, resultValue.Count);
    }

    [TestMethod]
    public void UnRelatedHomeOwnersTest_Ok()
    {
        // ARRANGE
        var user1Id = Guid.NewGuid();
        var user2Id = Guid.NewGuid();
        var user1 = new User() { Id = user1Id, Name = "a", Surname = "b", Password = "psw1", Email = "user1@gmail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var user2 = new User() { Id = user1Id, Name = "a", Surname = "b", Password = "psw1", Email = "user1@gmail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var homeMember1 = new HomeMember(user1) { HomeMemberId = Guid.NewGuid(), HomePermissions = new List<HomePermission>(), Notifications = new List<Notification>() };
        var homeMembers = new List<HomeMember>() { homeMember1 };
        var expectedReturnedUsers = new List<User>() { user2 };
        var home = new Home() { Id = Guid.NewGuid(), MainStreet = "Cuareim", DoorNumber = "1234", Latitude = "12", Longitude = "34", MaxMembers = 5, Owner = user1, Name = "Home Name" };

        homeLogicMock.Setup(h => h.UnRelatedHomeOwners(home.Id)).Returns(expectedReturnedUsers);

        var expected = new OkObjectResult(new List<UserResponseModel>
        {
            new UserResponseModel(expectedReturnedUsers.First()),
        });

        List<UserResponseModel> expectedObject = (expected.Value as List<UserResponseModel>)!;

        // ACT
        var result = homeController.UnRelatedHomeOwners(home.Id) as OkObjectResult;
        var objectResult = (result.Value as List<UserResponseModel>)!;

        // ASSERT
        homeLogicMock.VerifyAll();
        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode) && expectedObject.First().Id.Equals(objectResult.First().Id));
    }

    [TestMethod]
    public void GetHomeByIdTest_Ok()
    {
        var homeRequestModel = new CreateHomeRequestModel()
        {
            MainStreet = "Cuareim",
            DoorNumber = "1234",
            Latitude = "12",
            Longitude = "34",
            MaxMembers = 5,
            Name = "Home Name"
        };
        Home home = homeRequestModel.ToEntity();
        home.Id = Guid.NewGuid();

        homeLogicMock.Setup(h => h.GetHomeById(home.Id)).Returns(home);

        var expected = new OkObjectResult(new HomeResponseModel(home));

        var result = homeController.GetHomeById(home.Id) as OkObjectResult;

        homeLogicMock.VerifyAll();
        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode));
    }

    [TestMethod]
    public void GetHomeById_HomeIdMissing_Test_ReturnsNotFound()
    {
        var missingHomeId = Guid.NewGuid();
        homeLogicMock.Setup(h => h.GetHomeById(missingHomeId)).Returns((Home)null);

        var expected = new NotFoundResult();

        var result = homeController.GetHomeById(missingHomeId) as NotFoundResult;

        homeLogicMock.VerifyAll();
        Assert.IsNotNull(result);
        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode));
    }
}
