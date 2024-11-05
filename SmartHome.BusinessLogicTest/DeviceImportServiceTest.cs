using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SmartHome.BusinessLogic.DeviceTypes;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Services;

namespace SmartHome.BusinessLogic.Test;

[TestClass]
public class DeviceImportServiceTest
{
    private Mock<IGenericRepository<Device>>? deviceRepositoryMock;
    private Mock<IGenericRepository<Business>>? businessRepositoryMock;
    private DeviceService? deviceService;
    private DeviceImportService? deviceImportService;
    private readonly Role businessOwner = new Role() { Name = "businessOwner" };
    private readonly string path = @".\test.json";

    [TestInitialize]
    public void TestInitialize()
    {
        deviceRepositoryMock = new Mock<IGenericRepository<Device>>();
        businessRepositoryMock = new Mock<IGenericRepository<Business>>();
        deviceService = new DeviceService(businessRepositoryMock.Object, deviceRepositoryMock.Object);
        deviceImportService = new DeviceImportService(deviceService);
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
        var expected = new List<Device>()
        {
            device1,
            device2
        };

        businessRepositoryMock.Setup(u => u.Find(It.IsAny<Func<Business, bool>>())).Returns(company1);
        deviceRepositoryMock.SetupSequence(d => d.Add(It.IsAny<Device>()))
            .Returns(device1)
            .Returns(device2);

        var result = deviceImportService.ImportDevices("JSON", path, user1);
        CollectionAssert.AreEqual(expected, result);
    }
}
