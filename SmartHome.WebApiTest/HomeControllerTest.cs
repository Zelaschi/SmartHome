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
    private HomeController? homeController;
    private readonly Role homeOwner = new Role() { Name = "HomeOwner" };

    [TestInitialize]
    public void TestInitialize()
    {
        homeLogicMock = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeController = new HomeController(homeLogicMock.Object);
    }

    [TestMethod]
    public void CreateHomeTest_Ok()
    {
        var user1Id = Guid.NewGuid();
        var user1 = new User() { Id = user1Id, Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var homeRequestModel = new CreateHomeRequestModel()
        {
            Owner = user1,
            MainStreet = "Cuareim",
            DoorNumber = "1234",
            Latitude = "12",
            Longitude = "34",
            MaxMembers = 5
        };

        Home home = homeRequestModel.ToEntity();
        home.Id = Guid.NewGuid();

        HttpContext httpContext = new DefaultHttpContext();
        httpContext.Items.Add("UserId", user1.Id.ToString());

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        homeController = new HomeController(homeLogicMock.Object) { ControllerContext = controllerContext };

        homeLogicMock.Setup(h => h.CreateHome(It.IsAny<Home>(), It.IsAny<Guid>())).Returns(home);
        var expectedResult = new HomeResponseModel(home);
        var expectedDeviceResult = new CreatedAtActionResult("CreateHome", "CreateHome", new { Id = home.Id }, expectedResult);

        var result = homeController.CreateHome(homeRequestModel) as CreatedAtActionResult;
        var homeResult = result.Value as HomeResponseModel;

        homeLogicMock.VerifyAll();
        Assert.IsTrue(expectedDeviceResult.StatusCode.Equals(result.StatusCode) && expectedResult.Equals(homeResult));
    }

    [TestMethod]
    public void GetAllHomesByUserIdTest_Ok()
    {
        var user1Id = Guid.NewGuid();
        var user1 = new User() { Id = user1Id, Name = "a", Surname = "b", Password = "psw1", Email = "user1@gmail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var user2 = new User() { Id = Guid.NewGuid(), Name = "c", Surname = "d", Password = "psw2", Email = "user2@hotmail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var home1 = new Home() { Id = Guid.NewGuid(), MainStreet = "Cuareim", DoorNumber = "1234", Latitude = "12", Longitude = "34", MaxMembers = 5, Owner = user1};
        var home2 = new Home() { Id = Guid.NewGuid(), MainStreet = "18 de Julio", DoorNumber = "5678", Latitude = "56", Longitude = "78", MaxMembers = 10, Owner = user2};
        var homes = new List<Home>() { home1, home2 };
        user1.Houses = homes;

        IUsersLogic usersLogicMock = new Mock<IUsersLogic>().Object;
        homeLogicMock.Setup(h => h.GetAllHomesByUserId(It.IsAny<Guid>())).Returns(homes);

        var expected = new OkObjectResult(new List<HomeResponseModel>
        {
            new HomeResponseModel(homes.First()),
            new HomeResponseModel(homes.Last())
        });

        var result = homeController.GetAllHomesByUserId(user1Id) as OkObjectResult;
        var objectResult = (result.Value as List<HomeResponseModel>)!;

        var expectedObject = (expected.Value as List<HomeResponseModel>)!;
        homeLogicMock.VerifyAll();

        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode) && expectedObject.First().Equals(objectResult.First()));
    }

    [TestMethod]
    public void AddDeviceToHomeTest_Ok()
    {
        var user1 = new User() { Id = Guid.NewGuid(), Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var homeRequestModel = new CreateHomeRequestModel()
        {
            Owner = user1,
            MainStreet = "Cuareim",
            DoorNumber = "1234",
            Latitude = "12",
            Longitude = "34",
            MaxMembers = 5
        };
        Home home = homeRequestModel.ToEntity();
        home.Id = Guid.NewGuid();
        var company = new Business() { Id = Guid.NewGuid(), Name = "hikvision", Logo = "logo1", RUT = "rut1", BusinessOwner = user1 };
        var device = new Device() { Id = Guid.NewGuid(), Name = "device1", ModelNumber = "a", Description = "testDevice", Photos = " ", Business = company };

        homeLogicMock.Setup(h => h.AddDeviceToHome(It.IsAny<Guid>(), It.IsAny<Guid>()));

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
        var home = new Home() { Id = Guid.NewGuid(), MainStreet = "Cuareim", DoorNumber = "1234", Latitude = "12", Longitude = "34", MaxMembers = 5, Owner = user1 };

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
            Owner = user,
            MainStreet = "Cuareim",
            DoorNumber = "1234",
            Latitude = "12",
            Longitude = "34",
            MaxMembers = 5
        };
        Home home = homeRequestModel.ToEntity();
        home.Id = Guid.NewGuid();

        HttpContext httpContext = new DefaultHttpContext();
        httpContext.Items.Add("UserId", user.Id.ToString());

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        homeLogicMock.Setup(h => h.AddHomeMemberToHome(It.IsAny<Guid>(), It.IsAny<Guid>()));

        homeController = new HomeController(homeLogicMock.Object) { ControllerContext = controllerContext };

        // ACT
        var expected = new NoContentResult();
        var result = homeController.AddHomeMemberToHome(home.Id) as NoContentResult;

        // ASSERT
        homeLogicMock.VerifyAll();
        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode));
    }

    [TestMethod]
    public void GetAllHomeDevicesTest_Ok()
    {
        // ARRANGE
        var user1Id = Guid.NewGuid();
        var user1 = new User() { Id = user1Id, Name = "a", Surname = "b", Password = "psw1", Email = "user1@gmail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var business = new Business() { Id = Guid.NewGuid(), Name = "hikvision", Logo = "logo1", RUT = "rut1", BusinessOwner = user1 };
        var device1 = new Device() { Id = Guid.NewGuid(), Name = "A", Type = "A", ModelNumber = "1", Description = "A", Photos = "jpg", Business = business };
        var device2 = new Device() { Id = Guid.NewGuid(), Name = "device1", ModelNumber = "a", Description = "testDevice", Photos = " ", Business = business };
        var homeDevice1 = new HomeDevice() { HardwardId = Guid.NewGuid(), Online = true, Device = device1 };
        var homeDevice2 = new HomeDevice(){ HardwardId = Guid.NewGuid(), Online = true, Device = device2 };
        var homeDevices = new List<HomeDevice>() { homeDevice1, homeDevice2 };
        var home1 = new Home() { Id = Guid.NewGuid(), MainStreet = "Cuareim", DoorNumber = "1234", Latitude = "12", Longitude = "34", MaxMembers = 5, Owner = user1 };
        home1.Devices = homeDevices;
        homeLogicMock.Setup(h => h.GetAllHomeDevices(home1.Id)).Returns(homeDevices);

        var expected = new OkObjectResult(new List<HomeDeviceResponseModel>
        {
            new HomeDeviceResponseModel(homeDevices.First()),
            new HomeDeviceResponseModel(homeDevices.Last())
        });

        List<HomeDeviceResponseModel> expectedObject = (expected.Value as List<HomeDeviceResponseModel>)!;

        // ACT
        var result = homeController.GetAllHomeDevices(home1.Id) as OkObjectResult;
        var objectResult = (result.Value as List<HomeDeviceResponseModel>)!;

        // ASSERT
        homeLogicMock.VerifyAll();
        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode) && expectedObject.First().HardwardId.Equals(objectResult.First().HardwardId));
    }
}
