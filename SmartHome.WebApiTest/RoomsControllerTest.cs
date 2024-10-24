using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.RoomModels.In;
using SmartHome.WebApi.WebModels.RoomModels.Out;

namespace SmartHome.WebApiTest;
[TestClass]
public class RoomsControllerTest
{
    private Mock<IRoomLogic>? roomLogicMock;
    private RoomsController? roomController;

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
}
