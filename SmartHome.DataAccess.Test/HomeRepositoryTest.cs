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

        // Usar la entidad ya rastreada en lugar de crear una nueva instancia
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

    #endregion

    #region Find
    #endregion

    #region GetAll
    #endregion
}
