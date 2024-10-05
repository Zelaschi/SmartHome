using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.DeviceModels.In;
using SmartHome.WebApi.WebModels.DeviceModels.Out;

namespace SmartHome.WebApiTest;

[TestClass]
public class DeviceControllerTest
{
    private Mock<IDeviceLogic>? deviceLogicMock;
    private DeviceController? deviceController;
    private readonly Role homeOwner = new Role() { Name = "HomeOwner" };

    [TestInitialize]
    public void Testinitialize()
    {
        deviceLogicMock = new Mock<IDeviceLogic>(MockBehavior.Strict);
        deviceController = new DeviceController(deviceLogicMock.Object);
    }

    [TestMethod]

    public void GetAllDevicesTest_Ok()
    {
        var user1 = new User() { Id = Guid.NewGuid(), Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var company1 = new Business() { Id = Guid.NewGuid(), Name = "hikvision", Logo = "logo1", RUT = "rut1", BusinessOwner = user1 };
        var user2 = new User() { Id = Guid.NewGuid(), Name = "c", Surname = "d", Password = "psw2", Email = "mail2@mail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var company2 = new Business() { Id = Guid.NewGuid(), Name = "kolke", Logo = "logo2", RUT = "rut2", BusinessOwner = user2 };

        IEnumerable<Device> devices = new List<Device>()
        {
            new Device(){ Id = Guid.NewGuid(), Business = company1, Description = "description1", ModelNumber = "1234", Name = "Sensor1", Photos = "fotos1" },
            new Device(){ Id = Guid.NewGuid(), Business = company2, Description = "description1", ModelNumber = "1234", Name = "Sensor1", Photos = "fotos1" }
        };

        deviceLogicMock.Setup(d => d.GetAllDevices()).Returns(devices);

        var expected = new OkObjectResult(new List<DeviceResponseModel>
        {
            new DeviceResponseModel(devices.First()),
            new DeviceResponseModel(devices.Last())
        });

        List<DeviceResponseModel> expectedObject = (expected.Value as List<DeviceResponseModel>)!;

        var result = deviceController.GetAllDevices() as OkObjectResult;
        var objectResult = (result.Value as List<DeviceResponseModel>)!;

        deviceLogicMock.VerifyAll();
        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode) && expectedObject.First().Name.Equals(objectResult.First().Name));
    }

    [TestMethod]

    public void CreateDeviceTest_Ok()
    {
        var user1 = new User() { Id = Guid.NewGuid(), Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var company1 = new Business() { Id = Guid.NewGuid(), Name = "hikvision", Logo = "logo1", RUT = "rut1", BusinessOwner = user1 };

        var deviceRequestModel = new CreateDeviceRequestModel()
        {
            Name = "Sensor de ventana 1",
            Description = "Sensor para ventanas",
            ModelNumber = "1234",
            Photos = "foto del sensor",
            Business = company1
        };

        Device device = deviceRequestModel.ToEntity();
        device.Id = Guid.NewGuid();

        deviceLogicMock.Setup(d => d.CreateDevice(It.IsAny<Device>())).Returns(device);
        var expectedResult = new DeviceResponseModel(device);
        var expectedDeviceResult = new CreatedAtActionResult("CreateDevice", "CreateDevice", new { Id = device.Id }, expectedResult);

        var result = deviceController.CreateDevice(deviceRequestModel) as CreatedAtActionResult;
        var deviceResult = result.Value as DeviceResponseModel;

        deviceLogicMock.VerifyAll();
        Assert.IsTrue(expectedDeviceResult.StatusCode.Equals(result.StatusCode) && expectedResult.Equals(deviceResult));
    }

    [TestMethod]

    public void GetAll_DeviceTypesTest_Ok()
    {
        IEnumerable<string> deviceTypes = new List<string>() { "Window Sensor", "Security Camera" };

        deviceLogicMock.Setup(d => d.GetAllDeviceTypes()).Returns(deviceTypes);

        var expected = new OkObjectResult(new List<DeviceTypesResponseModel>
        {
            new DeviceTypesResponseModel(deviceTypes.First()),
            new DeviceTypesResponseModel(deviceTypes.Last())
        });

        List<DeviceTypesResponseModel> expectedObject = (expected.Value as List<DeviceTypesResponseModel>)!;

        var result = deviceController.GetAllDeviceTypes() as OkObjectResult;
        var objectResult = (result.Value as List<DeviceTypesResponseModel>)!;

        deviceLogicMock.VerifyAll();
        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode) && expectedObject.First().Type.Equals(objectResult.First().Type));
    }
}
