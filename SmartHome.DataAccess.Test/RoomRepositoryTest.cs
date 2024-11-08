using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Test;

[TestClass]
public class RoomRepositoryTest
{
    private readonly SmartHomeEFCoreContext _context;
    private readonly RoomRepository _roomRepository;

    public RoomRepositoryTest()
    {
        _context = DbContextBuilder.BuildTestDbContext();
        _roomRepository = new RoomRepository(_context);
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

    #region Update
    #endregion

    #region Delete
    #endregion

    #region GetAll
    #endregion

    #region Find
    #endregion
}
