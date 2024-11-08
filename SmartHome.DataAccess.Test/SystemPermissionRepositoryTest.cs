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

    [TestMethod]
    public void Delete_SystemPermissionEntity_ShouldThrowException()
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

        Action act = () => _systemPermissionRepository.Delete(Guid.NewGuid());

        act.Should().Throw<DatabaseException>()
            .WithMessage("The SystemPermission does not exist in the Data Base.");
    }
    #endregion

    #region Update
    [TestMethod]
    public void Update_SystemPermissionEntity_ShouldReturnSystemPermissionEntity()
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

        var systemPermissionUpdated = new SystemPermission
        {
            Id = systemPermission.Id,
            Name = "Test Permission Updated",
            Description = "Test Description Updated"
        };

        _systemPermissionRepository.Update(systemPermissionUpdated);
        _context.SaveChanges();

        using var otherContext = DbContextBuilder.BuildTestDbContext();
        var sysPermissionsSaved = otherContext.SystemPermissions.ToList();

        sysPermissionsSaved.Count.Should().Be(1);
        var sysPermissionSaved = sysPermissionsSaved[0];
        sysPermissionSaved.Id.Should().Be(systemPermission.Id);
        sysPermissionSaved.Name.Should().Be(systemPermissionUpdated.Name);
        sysPermissionSaved.Description.Should().Be(systemPermissionUpdated.Description);
    }

    [TestMethod]
    public void Update_SystemPermissionEntityNotFound_ShouldThrowException()
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

        var systemPermissionUpdated = new SystemPermission
        {
            Id = Guid.NewGuid(),
            Name = "Test Permission Updated",
            Description = "Test Description Updated"
        };

        Action act = () => _systemPermissionRepository.Update(systemPermissionUpdated);

        act.Should().Throw<DatabaseException>()
            .WithMessage("The SystemPermission does not exist in the Data Base.");
    }
    #endregion

    #region Find
    [TestMethod]
    public void Find_SystemPermissionEntity_ShouldReturnSystemPermissionEntity()
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

        var systemPermissionFound = _systemPermissionRepository.Find(sp => sp.Id == systemPermission.Id);

        systemPermissionFound.Should().NotBeNull();
        systemPermissionFound!.Id.Should().Be(systemPermission.Id);
        systemPermissionFound.Name.Should().Be(systemPermission.Name);
        systemPermissionFound.Description.Should().Be(systemPermission.Description);
    }
    #endregion

    #region GetAll
    [TestMethod]
    public void GetAll_SystemPermissionEntities_ShouldReturnSystemPermissionEntities()
    {
        _context.SystemPermissions.RemoveRange(_context.SystemPermissions);
        var systemPermission1 = new SystemPermission
        {
            Id = Guid.NewGuid(),
            Name = "Test Permission 1",
            Description = "Test Description 1"
        };
        var systemPermission2 = new SystemPermission
        {
            Id = Guid.NewGuid(),
            Name = "Test Permission 2",
            Description = "Test Description 2"
        };

        _systemPermissionRepository.Add(systemPermission1);
        _systemPermissionRepository.Add(systemPermission2);
        _context.SaveChanges();

        var systemPermissionsFound = _systemPermissionRepository.FindAll();

        systemPermissionsFound.Count.Should().Be(2);
        systemPermissionsFound[0].Id.Should().Be(systemPermission1.Id);
        systemPermissionsFound[0].Name.Should().Be(systemPermission1.Name);
        systemPermissionsFound[1].Id.Should().Be(systemPermission2.Id);
        systemPermissionsFound[1].Name.Should().Be(systemPermission2.Name);
    }
    #endregion
}
