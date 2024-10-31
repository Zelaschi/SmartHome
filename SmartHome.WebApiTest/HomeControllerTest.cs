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
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.DeviceModels.Out;
using SmartHome.WebApi.WebModels.HomeDeviceModels.Out;
using SmartHome.WebApi.WebModels.HomeMemberModels.Out;
using SmartHome.WebApi.WebModels.HomeModels.In;
using SmartHome.WebApi.WebModels.HomeModels.Out;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using Device = SmartHome.BusinessLogic.Domain.Device;
using User = SmartHome.BusinessLogic.Domain.User;

namespace SmartHome.WebApiTest;

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
        httpContext.Items.Add("User", user1);

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

        var expected = new NoContentResult();
        var result = homeController.AddDeviceToHome(home.Id, device.Id) as NoContentResult;

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
        httpContext.Items.Add("User", user);

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        homeLogicMock.Setup(h => h.AddHomeMemberToHome(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(It.IsAny<HomeMember>());

        homeController = new HomesController(homeLogicMock.Object) { ControllerContext = controllerContext };

        // ACT
        var expected = new NoContentResult();
        var result = homeController.AddHomeMemberToHome(home.Id, userId) as NoContentResult;

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
        var homeDevice2 = new HomeDevice(){ Id = Guid.NewGuid(), Online = true, Device = device2, Name = device2.Name };
        var homeDevices = new List<HomeDevice>() { homeDevice1, homeDevice2 };
        var home1 = new Home() { Id = Guid.NewGuid(), MainStreet = "Cuareim", DoorNumber = "1234", Latitude = "12", Longitude = "34", MaxMembers = 5, Owner = user1, Name = "Home Name" };
        home1.Devices = homeDevices;
        homeLogicMock.Setup(h => h.GetAllHomeDevices(home1.Id)).Returns(homeDevices);

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

        var result = homeController.UpdateHomeDeviceName(device.Id, "newName") as OkResult;

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

        var newName = "New home name";

        homeLogicMock.Setup(h => h.UpdateHomeName(It.IsAny<Guid>(), It.IsAny<string>()));

        var expected = new OkResult();

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
}
