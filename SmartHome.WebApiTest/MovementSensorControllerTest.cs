using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Constants;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.MovementSensorModels.In;
using SmartHome.WebApi.WebModels.MovementSensorModels.Out;

namespace SmartHome.WebApi.Test;
[TestClass]
public class MovementSensorControllerTest
{
    private Mock<ICreateDeviceLogic>? movementSensorLogicMock;
    private MovementSensorsController? movementSensorController;
    private readonly Role homeOwner = new Role
    {
        Name = "HomeOwner"
    };

    [TestInitialize]
    public void TestInitialize()
    {
        movementSensorLogicMock = new Mock<ICreateDeviceLogic>(MockBehavior.Strict);
        movementSensorController = new MovementSensorsController(movementSensorLogicMock.Object);
    }

    [TestMethod]
    public void CreateMovementSensorTest_Ok()
    {
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
        var company1 = new Business
        {
            Id = Guid.NewGuid(),
            Name = "hikvision",
            Logo = "logo1",
            RUT = "rut1",
            BusinessOwner = user1
        };

        var deviceRequestModel = new MovementSensorRequestModel()
        {
            Name = "Sensor de movimiento",
            Description = "Sensor de movimiento",
            ModelNumber = "1234",
            Photos = new List<string>(),
            Type = "MovementSensor"
        };

        Device device = deviceRequestModel.ToEntity();
        device.Business = company1;
        device.Id = Guid.NewGuid();

        var httpContext = new DefaultHttpContext();
        httpContext.Items.Add(UserStatic.User, user1);

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        movementSensorController = new MovementSensorsController(movementSensorLogicMock.Object)
        {
            ControllerContext = controllerContext
        };

        movementSensorLogicMock.Setup(d => d.CreateDevice(It.IsAny<Device>(), It.IsAny<User>(), It.IsAny<string>()))
            .Returns(device);

        var expectedResult = new MovementSensorResponseModel(device);
        var expectedObjectResult = new CreatedAtActionResult("CreateMovementSensor", "Device", new { Id = device.Id }, expectedResult);

        var result = movementSensorController.CreateMovementSensor(deviceRequestModel) as CreatedAtActionResult;
        var deviceResult = result.Value as MovementSensorResponseModel;

        movementSensorLogicMock.VerifyAll();
        Assert.AreEqual(expectedObjectResult.StatusCode, result.StatusCode);
        Assert.AreEqual(expectedResult, deviceResult);
    }

    [TestMethod]
    public void MovementSensorController_NullMovementSensorLogic_ThrowsArgumentNullException()
    {
        try
        {
            var controller = new MovementSensorsController(null);
        }
        catch (ArgumentNullException ex)
        {
            Assert.AreEqual("createDeviceLogic", ex.ParamName);
        }
    }

    [TestMethod]
    public void CreateMovementSensor_UserIdMissing_ReturnsUnauthorized()
    {
        var deviceRequestModel = new MovementSensorRequestModel
        {
            Name = "Movement Sensor Test",
            Description = "A test movement sensor",
            ModelNumber = "MS-1234",
            Photos = new List<String>(),
            Type = "MovementSensor"
        };

        var httpContext = new DefaultHttpContext();
        var controllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        movementSensorController.ControllerContext = controllerContext;

        var result = movementSensorController.CreateMovementSensor(deviceRequestModel) as UnauthorizedObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(401, result.StatusCode);
        Assert.AreEqual("UserId is missing", result.Value);
    }
}
