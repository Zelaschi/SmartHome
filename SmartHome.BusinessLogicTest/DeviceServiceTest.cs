using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.ExtraRepositoryInterfaces;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Services;

namespace SmartHome.BusinessLogic.Test;

[TestClass]
public class DeviceServiceTest
{
    private Mock<IGenericRepository<Device>>? deviceRepositoryMock;
    private Mock<IGenericRepository<Business>>? businessRepositoryMock;
    private Mock<IGenericRepository<ModelNumberValidator>>? validatorRepositoryMock;
    private ValidatorService? validatorService;
    private DeviceService? deviceService;

    [TestInitialize]

    public void Initialize()
    {
        deviceRepositoryMock = new Mock<IGenericRepository<Device>>();
        businessRepositoryMock = new Mock<IGenericRepository<Business>>();
        validatorRepositoryMock = new Mock<IGenericRepository<ModelNumberValidator>>();
        validatorService = new ValidatorService(validatorRepositoryMock.Object);
        deviceService = new DeviceService(businessRepositoryMock.Object, deviceRepositoryMock.Object, validatorService);
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
            Photos = [],
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
            Photos = [],
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
            Photos = [],
            Business = business
        };

        var expected = new Device
        {
            Id = windowSensor.Id,
            Name = "Window Sensor",
            Description = "Window Sensor",
            ModelNumber = "1234",
            Photos = [],
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

        var result = deviceService.CreateDevice(windowSensor, bowner, "Window Sensor");

        deviceRepositoryMock.Verify(x => x.Add(windowSensor), Times.Once);

        Assert.AreEqual(windowSensor, result);
    }

    [TestMethod]
    public void Create_WindowSensor_ModelNumeber_Repeated_Throws_Exception_Test()
    {
        // Arrange: Configurar los datos de prueba
        var validator = new ModelNumberValidator
        {
            Id = Guid.NewGuid(),
            Name = "ModelNumberValidator"
        };

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
            BusinessOwner = businessOwner,
            ValidatorId = validator.Id
        };

        var windowSensor = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Window Sensor",
            Description = "Window Sensor",
            ModelNumber = "1234",
            Photos = new List<Photo>(),
            Business = business
        };

        // Configurar el mock para businessRepository
        businessRepositoryMock.Setup(u => u.Find(It.IsAny<Func<Business, bool>>()))
            .Returns(business);

        // Configurar el mock para validatorRepository
        validatorRepositoryMock.Setup(u => u.Find(It.IsAny<Func<ModelNumberValidator, bool>>()))
            .Returns(validator); // Devuelve el validador cuando se busca por cualquier condición

        Exception exception = null;

        // Act: Intentar crear un dispositivo con un número de modelo inválido
        try
        {
            deviceService.CreateDevice(windowSensor, businessOwner, "Window Sensor");
        }
        catch (Exception e)
        {
            exception = e;
        }

        // Assert: Verificar que se lanzó la excepción esperada
        businessRepositoryMock.VerifyAll();

        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(DeviceException));
        Assert.AreEqual("Model number is not valid", exception.Message);
    }

    [TestMethod]
    public void Create_WindowSensor_Business_NotFound_Throws_Exception_Test()
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
            BusinessOwner = businessOwner,
            ValidatorId = id
        };

        Business businessNotFound = null;

        var windowSensor = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Window Sensor",
            Description = "Window Sensor",
            ModelNumber = "1234",
            Photos = new List<Photo>(),
            Business = business
        };

        var user = new User
        {
            Name = "X",
            Surname = "Y",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "x@y.com",
        };

        // Configurar el mock para businessRepository
        businessRepositoryMock.Setup(u => u.Find(It.IsAny<Func<Business, bool>>()))
            .Returns(businessNotFound);

        Exception exception = null;

        // Act: Intentar crear un dispositivo con un número de modelo inválido
        try
        {
            deviceService.CreateDevice(windowSensor, user, "Window Sensor");
        }
        catch (Exception e)
        {
            exception = e;
        }

        // Assert: Verificar que se lanzó la excepción esperada
        businessRepositoryMock.VerifyAll();

        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(DeviceException));
        Assert.AreEqual("Business was not found for the user", exception.Message);
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
            Photos = [],
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
        var deviceTypes = new List<string> {"Security Camera", "Inteligent Lamp",  "Window Sensor", "Movement Sensor" };

        var result = deviceService.GetAllDeviceTypes().ToList();

        CollectionAssert.AreEqual(deviceTypes, result);
    }

    [TestMethod]
    public void Create_MovementSensor_Test()
    {
        var bonwer = new User
        {
            Name = "Pedro",
            Surname = "Rodriguez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "pedrorod@gmail.com"
        };
        var movementSensor = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Movement Sensor",
            Description = "movement sensor description",
            ModelNumber = "1234",
            Photos = [],
            Type = "Movement Sensor",
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
        var expected = new Device
        {
            Id = movementSensor.Id,
            Name = "Movement Sensor",
            Description = "movement sensor description",
            ModelNumber = "1234",
            Photos = [],
            Type = "Movement Sensor",
            Business = business
        };

        businessRepositoryMock.Setup(u => u.Find(It.IsAny<Func<Business, bool>>())).Returns(business);
        deviceRepositoryMock.Setup(x => x.Add(movementSensor)).Returns(expected);

        var result = deviceService.CreateDevice(movementSensor, bonwer, "Window Sensor");

        deviceRepositoryMock.Verify(x => x.Add(movementSensor), Times.Once);

        Assert.AreEqual(movementSensor, result);
    }

    [TestMethod]
    public void Create_InteligentLamp_Test()
    {
        var bonwer = new User
        {
            Name = "Pedro",
            Surname = "Rodriguez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "pedrorod@gmail.com"
        };
        var inteligentLamp = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Inteligent Lamp",
            Description = "inteligent lamp description",
            ModelNumber = "1234",
            Photos = [],
            Type = "Inteligent Lamp",
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
        var expected = new Device
        {
            Id = inteligentLamp.Id,
            Name = "Inteligent Lamp",
            Description = "inteligent lamp description",
            ModelNumber = "1234",
            Photos = [],
            Type = "Inteligent Lamp",
            Business = business
        };

        businessRepositoryMock.Setup(u => u.Find(It.IsAny<Func<Business, bool>>())).Returns(business);
        deviceRepositoryMock.Setup(x => x.Add(inteligentLamp)).Returns(expected);

        var result = deviceService.CreateDevice(inteligentLamp, bonwer, "Inteligent Lamp");

        deviceRepositoryMock.Verify(x => x.Add(inteligentLamp), Times.Once);

        Assert.AreEqual(inteligentLamp, result);
    }

    [TestMethod]
    public void GetDevices_WithoutFilters_ReturnsAllDevices()
    {
        var devices = new List<Device>
        {
            new Device
            {
                Description = " ",
                Photos = new List<Photo>(),
                Name = "Device1",
                ModelNumber = "Model1",
                Business = new Business
                {
                    Name = "Business1",
                    Id = Guid.NewGuid(),
                    Logo = "Logo1",
                    RUT = "1234",
                },
                Type = "Type1"
            },
            new Device
            {
                Description = " ",
                Photos = new List<Photo>(),
                Name = "Device2",
                ModelNumber = "Model2",
                Business = new Business
                {
                    Name = "Business1",
                    Id = Guid.NewGuid(),
                    Logo = "Logo1",
                    RUT = "1234",
                },
                Type = "Type2"
            },
        };

        deviceRepositoryMock.Setup(x => x.FindAllFiltered(It.IsAny<Expression<Func<Device, bool>>>())).Returns(devices);

        var result = deviceService.GetDevices(null, null, null, null, null, null);

        deviceRepositoryMock.Verify(x => x.FindAllFiltered(It.IsAny<Expression<Func<Device, bool>>>()), Times.Once);
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public void GetDevices_WithDeviceNameFilter_ReturnsFilteredDevices()
    {
        var devices = new List<Device>
        {
            new Device
            {
                Description = " ",
                Photos = new List<Photo>(),
                Name = "Device1",
                ModelNumber = "Model1",
                Business = new Business
                {
                    Name = "Business1",
                    Id = Guid.NewGuid(),
                    Logo = "Logo1",
                    RUT = "1234",
                },
                Type = "Type1"
            }
        };

        deviceRepositoryMock.Setup(x => x.FindAllFiltered(It.IsAny<Expression<Func<Device, bool>>>())).Returns(devices);

        var result = deviceService.GetDevices(null, null, "Device1", null, null, null);

        deviceRepositoryMock.Verify(x => x.FindAllFiltered(It.IsAny<Expression<Func<Device, bool>>>()), Times.Once);
        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetDevices_WithDeviceModelFilter_ReturnsFilteredDevices()
    {
        var devices = new List<Device>
        {
            new Device
            {
                Description = " ",
                Photos = new List<Photo>(),
                Name = "Device1",
                ModelNumber = "Model1",
                Business = new Business
                {
                    Name = "Business1",
                    Id = Guid.NewGuid(),
                    Logo = "Logo1",
                    RUT = "1234",
                },
                Type = "Type1"
            }
        };

        deviceRepositoryMock.Setup(x => x.FindAllFiltered(It.IsAny<Expression<Func<Device, bool>>>())).Returns(devices);

        var result = deviceService.GetDevices(null, null, null, "Model1", null, null);

        deviceRepositoryMock.Verify(x => x.FindAllFiltered(It.IsAny<Expression<Func<Device, bool>>>()), Times.Once);
        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetDevices_WithBusinessNameFilter_ReturnsFilteredDevices()
    {
        var devices = new List<Device>
        {
           new Device
            {
                Description = " ",
                Photos = new List<Photo>(),
                Name = "Device1",
                ModelNumber = "Model1",
                Business = new Business
                {
                    Name = "Business1",
                    Id = Guid.NewGuid(),
                    Logo = "Logo1",
                    RUT = "1234",
                },
                Type = "Type1"
            }
        };

        deviceRepositoryMock.Setup(x => x.FindAllFiltered(It.IsAny<Expression<Func<Device, bool>>>())).Returns(devices);

        var result = deviceService.GetDevices(null, null, null, null, "Business1", null);

        deviceRepositoryMock.Verify(x => x.FindAllFiltered(It.IsAny<Expression<Func<Device, bool>>>()), Times.Once);
        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetDevices_WithDeviceTypeFilter_ReturnsFilteredDevices()
    {
        var devices = new List<Device>
        {
           new Device
            {
                Description = " ",
                Photos = new List<Photo>(),
                Name = "Device1",
                ModelNumber = "Model1",
                Business = new Business
                {
                    Name = "Business1",
                    Id = Guid.NewGuid(),
                    Logo = "Logo1",
                    RUT = "1234",
                },
                Type = "Type1"
            }
        };

        deviceRepositoryMock.Setup(x => x.FindAllFiltered(It.IsAny<Expression<Func<Device, bool>>>())).Returns(devices);

        var result = deviceService.GetDevices(null, null, null, null, null, "Type1");

        deviceRepositoryMock.Verify(x => x.FindAllFiltered(It.IsAny<Expression<Func<Device, bool>>>()), Times.Once);
        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetDevices_WithFiltersAndPagination_ReturnsPagedFilteredDevices()
    {
        var devices = new List<Device>
        {
           new Device
            {
                Description = " ",
                Photos = new List<Photo>(),
                Name = "Device1",
                ModelNumber = "Model1",
                Business = new Business
                {
                    Name = "Business1",
                    Id = Guid.NewGuid(),
                    Logo = "Logo1",
                    RUT = "1234",
                },
                Type = "Type1"
            }
        };

        deviceRepositoryMock.Setup(x => x.FindAllFiltered(It.IsAny<Expression<Func<Device, bool>>>(), 1, 1)).Returns(devices);

        var result = deviceService.GetDevices(1, 1, "Device1", "Model1", "Business1", "Type1");

        deviceRepositoryMock.Verify(x => x.FindAllFiltered(It.IsAny<Expression<Func<Device, bool>>>(), 1, 1), Times.Once);
        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetDevices_WithPaginationOnly_ReturnsPagedDevices()
    {
        var devices = new List<Device>
        {
            new Device
            {
                Description = " ",
                Photos = new List<Photo>(),
                Name = "Device1",
                ModelNumber = "Model1",
                Business = new Business
                {
                    Name = "Business1",
                    Id = Guid.NewGuid(),
                    Logo = "Logo1",
                    RUT = "1234",
                },
                Type = "Type1"
            }
        };

        deviceRepositoryMock.Setup(x => x.FindAllFiltered(It.IsAny<Expression<Func<Device, bool>>>(), 1, 1)).Returns(devices);

        var result = deviceService.GetDevices(1, 1, null, null, null, null);

        deviceRepositoryMock.Verify(x => x.FindAllFiltered(It.IsAny<Expression<Func<Device, bool>>>(), 1, 1), Times.Once);
        Assert.AreEqual(1, result.Count());
    }
}
