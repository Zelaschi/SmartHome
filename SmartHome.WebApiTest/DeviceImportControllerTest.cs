using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.DeviceTypes;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.DeviceImportModels.In;
using SmartHome.WebApi.WebModels.DeviceImportModels.Out;

namespace SmartHome.WebApiTest;

[TestClass]
public class DeviceImportControllerTest
{
    private Mock<IDeviceImportLogic>? _deviceImportService;
    private DeviceImportController? _deviceImportController;
    private readonly Role businessOwner = new Role() { Name = "businessOwner" };

    [TestInitialize]
    public void TestInitialize()
    {
        _deviceImportService = new Mock<IDeviceImportLogic>();
        _deviceImportController = new DeviceImportController(_deviceImportService.Object);
    }

    [TestMethod]
    public void ImportDevicesTest_OK()
    {
        // Arrange
        var user1 = new User()
        {
            Id = Guid.NewGuid(),
            Name = "a",
            Surname = "b",
            Password = "psw1",
            Email = "mail1@mail.com",
            Role = businessOwner,
            CreationDate = DateTime.Today
        };
        var company1 = new Business()
        {
            Id = Guid.NewGuid(),
            Name = "hikvision",
            Logo = "logo1",
            RUT = "rut1",
            BusinessOwner = user1
        };

        var deviceImportRequestModel = new DeviceImportRequestModel
        {
            DllName = "JSON",
            FileName = "test.json"
        };

        var httpContext = new DefaultHttpContext();
        httpContext.Items.Add("User", user1);

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        _deviceImportController = new DeviceImportController(_deviceImportService.Object)
        {
            ControllerContext = controllerContext
        };

        var device1 = new SecurityCamera()
        {
            Id = Guid.Parse("69508433-1569-47a4-9591-447c3c4bdcbd"),
            Name = "Business G235",
            Description = "Camera 1",
            ModelNumber = "G235",
            Type = DeviceTypesStatic.SecurityCamera,
            Business = company1,
            Photos = new List<Photo>()
        };
        var device2 = new Device
        {
            Id = Guid.Parse("cc077ab4-432b-43b9-85d3-d256dcc887fb"),
            Name = "Kasa A540",
            Description = "Window Sensor 1",
            ModelNumber = "A540",
            Business = company1,
            Photos = new List<Photo>(),
            Type = DeviceTypesStatic.WindowSensor,
        };

        var devices = new List<Device>
        {
            device1,
            device2
        };

        var devicesDTO = devices.Select(device => new DeviceWithoutPhotosResponseModel(device)).ToList();

        _deviceImportService.Setup(d => d.ImportDevices(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<User>())).Returns(devices);

        var expectedResult = new CreatedAtActionResult("ImportDevice", "DeviceImport", new { Id = string.Empty }, devicesDTO);

        // Act

        var result = _deviceImportController.ImportDevice(deviceImportRequestModel) as CreatedAtActionResult;
        var objectResult = result.Value as List<DeviceWithoutPhotosResponseModel>;

        // Assert
        _deviceImportService.VerifyAll();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        CollectionAssert.AreEqual(objectResult, devicesDTO);
    }
}
