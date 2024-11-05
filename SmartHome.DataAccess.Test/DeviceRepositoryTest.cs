using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Test;

[TestClass]
public class DeviceRepositoryTest
{
    private readonly SmartHomeEFCoreContext _context;
    private readonly DeviceRepository _deviceRepository;

    public DeviceRepositoryTest()
    {
        _context = DbContextBuilder.BuildTestDbContext();
        _deviceRepository = new DeviceRepository(_context);
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
