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

        _deviceImportService.Setup(d => d.ImportDevices(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<User>())).Returns(2);

        var expectedResult = new CreatedAtActionResult("ImportDevice", "DeviceImport", new { Id = string.Empty }, 2);

        // Act

        var result = _deviceImportController.ImportDevice(deviceImportRequestModel) as CreatedAtActionResult;
        var intResult = result.Value as int?;

        // Assert
        _deviceImportService.VerifyAll();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        Assert.AreEqual(intResult, 2);
    }
}
