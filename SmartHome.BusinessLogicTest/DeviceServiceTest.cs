using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Services;

namespace SmartHome.BusinessLogicTest;

[TestClass]
public class DeviceServiceTest
{
    private Mock<IGenericRepository<Device>>? deviceRepositoryMock;
    private DeviceService? deviceService;

    [TestInitialize]

    public void Initialize()
    {
        deviceRepositoryMock = new Mock<IGenericRepository<Device>>();
        deviceService = new DeviceService(deviceRepositoryMock.Object);
    }

    [TestMethod]

    public void GetAll_Devices_Test()
    {
        var devices = new List<Device>
        {
            new Device
            {
                Id = Guid.NewGuid(),
                Name = "WindowSensor",
                Description = "Window Sensor",
                ModelNumber = "1234",
                Photos = "Photo1",
                Business = new Business
                {
                    Id = Guid.NewGuid(),
                    Name = "HikVision",
                    Logo = "Logo1",
                    RUT = "1234",
                    BusinessOwner = new User
                    {
                        Name = "Juan",
                        Surname = "Perez",
                        Password = "Password@1234",
                        CreationDate = DateTime.Today,
                        Email = "juanperez@gmail.com"
                    }
                }
            },
            new SecurityCamera
            {
                Id = Guid.NewGuid(),
                Name = "WindowSensor",
                Description = "Security Camera",
                ModelNumber = "1234",
                Photos = "Photo1",
                Type = "SecurityCamera",
                Business = new Business
                {
                    Id = Guid.NewGuid(),
                    Name = "Kolke",
                    Logo = "Logo1",
                    RUT = "1234",
                    BusinessOwner = new User
                    {
                        Name = "Pedro",
                        Surname = "Rodriguez",
                        Password = "Password@1234",
                        CreationDate = DateTime.Today,
                        Email = "pedrorod@gmail.com"
                    }
                },
                Outdoor = true,
                Indoor = true,
                MovementDetection = true,
                PersonDetection = true
            }
        };

        deviceRepositoryMock.Setup(x => x.FindAll()).Returns(devices);

        var result = deviceService.GetAllDevices();

        deviceRepositoryMock.VerifyAll();
        Assert.AreEqual(devices, result);
    }

    [TestMethod]

    public void Create_SecurityCamera_Test()
    {
        var securityCamera = new SecurityCamera
        {
            Id = Guid.NewGuid(),
            Name = "Security Camera",
            Description = "Security Camera outdoor",
            ModelNumber = "1234",
            Photos = "Photo1",
            Type = "SecurityCamera",
            Outdoor = true,
            Business = new Business
            {
                Id = Guid.NewGuid(),
                Name = "Kolke",
                Logo = "Logo1",
                RUT = "1234",
                BusinessOwner = new User
                {
                    Name = "Pedro",
                    Surname = "Rodriguez",
                    Password = "Password@1234",
                    CreationDate = DateTime.Today,
                    Email = "pedrorod@gmail.com"
                }
            }
        };

        var expected = new SecurityCamera
        {
            Id = securityCamera.Id,
            Name = "Security Camera",
            Description = "Security Camera outdoor",
            ModelNumber = "1234",
            Photos = "Photo1",
            Type = "SecurityCamera",
            Outdoor = true,
            Business = new Business
            {
                Id = Guid.NewGuid(),
                Name = "Kolke",
                Logo = "Logo1",
                RUT = "1234",
                BusinessOwner = new User
                {
                    Name = "Pedro",
                    Surname = "Rodriguez",
                    Password = "Password@1234",
                    CreationDate = DateTime.Today,
                    Email = "pedrorod@gmail.com"
                }
            }
        };

        deviceRepositoryMock.Setup(x => x.Add(securityCamera)).Returns(expected);

        var result = deviceService.CreateSecurityCamera(securityCamera);

        deviceRepositoryMock.Verify(x => x.Add(securityCamera), Times.Once);

        Assert.AreEqual(securityCamera, result);
    }
}
