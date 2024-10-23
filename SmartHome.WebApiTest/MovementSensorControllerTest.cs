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
using SmartHome.WebApi.WebModels.MovementSensorModels.In;
using SmartHome.WebApi.WebModels.MovementSensorModels.Out;
using SmartHome.WebApi.WebModels.WindowSensorModels.In;
using SmartHome.WebApi.WebModels.WindowSensorModels.Out;

namespace SmartHome.WebApiTest;
[TestClass]
public class MovementSensorControllerTest
{
    private Mock<IMovementSensorLogic>? movementSensorLogicMock;
    private MovementSensorsController? movementSensorController;
    private readonly Role homeOwner = new Role() { Name = "HomeOwner" };

    [TestInitialize]
    public void TestInitialize()
    {
        movementSensorLogicMock = new Mock<IMovementSensorLogic>(MockBehavior.Strict);
        movementSensorController = new MovementSensorsController(movementSensorLogicMock.Object);
    }

    [TestMethod]
    public void CreateMovementSensorTest_Ok()
    {
        // Arrange
        var user1 = new User() { Id = Guid.NewGuid(), Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var company1 = new Business() { Id = Guid.NewGuid(), Name = "hikvision", Logo = "logo1", RUT = "rut1", BusinessOwner = user1 };

        var deviceRequestModel = new MovementSensorRequestModel()
        {
            Name = "Sensor de movimiento",
            Description = "Sensor de movimiento",
            ModelNumber = "1234",
            Photos = "foto del sensor"
        };

        Device device = deviceRequestModel.ToEntity();
        device.Business = company1;
        device.Id = Guid.NewGuid();

        var httpContext = new DefaultHttpContext();
        httpContext.Items.Add("User", user1);

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        movementSensorController = new MovementSensorsController(movementSensorLogicMock.Object) { ControllerContext = controllerContext };

        movementSensorLogicMock.Setup(d => d.CreateMovementSensor(It.IsAny<Device>(), It.IsAny<User>())).Returns(device);

        var expectedResult = new MovementSensorResponseModel(device);
        var expectedObjectResult = new CreatedAtActionResult("CreateMovementSensor", "Device", new { Id = device.Id }, expectedResult);

        // Act
        var result = movementSensorController.CreateMovementSensor(deviceRequestModel) as CreatedAtActionResult;
        var deviceResult = result.Value as MovementSensorResponseModel;

        // Assert
        movementSensorLogicMock.VerifyAll();
        Assert.IsTrue(expectedObjectResult.StatusCode.Equals(result.StatusCode) && expectedResult.Equals(deviceResult));
    }

}
