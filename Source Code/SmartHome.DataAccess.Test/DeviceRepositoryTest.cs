using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Test;

[TestClass]
public class DeviceRepositoryTest
{
    private readonly SmartHomeEFCoreContext _context;
    private readonly DeviceRepository _deviceRepository;

    public DeviceRepositoryTest()
    {
        _context = DbContextBuilder.BuildTestDbContext();
        _deviceRepository = new DeviceRepository(_context);
    }

    [TestInitialize]
    public void Setup()
    {
        _context.Database.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    #region Add
    #region Success
    [TestMethod]
    public void Add_WhenInfoIsProvided_ShouldAddedToDatabase()
    {
        var device = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Test Device",
            ModelNumber = "Test Model Number",
            Description = "Test Description",
            Photos = new List<Photo>()
        };

        _deviceRepository.Add(device);

        using var otherContext = DbContextBuilder.BuildTestDbContext();
        var devicesSaved = otherContext.Devices.ToList();

        devicesSaved.Count.Should().Be(1);
        var deviceSaved = devicesSaved[0];
        deviceSaved.Id.Should().Be(device.Id);
        deviceSaved.Name.Should().Be(device.Name);
    }

    #endregion
    #endregion

    #region GetAll
    [TestMethod]
    public void GetAll_WhenExistOnlyOne_ShouldReturnOne()
    {
        var device = new Device
        {
            Description = "Test Description",
            Id = Guid.NewGuid(),
            ModelNumber = "Test Model Number",
            Name = "Test Device",
            Photos = new List<Photo>()
        };
        _context.Devices.Add(device);
        _context.SaveChanges();

        var devices = _deviceRepository.FindAll();

        devices.Count.Should().Be(1);
        devices[0].Id.Should().Be(device.Id);
        devices[0].Name.Should().Be(device.Name);
    }
    #endregion

    #region Delete
    [TestMethod]
    public void Delete_WhenDeviceExists_ShouldRemoveFromDatabase()
    {
        var device = new Device
        {
            Description = "Test Description",
            Id = Guid.NewGuid(),
            ModelNumber = "Test Model Number",
            Name = "Test Device",
            Photos = new List<Photo>()
        };
        _context.Devices.Add(device);
        _context.SaveChanges();

        _deviceRepository.Delete(device.Id);

        _context.Devices.FirstOrDefault(b => b.Id == device.Id).Should().BeNull();
    }

    [TestMethod]
    public void Delete_WhenDeviceDoesNotExists_ShouldRemoveFromDatabase()
    {
        var device = new Device
        {
            Description = "Test Description",
            Id = Guid.NewGuid(),
            ModelNumber = "Test Model Number",
            Name = "Test Device",
            Photos = new List<Photo>()
        };

        Action action = () => _deviceRepository.Delete(device.Id);

        action.Should().Throw<DatabaseException>()
            .WithMessage("The Device does not exist in the Data Base.");
    }
    #endregion

    #region Find
    [TestMethod]
    public void Find_WhenDeviceExists_ShouldReturnDevice()
    {
        var device = new Device
        {
            Description = "Test Description",
            Id = Guid.NewGuid(),
            ModelNumber = "Test Model Number",
            Name = "Test Device",
            Photos = new List<Photo>()
        };
        _context.Devices.Add(device);
        _context.SaveChanges();

        var result = _deviceRepository.Find(b => b.Id == device.Id);

        result.Should().NotBeNull();
        result.Id.Should().Be(device.Id);
        result.Name.Should().Be(device.Name);
    }
    #endregion

    #region Update
    [TestMethod]
    public void Update_WhenDeviceExists_ShouldUpdateInDatabase()
    {
        var device = new Device
        {
            Description = "Test Description",
            Id = Guid.NewGuid(),
            ModelNumber = "Test Model Number",
            Name = "Test Device",
            Photos = new List<Photo>()
        };
        _context.Devices.Add(device);
        _context.SaveChanges();

        var updatedDevice = new Device
        {
            Description = "Updated Description",
            Id = device.Id,
            ModelNumber = "Updated Model Number",
            Name = "Updated Device",
            Photos = new List<Photo>()
        };

        var result = _deviceRepository.Update(updatedDevice);

        result.Should().NotBeNull();
        result.Id.Should().Be(device.Id);
        result.Name.Should().Be("Updated Device");

        var updatedEntityInDb = _context.Devices.FirstOrDefault(b => b.Id == device.Id);
        updatedEntityInDb.Should().NotBeNull();
        updatedEntityInDb.Name.Should().Be("Updated Device");
    }

    [TestMethod]
    public void Update_WhenDeviceDoesNotExist_ShouldThrowDatabaseException()
    {
        var nonExistingDevice = new Device
        {
            Description = "Non-existing Description",
            Id = Guid.NewGuid(),
            ModelNumber = "Non-existing Model Number",
            Name = "Non-existing Device",
            Photos = new List<Photo>()
        };

        Action action = () => _deviceRepository.Update(nonExistingDevice);

        action.Should().Throw<DatabaseException>()
            .WithMessage("The Device does not exist in the Data Base.");
    }
    #endregion

    #region Filtered
    [TestMethod]
    public void FindAllFiltered_ShouldReturnAllDevices_WhenNoFilterIsProvided()
    {
        var device = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Test Device",
            ModelNumber = "Test Model Number",
            Description = "Test Description",
            Photos = new List<Photo>()
        };

        _deviceRepository.Add(device);
        _context.SaveChanges();

        var result = _deviceRepository.FindAllFiltered(null, 1, 10);

        result.Should().HaveCount(1);
    }

    [TestMethod]
    public void FindAllFiltered_ShouldReturnFilteredDevices_WhenFilterIsProvided()
    {
        var device = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Test Device",
            ModelNumber = "Test Model Number",
            Description = "Test Description",
            Photos = new List<Photo>()
        };

        var device2 = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Test Device 2",
            ModelNumber = "Test Model Number 2",
            Description = "Test Description 2",
            Photos = new List<Photo>()
        };
        _deviceRepository.Add(device2);
        _deviceRepository.Add(device);
        _context.SaveChanges();

        var result = _deviceRepository.FindAllFiltered(d => d.Name == "Test Device", 1, 10);

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Test Device");
        _deviceRepository.FindAll().Should().HaveCount(2);
    }

    [TestMethod]
    public void FindAllFiltered_ShouldReturnPagedDevices_WhenPageNumberAndPageSizeAreProvided()
    {
        var device = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Test Device",
            ModelNumber = "Test Model Number",
            Description = "Test Description",
            Photos = new List<Photo>()
        };

        var device2 = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Test Device 2",
            ModelNumber = "Test Model Number 2",
            Description = "Test Description 2",
            Photos = new List<Photo>()
        };
        _deviceRepository.Add(device);
        _deviceRepository.Add(device2);
        _context.SaveChanges();

        var result = _deviceRepository.FindAllFiltered(null, 1, 1);

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Test Device");
    }

    [TestMethod]
    public void FindAllFiltered_ShouldReturnEmptyList_WhenNoDevicesAreFound()
    {
        var result = _deviceRepository.FindAllFiltered(null, 1, 10);

        result.Should().BeEmpty();
    }

    [TestMethod]
    public void FindAllFiltered_OneParam_ShouldReturnAllDevices_WhenNoFilterIsProvided()
    {
        var device = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Test Device",
            ModelNumber = "Test Model Number",
            Description = "Test Description",
            Photos = new List<Photo>()
        };

        var device2 = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Test Device 2",
            ModelNumber = "Test Model Number 2",
            Description = "Test Description 2",
            Photos = new List<Photo>()
        };
        _deviceRepository.Add(device);
        _deviceRepository.Add(device2);
        _context.SaveChanges();

        var result = _deviceRepository.FindAllFiltered(null);

        // Assert
        result.Should().HaveCount(2);
    }

    [TestMethod]
    public void FindAllFiltered_OneParam_ShouldReturnDevicesFiltered_WhenFilterIsProvided()
    {
        var device = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Test Device",
            ModelNumber = "Test Model Number",
            Description = "Test Description",
            Photos = new List<Photo>()
        };

        var device2 = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Test Device 2",
            ModelNumber = "Test Model Number 2",
            Description = "Test Description 2",
            Photos = new List<Photo>()
        };
        _deviceRepository.Add(device);
        _deviceRepository.Add(device2);
        _context.SaveChanges();

        var result = _deviceRepository.FindAllFiltered(d => d.Name == "Test Device");

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Test Device");
    }

    #endregion
}
