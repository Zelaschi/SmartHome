using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Services;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Test;
[TestClass]
public class HomePermissionRepositoryTest
{
    private readonly SmartHomeEFCoreContext _context;
    private readonly HomePermissionRepository _homePermissionRepository;

    public HomePermissionRepositoryTest()
    {
        _context = DbContextBuilder.BuildTestDbContext();
        _homePermissionRepository = new HomePermissionRepository(_context);
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
    public void Add_WhenInfoIsProvided_ShouldAddedToDatabase()
    {
        _context.HomePermissions.RemoveRange(_context.HomePermissions);
        var homePermission = new HomePermission
        {
            Id = Guid.NewGuid(),
            Name = "Test Home Permission"
        };

        _homePermissionRepository.Add(homePermission);

        using var otherContext = DbContextBuilder.BuildTestDbContext();
        var homePermissionsSaved = otherContext.HomePermissions.ToList();

        homePermissionsSaved.Count.Should().Be(1);
        var homePermissionSaved = homePermissionsSaved[0];
        homePermissionSaved.Id.Should().Be(homePermission.Id);
        homePermissionSaved.Name.Should().Be(homePermission.Name);
    }
    #endregion

    #region Delete
    [TestMethod]
    public void Delete_WhenHomePermissionExists_ShouldRemoveFromDatabase()
    {
        _context.HomePermissions.RemoveRange(_context.HomePermissions);
        var homePermission = new HomePermission
        {
            Id = Guid.NewGuid(),
            Name = "Test Home Permission"
        };

        _homePermissionRepository.Add(homePermission);
        _context.SaveChanges();

        _homePermissionRepository.Delete(homePermission.Id);

        _context.HomePermissions.FirstOrDefault(hp => hp.Id == homePermission.Id).Should().BeNull();
    }

    [TestMethod]
    public void Delete_WhenHomePermissionDoesNotExists_ShouldThrowException()
    {
        _context.HomePermissions.RemoveRange(_context.HomePermissions);
        var homePermission = new HomePermission
        {
            Id = Guid.NewGuid(),
            Name = "Test Home Permission"
        };

        Action action = () => _homePermissionRepository.Delete(homePermission.Id);

        action.Should().Throw<DatabaseException>()
            .WithMessage("The HomePermission does not exist in the Data Base.");
    }
    #endregion

    #region Find
    [TestMethod]
    public void Find_WhenHomePermissionExists_ShouldReturnBusiness()
    {
        _context.HomePermissions.RemoveRange(_context.HomePermissions);
        var homePermission = new HomePermission
        {
            Id = Guid.NewGuid(),
            Name = "Test Home Permission"
        };

        _homePermissionRepository.Add(homePermission);
        _context.SaveChanges();

        var result = _homePermissionRepository.Find(hp => hp.Id == homePermission.Id);

        result.Should().NotBeNull();
        result.Id.Should().Be(homePermission.Id);
        result.Name.Should().Be(homePermission.Name);
    }
    #endregion

    #region Update
    [TestMethod]
    public void Update_WhenHomeDeviceExists_ShouldUpdateInDatabase()
    {
        _context.HomePermissions.RemoveRange(_context.HomePermissions);
        var homePermission = new HomePermission
        {
            Id = Guid.NewGuid(),
            Name = "Test Home Permission"
        };

        _homePermissionRepository.Add(homePermission);
        _context.SaveChanges();

        var updatedHomePermission = new HomePermission
        {
            Id = homePermission.Id,
            Name = "Updated Home Permission"
        };

        var result = _homePermissionRepository.Update(updatedHomePermission);

        result.Should().NotBeNull();
        result.Id.Should().Be(homePermission.Id);
        result.Name.Should().Be("Updated Home Permission");

        var updatedEntityInDb = _context.HomePermissions.FirstOrDefault(hp => hp.Id == homePermission.Id);
        updatedEntityInDb.Should().NotBeNull();
        updatedEntityInDb.Name.Should().Be("Updated Home Permission");
    }
    #endregion

    #region FindAll
    [TestMethod]
    public void GetAll_WhenExistOnlyOne_ShouldReturnOne()
    {
        _context.HomePermissions.RemoveRange(_context.HomePermissions);
        var homePermission = new HomePermission
        {
            Id = Guid.NewGuid(),
            Name = "Test Home Permission"
        };

        _homePermissionRepository.Add(homePermission);
        _context.SaveChanges();

        var homePermissions = _homePermissionRepository.FindAll();

        homePermissions.Count.Should().Be(1);
        homePermissions[0].Id.Should().Be(homePermission.Id);
        homePermissions[0].Name.Should().Be(homePermission.Name);
    }
    #endregion
}
