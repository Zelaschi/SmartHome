using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.HomeDeviceModels.Out;
using SmartHome.WebApi.WebModels.HomeModels.In;
using SmartHome.WebApi.WebModels.RoomModels.In;
using SmartHome.WebApi.WebModels.RoomModels.Out;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SmartHome.WebApi.Test;
[TestClass]
public class RoomsControllerTest
{
    private Mock<IRoomLogic>? roomLogicMock;
    private RoomsController? roomController;
    private readonly Role homeOwner = new Role() { Name = "HomeOwner" };

    [TestInitialize]
    public void TestInitialize()
    {
        roomLogicMock = new Mock<IRoomLogic>(MockBehavior.Strict);
        roomController = new RoomsController(roomLogicMock.Object);
    }

    [TestMethod]
    public void CreateRoomTest_OK()
    {
        var roomRequestModel = new RoomRequestModel()
        {
            Name = "roomName"
        };

        var room = roomRequestModel.ToEntity();
        roomLogicMock.Setup(r => r.CreateRoom(It.IsAny<Room>(), It.IsAny<Guid>())).Returns(room);

        var expectedResult = new RoomResponseModel(room);
        var expectedObjectResult = new CreatedAtActionResult("CreateRoom", "CreateRoom", new { Id = room.Id }, expectedResult);

        var result = roomController.CreateRoom(roomRequestModel, Guid.NewGuid()) as CreatedAtActionResult;
        var roomResult = result.Value as RoomResponseModel;

        roomLogicMock.VerifyAll();
        Assert.IsTrue(expectedObjectResult.StatusCode.Equals(result.StatusCode) && expectedResult.Equals(roomResult));
    }

    [TestMethod]
    public void AddDevicesToRoomTest_OK()
    {
        var user1 = new User()
        {
            Id = Guid.NewGuid(),
            Name = "a",
            Surname = "b",
            Password = "psw1",
            Email = "mail1@mail.com",
            Role = homeOwner,
            CreationDate = DateTime.Today
        };

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

        var company = new Business()
        {
            Id = Guid.NewGuid(),
            Name = "hikvision",
            Logo = "logo1",
            RUT = "rut1",
            BusinessOwner = user1
        };

        var homeDeviceId = Guid.NewGuid();
        var roomId = Guid.NewGuid();

        var device = new Device()
        {
            Id = Guid.NewGuid(),
            Name = "device1",
            ModelNumber = "a",
            Description = "testDevice",
            Photos = [],
            Business = company
        };

        var homeDevice = new HomeDevice()
        {
            Id = homeDeviceId,
            Online = true,
            Device = device,
            HomeId = home.Id,
            Name = device.Name
        };

        roomLogicMock.Setup(r => r.AddDevicesToRoom(homeDeviceId, roomId))
        .Returns(homeDevice)
        .Verifiable();

        var expectedResult = new HomeDeviceResponseModel(homeDevice);

        var hdIdReq = new HomeDeviceIdRequestModel { HomeDeviceId = homeDeviceId };

        var result = roomController.AddDevicesToRoom(hdIdReq, roomId);

        result.Should().BeOfType<CreatedAtActionResult>();

        var createdResult = (CreatedAtActionResult)result;
        createdResult.Should().NotBeNull();
        createdResult.ActionName.Should().Be("AddDevicesToRoom");
        createdResult.StatusCode.Should().Be(201);

        var returnedModel = (HomeDeviceResponseModel)createdResult.Value;
        returnedModel.Should().NotBeNull();
        returnedModel.HardwardId.Should().Be(expectedResult.HardwardId);

        roomLogicMock.Verify(r => r.AddDevicesToRoom(homeDeviceId, roomId), Times.Once());
    }

    [TestMethod]
    public void GetAllRoomsFromHomeTest_Ok()
    {
        var owner = new User()
        {
            Id = Guid.NewGuid(),
            Name = "a",
            Surname = "b",
            Password = "psw1",
            Email = "test@gmail.com",
            Role = homeOwner,
            CreationDate = DateTime.Today
        };

        var homeId = Guid.NewGuid();

        var home = new Home()
        {
            Id = homeId,
            MainStreet = "Elm Street",
            DoorNumber = "4567",
            Latitude = "10",
            Longitude = "20",
            Owner = owner,
            MaxMembers = 4,
            Name = "Home Name"
        };

        var room1 = new Room() { Id = Guid.NewGuid(), Name = "Living Room", Home = home };
        var room2 = new Room() { Id = Guid.NewGuid(), Name = "Kitchen", Home = home};
        var roomsList = new List<Room> { room1, room2 };

        roomLogicMock.Setup(r => r.GetAllRoomsFromHome(homeId)).Returns(roomsList);

        var result = roomController.GetAllRoomsFromHome(homeId) as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);

        var response = result.Value as List<RoomResponseModel>;
        Assert.IsNotNull(response);
        Assert.AreEqual(2, response.Count);
        Assert.AreEqual(room1.Name, response[0].Name);
        Assert.AreEqual(room2.Name, response[1].Name);

        roomLogicMock.Verify(r => r.GetAllRoomsFromHome(homeId), Times.Once);
    }
}
