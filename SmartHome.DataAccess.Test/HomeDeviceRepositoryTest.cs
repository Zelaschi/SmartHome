using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Test;

[TestClass]
public class HomeDeviceRepositoryTest
{
    private readonly SmartHomeEFCoreContext _context;
    private readonly HomeDeviceRepository _homeDeviceRepository;

    public HomeDeviceRepositoryTest()
    {
        _context = DbContextBuilder.BuildTestDbContext();
        _homeDeviceRepository = new HomeDeviceRepository(_context);
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
            ModelNumber = "Model123",
            Description = "Test Description",
            Photos = new List<Photo>()
        };

        _context.Devices.Add(device);
        _context.SaveChanges();

        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "User Role"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        var owner = new User
        {
            Name = "Test Name",
            Surname = "Test Surname",
            Password = "TestPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };

        _context.Users.Add(owner);
        _context.SaveChanges();

        var home = new Home
        {
            Id = Guid.NewGuid(),
            Name = "Test Home",
            MainStreet = "Test Street",
            DoorNumber = "123",
            Latitude = "0.0000",
            Longitude = "0.0000",
            MaxMembers = 4,
            Owner = owner
        };

        _context.Homes.Add(home);
        _context.SaveChanges();

        var homeDevice = new HomeDevice
        {
            Id = Guid.NewGuid(),
            Name = "Test HomeDevice",
            Device = device,
            HomeId = home.Id,
            Online = true
        };
        _homeDeviceRepository.Add(homeDevice);
        _context.SaveChanges();

        using var otherContext = DbContextBuilder.BuildTestDbContext();
        var homeDevicesSaved = otherContext.HomeDevices.ToList();

        homeDevicesSaved.Count.Should().Be(1);
        var homeDeviceSaved = homeDevicesSaved[0];
        homeDeviceSaved.Id.Should().Be(homeDevice.Id);
        homeDeviceSaved.Name.Should().Be(homeDevice.Name);
    }
    #endregion
    #endregion
}
