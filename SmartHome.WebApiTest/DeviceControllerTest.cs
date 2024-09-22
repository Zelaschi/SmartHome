using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.Domain;
using SmartHome.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebModels.DeviceModels.Out;
using SmartHome.WebModels.DeviceModels.In;
using Microsoft.AspNetCore.Http;

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
        var company1 = new Company() { Id = Guid.NewGuid(), Name = "hikvision", Logo = "logo1", RUT = "rut1", CompanyOwner = user1 };
        var user2 = new User() { Id = Guid.NewGuid(), Name = "c", Surname = "d", Password = "psw2", Email = "mail2@mail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var company2 = new Company() { Id = Guid.NewGuid(), Name = "kolke", Logo = "logo2", RUT = "rut2", CompanyOwner = user2 };

        IEnumerable<Device> devices = new List<Domain.Device>()
        {
            new Device(){ Id = Guid.NewGuid(), Company = company1, Description = "description1", ModelNumber = "1234", Name = "Sensor1", Photos = "fotos1" },
            new Device(){ Id = Guid.NewGuid(), Company = company2, Description = "description1", ModelNumber = "1234", Name = "Sensor1", Photos = "fotos1" }
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
        var company1 = new Company() { Id = Guid.NewGuid(), Name = "hikvision", Logo = "logo1", RUT = "rut1", CompanyOwner = user1 };

        CreateDeviceRequestModel deviceRequestModel = new CreateDeviceRequestModel()
        {
            Name = "Sensor de ventana 1",
            Description = "Sensor para ventanas",
            ModelNumber = "1234",
            Photos = "foto del sensor",
            Company = company1
        };

        Device device = deviceRequestModel.ToEntity();
        device.Id = Guid.NewGuid(); 

        deviceLogicMock.Setup(d => d.CreateDevice(It.IsAny<Device>())).Returns(device);
        DeviceResponseModel expectedResult = new DeviceResponseModel(device);
        CreatedAtActionResult expectedDeviceResult = new CreatedAtActionResult("CreateDevice", "CreateDevice", new {Id = device.Id}, expectedResult);

        CreatedAtActionResult result = deviceController.CreateDevice(deviceRequestModel) as CreatedAtActionResult;
        DeviceResponseModel deviceResult = result.Value as DeviceResponseModel;

        deviceLogicMock.VerifyAll();
        Assert.IsTrue(expectedObjectResult.StatusCode.Equals(result.StatusCode) && expectedResult.Equals(deviceResult));
    }
}
