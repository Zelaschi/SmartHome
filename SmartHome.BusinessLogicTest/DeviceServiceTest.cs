﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.ExtraRepositoryInterfaces;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Services;

namespace SmartHome.BusinessLogicTest;

[TestClass]
public class DeviceServiceTest
{
    private Mock<IGenericRepository<Device>>? deviceRepositoryMock;
    private Mock<IGenericRepository<Business>>? businessRepositoryMock;
    private Mock<IDeviceTypeRepository>? deviceTypeRepositoryMock;
    private DeviceService? deviceService;

    [TestInitialize]

    public void Initialize()
    {
        deviceRepositoryMock = new Mock<IGenericRepository<Device>>();
        deviceTypeRepositoryMock = new Mock<IDeviceTypeRepository>();
        businessRepositoryMock = new Mock<IGenericRepository<Business>>();
        deviceService = new DeviceService(businessRepositoryMock.Object, deviceRepositoryMock.Object, deviceTypeRepositoryMock.Object);
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

        var result = deviceService.GetAllDevices().ToList();

        deviceRepositoryMock.VerifyAll();
        CollectionAssert.AreEqual(devices, result);
    }

    [TestMethod]

    public void Create_SecurityCamera_Test()
    {
        var bonwer = new User
        {
            Name = "Pedro",
            Surname = "Rodriguez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "pedrorod@gmail.com"
        };
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
                BusinessOwner = bonwer
            }
        };
        var business = new Business
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
            Business = business
        };

        businessRepositoryMock.Setup(u => u.Find(It.IsAny<Func<Business, bool>>())).Returns(business);
        deviceRepositoryMock.Setup(x => x.Add(securityCamera)).Returns(expected);

        var result = deviceService.CreateSecurityCamera(securityCamera, bonwer);

        deviceRepositoryMock.Verify(x => x.Add(securityCamera), Times.Once);

        Assert.AreEqual(securityCamera, result);
    }

    [TestMethod]

    public void Create_WindowSensor_Test()
    {
        var bowner = new User
        {
            Name = "Juan",
            Surname = "Perez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "juanperez@gmail.com"
        };
        var business = new Business
        {
            Id = Guid.NewGuid(),
            Name = "HikVision",
            Logo = "Logo1",
            RUT = "1234",
            BusinessOwner = bowner
        };
        var windowSensor = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Window Sensor",
            Description = "Window Sensor",
            ModelNumber = "1234",
            Photos = "Photo1",
            Business = business
        };

        var expected = new Device
        {
            Id = windowSensor.Id,
            Name = "Window Sensor",
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
        };
        businessRepositoryMock.Setup(u => u.Find(It.IsAny<Func<Business, bool>>())).Returns(business);

        deviceRepositoryMock.Setup(x => x.Add(windowSensor)).Returns(expected);

        var result = deviceService.CreateDevice(windowSensor, bowner);

        deviceRepositoryMock.Verify(x => x.Add(windowSensor), Times.Once);

        Assert.AreEqual(windowSensor, result);
    }

    [TestMethod]

    public void Create_WindowSensor_ModelNumeber_Repeated_Throws_Exception_Test()
    {
        var businessOwner = new User
        {
            Name = "Juan",
            Surname = "Perez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "juanperez@gmail.com"
        };
        var id = Guid.NewGuid();
        var business = new Business
        {
            Id = id,
            Name = "HikVision",
            Logo = "Logo1",
            RUT = "1234",
            BusinessOwner = businessOwner
        };
        var windowSensor = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Window Sensor",
            Description = "Window Sensor",
            ModelNumber = "1234",
            Photos = "Photo1",
            Business = business
        };

        businessRepositoryMock.Setup(u => u.Find(It.IsAny<Func<Business, bool>>())).Returns(business);
        deviceRepositoryMock.Setup(u => u.Find(It.IsAny<Func<Device, bool>>())).Returns(windowSensor);
        Exception exception = null;

        try
        {
            deviceService.CreateDevice(windowSensor, businessOwner);
        }
        catch (Exception e)
        {
            exception = e;
        }

        deviceRepositoryMock.VerifyAll();
        Assert.IsInstanceOfType(exception, typeof(DeviceException));
        Assert.AreEqual("Device model already exists", exception.Message);
    }

    [TestMethod]

    public void Create_SecurityCamera_ModelNumeber_Repeated_Throws_Exception_Test()
    {
        var businessOwner = new User
        {
            Name = "Juan",
            Surname = "Perez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "juanperez@gmail.com"
        };
        var id = Guid.NewGuid();
        var business = new Business
        {
            Id = id,
            Name = "HikVision",
            Logo = "Logo1",
            RUT = "1234",
            BusinessOwner = businessOwner
        };
        var cameraId = Guid.NewGuid();
        var securityCamera = new SecurityCamera
        {
            Id = cameraId,
            Name = "Security Camera",
            Description = "Security Camera outdoor",
            ModelNumber = "1234",
            Photos = "Photo1",
            Type = "SecurityCamera",
            Outdoor = true,
            Business = business
        };

        deviceRepositoryMock.Setup(u => u.Find(It.IsAny<Func<Device, bool>>())).Returns(securityCamera);
        businessRepositoryMock.Setup(u => u.Find(It.IsAny<Func<Business, bool>>())).Returns(business);
        Exception exception = null;

        try
        {
            deviceService.CreateSecurityCamera(securityCamera, businessOwner);
        }
        catch (Exception e)
        {
            exception = e;
        }

        deviceRepositoryMock.VerifyAll();
        Assert.IsInstanceOfType(exception, typeof(DeviceException));
        Assert.AreEqual("Security Camera model already exists", exception.Message);
    }

    [TestMethod]

    public void ListAll_DeviceTypes_Test()
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
                Type = "Security Camera",
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

        var deviceTypes = new List<string> { "Window Sensor", "Security Camera" };

        deviceTypeRepositoryMock.Setup(x => x.GetAllDeviceTypes()).Returns(deviceTypes);

        var result = deviceService.GetAllDeviceTypes().ToList();

        deviceTypeRepositoryMock.Verify(x => x.GetAllDeviceTypes(), Times.Once);

        CollectionAssert.AreEqual(deviceTypes, result);
    }

    [TestMethod]
    public void Create_WindowSensor_Business_Not_Found_Throws_Exception_Test()
    {
        var businessOwner = new User
        {
            Name = "Juan",
            Surname = "Perez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "juanperez@gmail.com"
        };
        var user = new User
        {
            Name = "Diego",
            Surname = "Forlan",
            Password = "Goleador",
            CreationDate = DateTime.Today,
            Email = "df10@gmail.com"
        };
        var id = Guid.NewGuid();
        var business = new Business
        {
            Id = id,
            Name = "HikVision",
            Logo = "Logo1",
            RUT = "1234",
            BusinessOwner = businessOwner
        };
        var windowSensor = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Window Sensor",
            Description = "Window Sensor",
            ModelNumber = "1234",
            Photos = "Photo1",
            Business = business
        };

        businessRepositoryMock.Setup(u => u.Find(It.IsAny<Func<Business, bool>>())).Returns((Business)null);
        Exception exception = null;

        try
        {
            deviceService.CreateDevice(windowSensor, user);
        }
        catch (Exception e)
        {
            exception = e;
        }

        Assert.IsInstanceOfType(exception, typeof(DeviceException));
        Assert.AreEqual("Business was not found for the user", exception.Message);
    }

    [TestMethod]
    public void Create_SecurityCamera_Business_Not_Found_Throws_Exception_Test()
    {
        var businessOwner = new User
        {
            Name = "Juan",
            Surname = "Perez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "juanperez@gmail.com"
        };
        var user = new User
        {
            Name = "Diego",
            Surname = "Forlan",
            Password = "Goleador",
            CreationDate = DateTime.Today,
            Email = "df10@gmail.com"
        };
        var id = Guid.NewGuid();
        var business = new Business
        {
            Id = id,
            Name = "HikVision",
            Logo = "Logo1",
            RUT = "1234",
            BusinessOwner = businessOwner
        };
        var securityCamera = new SecurityCamera
        {
            Id = Guid.NewGuid(),
            Name = "WindowSensor",
            Description = "Security Camera",
            ModelNumber = "1234",
            Photos = "Photo1",
            Type = "Security Camera",
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
        };

        businessRepositoryMock.Setup(u => u.Find(It.IsAny<Func<Business, bool>>())).Returns((Business)null);
        Exception exception = null;

        try
        {
            deviceService.CreateSecurityCamera(securityCamera, user);
        }
        catch (Exception e)
        {
            exception = e;
        }

        Assert.IsInstanceOfType(exception, typeof(DeviceException));
        Assert.AreEqual("Business was not found for the user", exception.Message);
    }
}
