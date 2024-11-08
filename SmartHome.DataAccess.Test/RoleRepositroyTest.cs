using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Test;

[TestClass]
public class RoleRepositoryTest
{
    private readonly SmartHomeEFCoreContext _context;
    private readonly RoleRepository _roleRepository;

    public RoleRepositoryTest()
    {
        _context = DbContextBuilder.BuildTestDbContext();
        _roleRepository = new RoleRepository(_context);
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
    #endregion

    #region Delete
    #endregion

    #region Find
    #endregion

    #region Update
    #endregion

    #region GetAll
    #endregion
}
