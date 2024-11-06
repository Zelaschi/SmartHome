using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Test;
public class HomeDeviceRepositoryTest
{
    private readonly SmartHomeEFCoreContext _context;
    private readonly HomeDeviceRepository _homeDeviceRepository;

    public HomeDeviceRepositoryTest()
    {
        _context = DbContextBuilder.BuildTestDbContext();
        _homeDeviceRepository = new HomeDeviceRepository(_context);
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
}
