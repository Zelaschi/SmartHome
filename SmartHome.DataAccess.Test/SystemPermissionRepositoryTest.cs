using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Services;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Test;

[TestClass]
public class SystemPermissionRepositoryTest
{
    private readonly SmartHomeEFCoreContext _context;
    private readonly SystemPermissionRepository _systemPermissionRepository;

    public SystemPermissionRepositoryTest()
    {
        _context = DbContextBuilder.BuildTestDbContext();
        _systemPermissionRepository = new SystemPermissionRepository(_context);
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
    public void Add_SystemPermissionEntity_ShouldReturnSystemPermissionEntity()
    {
        _context.SystemPermissions.RemoveRange(_context.SystemPermissions);
        var systemPermission = new SystemPermission
        {
            Id = Guid.NewGuid(),
            Name = "Test Permission",
            Description = "Test Description"
        };
        _systemPermissionRepository.Add(systemPermission);
        _context.SaveChanges();

        using var otherContext = DbContextBuilder.BuildTestDbContext();
        var sysPermissionsSaved = otherContext.SystemPermissions.ToList();

        sysPermissionsSaved.Count.Should().Be(1);
        var sysPermissionSaved = sysPermissionsSaved[0];
        sysPermissionSaved.Id.Should().Be(systemPermission.Id);
        sysPermissionSaved.Name.Should().Be(systemPermission.Name);
    }
    #endregion

    #region Delete
    [TestMethod]
    public void Delete_SystemPermissionEntity_ShouldNotThrowException()
    {
        _context.SystemPermissions.RemoveRange(_context.SystemPermissions);
        var systemPermission = new SystemPermission
        {
            Id = Guid.NewGuid(),
            Name = "Test Permission",
            Description = "Test Description"
        };

        _systemPermissionRepository.Add(systemPermission);
        _context.SaveChanges();

        _systemPermissionRepository.Delete(systemPermission.Id);

        _context.SystemPermissions.FirstOrDefault(sp => sp.Id == systemPermission.Id).Should().BeNull();
    }
    #endregion

    #region Update
    #endregion

    #region Find
    #endregion

    #region GetAll
    #endregion
}
