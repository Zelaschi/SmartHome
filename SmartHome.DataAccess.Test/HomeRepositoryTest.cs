using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Services;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Test;
[TestClass]
public class HomeRepositoryTest
{
    private readonly SmartHomeEFCoreContext _context;
    private readonly HomeRepository _homeRepository;

    public HomeRepositoryTest()
    {
        _context = DbContextBuilder.BuildTestDbContext();
        _homeRepository = new HomeRepository(_context);
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
    [TestMethod]
    public void Add_HomeEntity_ShouldReturnHomeEntity()
    {
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

        using var otherContext = DbContextBuilder.BuildTestDbContext();
        var homesSaved = otherContext.Homes.ToList();

        homesSaved.Count.Should().Be(1);
        var homeSaved = homesSaved[0];
        homeSaved.Id.Should().Be(home.Id);
        homeSaved.Name.Should().Be(home.Name);
    }
    #endregion

    #region Delete
    [TestMethod]
    public void Delete_WhenHomeExists_ShouldRemoveFromDatabase()
    {
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

        _homeRepository.Delete(home.Id);

        _context.Homes.FirstOrDefault(h => h.Id == home.Id).Should().BeNull();
    }

    [TestMethod]
    public void Delete_WhenHomeDoesNotExists_ShouldRemoveFromDatabase()
    {
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

        var nonExistingHome = new Home
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

        Action action = () => _homeRepository.Delete(nonExistingHome.Id);

        action.Should().Throw<DatabaseException>()
            .WithMessage("The Home does not exist in the Data Base.");
    }
    #endregion

    #region Update
    [TestMethod]
    public void Update_WhenHomeExists_ShouldUpdateInDatabase()
    {
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

        home.Name = "Updated Home";
        home.MainStreet = "Updated Street";
        home.DoorNumber = "321";
        home.Latitude = "1.0000";
        home.Longitude = "1.0000";
        home.MaxMembers = 2;

        var result = _homeRepository.Update(home);

        result.Should().NotBeNull();
        result.Id.Should().Be(home.Id);
        result.Name.Should().Be("Updated Home");

        var updatedEntityInDb = _context.Homes.FirstOrDefault(h => h.Id == home.Id);
        updatedEntityInDb.Should().NotBeNull();
        updatedEntityInDb.Name.Should().Be("Updated Home");
    }

    [TestMethod]
    public void Update_WhenHomeDoesNotExist_ShouldThrowDatabaseException()
    {
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

        home.Name = "Updated Home";
        home.MainStreet = "Updated Street";
        home.DoorNumber = "321";
        home.Latitude = "1.0000";
        home.Longitude = "1.0000";
        home.MaxMembers = 2;

        Action action = () =>
        {
            try
            {
                _homeRepository.Update(home);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DatabaseException("The Home does not exist in the Data Base.");
            }
        };

        action.Should().Throw<DatabaseException>()
            .WithMessage("The Home does not exist in the Data Base.");
    }

    #endregion

    #region Find
    [TestMethod]
    public void Find_WhenHomeExists_ShouldReturnHomeWithAllRelatedEntities()
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "User Role"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        var owner = new User
        {
            Name = "Test Owner",
            Surname = "Owner Surname",
            Password = "TestPassword123",
            Email = "owner@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(owner);
        _context.SaveChanges();

        var memberUser = new User
        {
            Name = "Test Member",
            Surname = "Member Surname",
            Password = "MemberPassword123",
            Email = "member@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(memberUser);
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

        var homePermission = new HomePermission
        {
            Id = Guid.NewGuid(),
            Name = "View Devices"
        };
        _context.HomePermissions.Add(homePermission);
        _context.SaveChanges();

        var homeMember = new HomeMember
        {
            HomeMemberId = Guid.NewGuid(),
            HomeId = home.Id,
            User = memberUser,
            HomePermissions = new List<HomePermission> { homePermission }
        };
        _context.HomeMembers.Add(homeMember);
        _context.SaveChanges();

        var device = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Test Device",
            ModelNumber = "Device123",
            Description = "Test Description",
            Photos = new List<Photo>()
        };
        _context.Devices.Add(device);
        _context.SaveChanges();

        var homeDevice = new HomeDevice
        {
            Id = Guid.NewGuid(),
            Name = "Home Device",
            HomeId = home.Id,
            Device = device,
            Online = true
        };
        _context.HomeDevices.Add(homeDevice);
        _context.SaveChanges();

        var result = _homeRepository.Find(h => h.Id == home.Id);

        result.Should().NotBeNull();
        result.Id.Should().Be(home.Id);
        result.Name.Should().Be("Test Home");

        result.Members.Should().NotBeNullOrEmpty();
        result.Members.First().User.Should().NotBeNull();
        result.Members.First().User.Name.Should().Be("Test Member");
        result.Members.First().HomePermissions.Should().NotBeNullOrEmpty();
        result.Devices.Should().NotBeNullOrEmpty();
        result.Devices.First().Device.Name.Should().Be("Test Device");
    }
    #endregion

    #region GetAll
    #endregion
}
