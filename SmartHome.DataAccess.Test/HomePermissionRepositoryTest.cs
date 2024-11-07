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
    #endregion

    #region Delete
    #endregion

    #region Find
    #endregion

    #region Update
    #endregion

    #region FindAll
    #endregion
}
