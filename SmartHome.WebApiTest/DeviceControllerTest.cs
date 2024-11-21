using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.DeviceModels.Out;
using SmartHome.WebApi.WebModels.PaginationModels.Out;

namespace SmartHome.WebApi.Test;

[TestClass]
public class DeviceControllerTest
{
    private Mock<IDeviceLogic>? deviceLogicMock;
    private DevicesController? deviceController;
    private readonly Role homeOwner = new Role() { Name = "HomeOwner" };

    [TestInitialize]
    public void TestInitialize()
    {
        deviceLogicMock = new Mock<IDeviceLogic>(MockBehavior.Strict);
        deviceController = new DevicesController(deviceLogicMock.Object);
    }

    [TestMethod]
    public void GetAllDevicesTest_Ok()
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
        var company2 = new Business
        {
            Id = Guid.NewGuid(),
            Name = "kolke",
            Logo = "logo2",
            RUT = "rut2",
            BusinessOwner = user1
        };

        var device1 = new Device
        {
            Id = Guid.NewGuid(),
            Business = company1,
            Description = "description1",
            ModelNumber = "1234",
            Name = "Sensor1",
            Photos = []
        };
        var device2 = new Device
        {
            Id = Guid.NewGuid(),
            Business = company2,
            Description = "description2",
            ModelNumber = "5678",
            Name = "Sensor2",
            Photos = []
        };

        IEnumerable<Device> devices = new List<Device>()
        {
            device1,
            device2
        };

        deviceLogicMock.Setup(d => d.GetDevices(null, null, null, null, null, null))
            .Returns(devices);

        var result = deviceController.GetAllDevices(null, null, null, null, null, null) as OkObjectResult;

        var objectResult = (result.Value as List<DeviceResponseModel>)!;
        var expectedResponse = new List<DeviceResponseModel>
        {
            new DeviceResponseModel(devices.First()),
            new DeviceResponseModel(devices.Last())
        };

        deviceLogicMock.VerifyAll();
        Assert.IsNotNull(result);
        Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        Assert.AreEqual(expectedResponse.Count, objectResult.Count);
    }

    [TestMethod]
    public void GetAllDevicesTest_WithPagination_Ok()
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
        var company2 = new Business
        {
            Id = Guid.NewGuid(),
            Name = "kolke",
            Logo = "logo2",
            RUT = "rut2",
            BusinessOwner = user1
        };
        var company3 = new Business
        {
            Id = Guid.NewGuid(),
            Name = "example",
            Logo = "logo3",
            RUT = "rut3",
            BusinessOwner = user1
        };

        var device1 = new Device
        {
            Id = Guid.NewGuid(),
            Business = company1,
            Description = "description1",
            ModelNumber = "1234",
            Name = "Sensor1",
            Photos = []
        };
        var device2 = new Device
        {
            Id = Guid.NewGuid(),
            Business = company2,
            Description = "description2",
            ModelNumber = "5678",
            Name = "Sensor2",
            Photos = []
        };
        var device3 = new Device
        {
            Id = Guid.NewGuid(),
            Business = company3,
            Description = "description3",
            ModelNumber = "91011",
            Name = "Sensor3",
            Photos = []
        };

        IEnumerable<Device> devices = new List<Device>
        {
            device1,
            device2,
            device3
        };
        var expectedReturnedDevices = new List<Device>
        {
            device1,
            device2
        };

        deviceLogicMock.Setup(d => d.GetDevices(1, 2, null, null, null, null))
            .Returns(expectedReturnedDevices);

        var result = deviceController.GetAllDevices(1, 2, null, null, null, null) as OkObjectResult;

        var paginatedResponse = result.Value as PaginatedResponse<DeviceResponseModel>;
        var returnedDevices = paginatedResponse.Data;

        Assert.IsNotNull(result);
        Assert.AreEqual(2, returnedDevices.Count);
        Assert.AreEqual(2, paginatedResponse.TotalCount);
        Assert.AreEqual(1, paginatedResponse.PageNumber);
        Assert.AreEqual(2, paginatedResponse.PageSize);
    }

    [TestMethod]
    public void GetAllDevicesTest_FilterByDeviceName_Ok()
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

        var device1 = new Device
        {
            Id = Guid.NewGuid(),
            Business = company1,
            Description = "description1",
            ModelNumber = "1234",
            Name = "Sensor1",
            Photos = []
        };
        var device2 = new Device
        {
            Id = Guid.NewGuid(),
            Business = company1,
            Description = "description2",
            ModelNumber = "5678",
            Name = "Sensor2",
            Photos = []
        };

        IEnumerable<Device> devices = new List<Device>()
        {
            device1,
            device2
        };

        var expectedReturnedDevices = new List<Device>
        {
            device1
        };

        deviceLogicMock.Setup(d => d.GetDevices(null, null, "Sensor1", null, null, null))
            .Returns(expectedReturnedDevices);

        var result = deviceController.GetAllDevices(null, null, "Sensor1", null, null, null) as OkObjectResult;

        var objectResult = (result.Value as List<DeviceResponseModel>)!;
        Assert.IsNotNull(result);
        Assert.AreEqual(1, objectResult.Count);
        Assert.AreEqual("Sensor1", objectResult.First().Name);
    }

    [TestMethod]
    public void GetAllDevicesTest_FilterByDeviceModel_Ok()
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

        var device1 = new Device
        {
            Id = Guid.NewGuid(),
            Business = company1,
            Description = "description1",
            ModelNumber = "1234",
            Name = "Sensor1",
            Photos = []
        };
        var device2 = new Device
        {
            Id = Guid.NewGuid(),
            Business = company1,
            Description = "description2",
            ModelNumber = "5678",
            Name = "Sensor2",
            Photos = []
        };

        IEnumerable<Device> devices = new List<Device>
        {
            device1,
            device2
        };

        var expectedReturnedDevices = new List<Device>
        {
            device1
        };

        deviceLogicMock.Setup(d => d.GetDevices(null, null, null, "1234", null, null))
            .Returns(expectedReturnedDevices);

        var result = deviceController.GetAllDevices(null, null, null, "1234", null, null) as OkObjectResult;

        var objectResult = (result.Value as List<DeviceResponseModel>)!;
        Assert.IsNotNull(result);
        Assert.AreEqual(1, objectResult.Count);
        Assert.AreEqual("1234", objectResult.First().ModelNumber);
    }

    [TestMethod]
    public void GetAllDevicesTest_FilterByBusinessName_Ok()
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
        var company2 = new Business
        {
            Id = Guid.NewGuid(),
            Name = "kolke",
            Logo = "logo2",
            RUT = "rut2",
            BusinessOwner = user1
        };

        var device1 = new Device
        {
            Id = Guid.NewGuid(),
            Business = company1,
            Description = "description1",
            ModelNumber = "1234",
            Name = "Sensor1",
            Photos = []
        };
        var device2 = new Device
        {
            Id = Guid.NewGuid(),
            Business = company2,
            Description = "description2",
            ModelNumber = "5678",
            Name = "Sensor2",
            Photos = []
        };

        IEnumerable<Device> devices = new List<Device>
        {
            device1,
            device2
        };
        var expectedReturnedDevices = new List<Device>
        {
            device1
        };

        deviceLogicMock.Setup(d => d.GetDevices(null, null, null, null, "hikvision", null))
            .Returns(expectedReturnedDevices);

        var result = deviceController.GetAllDevices(null, null, null, null, "hikvision", null) as OkObjectResult;

        var objectResult = (result.Value as List<DeviceResponseModel>)!;
        Assert.IsNotNull(result);
        Assert.AreEqual(1, objectResult.Count);
    }

    [TestMethod]
    public void GetAllDevicesTest_WithMultipleFilters_ReturnsFilteredDevices()
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

        var device1 = new Device
        {
            Id = Guid.NewGuid(),
            Business = company1,
            Description = "description1",
            ModelNumber = "1234",
            Name = "Sensor1",
            Photos = [],
            Type = "TypeA"
        };
        var device2 = new Device
        {
            Id = Guid.NewGuid(),
            Business = company1,
            Description = "description2",
            ModelNumber = "5678",
            Name = "Sensor2",
            Photos = [],
            Type = "TypeB"
        };

        IEnumerable<Device> devices = new List<Device>
        {
            device1,
            device2
        };

        var expectedReturnedDevices = new List<Device>
        {
            device1
        };

        deviceLogicMock.Setup(d => d.GetDevices(null, null, "Sensor1", "1234", null, "TypeA"))
            .Returns(expectedReturnedDevices);

        var result = deviceController.GetAllDevices(null, null, "Sensor1", "1234", null, "TypeA") as OkObjectResult;

        var objectResult = (result.Value as List<DeviceResponseModel>)!;
        Assert.IsNotNull(result);
        Assert.AreEqual(1, objectResult.Count);
        Assert.AreEqual("Sensor1", objectResult.First().Name);
        Assert.AreEqual("1234", objectResult.First().ModelNumber);
    }

    [TestMethod]
    public void GetAllDevicesTest_FilterByDeviceType_Ok()
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

        var device1 = new Device
        {
            Type = "TypeA",
            Id = Guid.NewGuid(),
            Business = company1,
            Description = "description1",
            ModelNumber = "1234",
            Name = "Sensor1",
            Photos = []
        };
        var device2 = new Device
        {
            Type = "TypeB",
            Id = Guid.NewGuid(),
            Business = company1,
            Description = "description2",
            ModelNumber = "5678",
            Name = "Sensor2",
            Photos = []
        };

        IEnumerable<Device> devices = new List<Device>
        {
           device1,
           device2
        };

        var expectedReturnedDevices = new List<Device>
        {
            device1
        };

        deviceLogicMock.Setup(d => d.GetDevices(null, null, null, null, null, "TypeA"))
            .Returns(expectedReturnedDevices);

        var result = deviceController.GetAllDevices(null, null, null, null, null, "TypeA") as OkObjectResult;

        var objectResult = (result.Value as List<DeviceResponseModel>)!;
        Assert.IsNotNull(result);
        Assert.AreEqual(1, objectResult.Count);
    }

    [TestMethod]
    public void DevicesController_NullDeviceLogic_ThrowsArgumentNullException()
    {
        try
        {
            var controller = new DevicesController(null);
        }
        catch (ArgumentNullException ex)
        {
            Assert.AreEqual("deviceLogic", ex.ParamName);
        }
    }
}
