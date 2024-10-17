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
using SmartHome.WebApi.WebModels.DeviceModels.In;
using SmartHome.WebApi.WebModels.DeviceModels.Out;
using SmartHome.WebApi.WebModels.WindowSensorModels.In;
using SmartHome.WebApi.WebModels.WindowSensorModels.Out;

namespace SmartHome.WebApiTest;
[TestClass]
public class WindowSensorsControllerTest
{
    private Mock<IWindowSensorLogic>? windowSensorLogicMock;
    private WindowSensorsController? windowSensorController;
    private readonly Role homeOwner = new Role() { Name = "HomeOwner" };

    [TestInitialize]
    public void TestInitialize()
    {
        windowSensorLogicMock = new Mock<IWindowSensorLogic>(MockBehavior.Strict);
        windowSensorController = new WindowSensorsController(windowSensorLogicMock.Object);
    }

    [TestMethod]
    public void CreateWindowSensorTest_Ok()
    {
        // Arrange
        var user1 = new User() { Id = Guid.NewGuid(), Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var company1 = new Business() { Id = Guid.NewGuid(), Name = "hikvision", Logo = "logo1", RUT = "rut1", BusinessOwner = user1 };

        var deviceRequestModel = new CreateWindowSensorRequestModel()
        {
            Name = "Sensor de ventana 1",
            Description = "Sensor para ventanas",
            ModelNumber = "1234",
            Photos = "foto del sensor"
        };

        WindowSensor device = deviceRequestModel.ToEntity();
        device.Business = company1;
        device.Id = Guid.NewGuid();

        var httpContext = new DefaultHttpContext();
        httpContext.Items.Add("User", user1);

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        windowSensorController = new WindowSensorsController(windowSensorLogicMock.Object) { ControllerContext = controllerContext };

        windowSensorLogicMock.Setup(d => d.CreateWindowSensor(It.IsAny<WindowSensor>(), It.IsAny<User>())).Returns(device);

        var expectedResult = new CreateWindowSensorResponseModel(device);
        var expectedObjectResult = new CreatedAtActionResult("CreateWindowSensor", "WindowSensor", new { Id = device.Id }, expectedResult);

        // Act
        var result = windowSensorController.CreateWindowSensor(deviceRequestModel) as CreatedAtActionResult;
        var deviceResult = result.Value as CreateWindowSensorResponseModel;

        // Assert
        windowSensorLogicMock.VerifyAll();
        Assert.IsTrue(expectedObjectResult.StatusCode.Equals(result.StatusCode) && expectedResult.Equals(deviceResult));
    }
}
