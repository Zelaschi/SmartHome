using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Constants;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.WindowSensorModels.In;
using SmartHome.WebApi.WebModels.WindowSensorModels.Out;

namespace SmartHome.WebApi.Test;
[TestClass]
public class WindowSensorsControllerTest
{
    private Mock<ICreateDeviceLogic>? windowSensorLogicMock;
    private WindowSensorsController? windowSensorController;
    private readonly Role homeOwner = new Role() { Name = "HomeOwner" };

    [TestInitialize]
    public void TestInitialize()
    {
        windowSensorLogicMock = new Mock<ICreateDeviceLogic>(MockBehavior.Strict);
        windowSensorController = new WindowSensorsController(windowSensorLogicMock.Object);
    }

    [TestMethod]
    public void CreateWindowSensorTest_Ok()
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

        var deviceRequestModel = new WindowSensorRequestModel()
        {
            Name = "Sensor de ventana 1",
            Description = "Sensor para ventanas",
            ModelNumber = "1234",
            Photos = []
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

        windowSensorController = new WindowSensorsController(windowSensorLogicMock.Object)
        {
            ControllerContext = controllerContext
        };

        windowSensorLogicMock.Setup(d => d.CreateDevice(It.IsAny<Device>(), It.IsAny<User>(), It.IsAny<string>()))
            .Returns(device);

        var expectedResult = new WindowSensorResponseModel(device);
        var expectedObjectResult = new CreatedAtActionResult("CreateWindowSensor", "WindowSensor", new { Id = device.Id }, expectedResult);

        var result = windowSensorController.CreateWindowSensor(deviceRequestModel) as CreatedAtActionResult;
        var deviceResult = result.Value as WindowSensorResponseModel;

        windowSensorLogicMock.VerifyAll();
        Assert.AreEqual(expectedObjectResult.StatusCode, result.StatusCode);
        Assert.AreEqual(expectedResult, deviceResult);
    }

    [TestMethod]
    public void CreateWindowSensor_UserIdMissing_ReturnsUnauthorized()
    {
        var deviceRequestModel = new WindowSensorRequestModel()
        {
            Name = "Window Sensor",
            Description = "Window sensor for home",
            ModelNumber = "1234",
            Photos = []
        };

        var httpContext = new DefaultHttpContext();
        var controllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        windowSensorController = new WindowSensorsController(windowSensorLogicMock.Object)
        {
            ControllerContext = controllerContext
        };

        var result = windowSensorController.CreateWindowSensor(deviceRequestModel) as UnauthorizedObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(401, result.StatusCode);
        Assert.AreEqual("UserId is missing", result.Value);
    }

    [TestMethod]
    public void WindowSensorsController_NullCreateDeviceLogic_ThrowsArgumentNullException()
    {
        try
        {
            var controller = new WindowSensorsController(null);
        }
        catch (ArgumentNullException ex)
        {
            Assert.AreEqual("createDeviceLogic", ex.ParamName);
        }
    }
}
