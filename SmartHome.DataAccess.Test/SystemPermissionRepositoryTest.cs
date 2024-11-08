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
        var systemPermission = new SystemPermission
        {
            Id = Guid.NewGuid(),
            Name = "Test Permission",
            Description = "Test Description"
        };

        var result = _systemPermissionRepository.Add(systemPermission);

        result.Should().NotBeNull();
        result.Id.Should().Be(systemPermission.Id);
        result.Name.Should().Be(systemPermission.Name);
        result.Description.Should().Be(systemPermission.Description);
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
