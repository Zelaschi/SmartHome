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
    #endregion

    #region Update
    #endregion

    #region Find
    #endregion

    #region GetAll
    #endregion
}
