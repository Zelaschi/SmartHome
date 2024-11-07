using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Test;
[TestClass]
public class HomeMemberRepositoryTest
{
    private readonly SmartHomeEFCoreContext _context;
    private readonly HomeMemberRepository _homeMemberRepository;

    public HomeMemberRepositoryTest()
    {
        _context = DbContextBuilder.BuildTestDbContext();
        _homeMemberRepository = new HomeMemberRepository(_context);
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
