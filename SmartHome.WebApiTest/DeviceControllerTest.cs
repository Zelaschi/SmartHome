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
        var user1 = new Domain.User() { Id = Guid.NewGuid(), Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var company1 = new Company() { Id = Guid.NewGuid(), Name = "hikvision", Logo = "logo1", RUT = "rut1", CompanyOwner = user1 };
        var user2 = new Domain.User() { Id = Guid.NewGuid(), Name = "c", Surname = "d", Password = "psw2", Email = "mail2@mail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var company2 = new Company() { Id = Guid.NewGuid(), Name = "kolke", Logo = "logo2", RUT = "rut2", CompanyOwner = user2 };

        IEnumerable<Domain.Device> devices = new List<Domain.Device>()
        {
            new Domain.Device(){ Id = Guid.NewGuid(), Company = company1, Description = "description1", ModelNumber = "1234", Name = "Sensor1", Photos = "fotos1" },
            new Domain.Device(){ Id = Guid.NewGuid(), Company = company2, Description = "description1", ModelNumber = "1234", Name = "Sensor1", Photos = "fotos1" }
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
}
