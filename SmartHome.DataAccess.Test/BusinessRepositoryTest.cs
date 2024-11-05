using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Test;

[TestClass]
public class BusinessRepositoryTest
{
    private readonly SmartHomeEFCoreContext _context;
    private readonly BusinessRepository _businessRepository;

    public BusinessRepositoryTest()
    {
        _context = DbContextBuilder.BuildTestDbContext();
        _businessRepository = new BusinessRepository(_context);
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
    #region Success
    [TestMethod]
    public void Add_WhenInfoIsProvided_ShouldAddedToDatabase()
    {
        var business = new Business
        {
            Id = Guid.NewGuid(),
            Name = "Test Business",
            Logo = "Test Logo",
            RUT = "Test RUT"
        };

        _businessRepository.Add(business);

        using var otherContext = DbContextBuilder.BuildTestDbContext();
        var businessesSaved = otherContext.Businesses.ToList();

        businessesSaved.Count.Should().Be(1);
        var businessSaved = businessesSaved[0];
        businessSaved.Id.Should().Be(business.Id);
        businessSaved.Name.Should().Be(business.Name);
    }

    #endregion
    #endregion

    #region GetAll
    [TestMethod]
    public void GetAll_WhenExistOnlyOne_ShouldReturnOne()
    {
        var business = new Business
        {
            Id = Guid.NewGuid(),
            Name = "Sample Business",
            Logo = "Sample Logo",
            RUT = "Sample RUT"
        };
        _context.Businesses.Add(business);
        _context.SaveChanges();

        var businesses = _businessRepository.FindAll();

        businesses.Count.Should().Be(1);
        businesses[0].Id.Should().Be(business.Id);
        businesses[0].Name.Should().Be(business.Name);
    }
    #endregion

    #region Delete
    [TestMethod]
    public void Delete_WhenBusinessExists_ShouldRemoveFromDatabase()
    {
        var business = new Business
        {
            Id = Guid.NewGuid(),
            Name = "Business to Delete",
            Logo = "Test Logo",
            RUT = "Test RUT"
        };
        _context.Businesses.Add(business);
        _context.SaveChanges();

        _businessRepository.Delete(business.Id);

        _context.Businesses.FirstOrDefault(b => b.Id == business.Id).Should().BeNull();
    }
    #endregion

    #region Find
    [TestMethod]
    public void Find_WhenBusinessExists_ShouldReturnBusiness()
    {
        var business = new Business
        {
            Id = Guid.NewGuid(),
            Name = "Find Business",
            Logo = "Test Logo",
            RUT = "Test RUT"
        };
        _context.Businesses.Add(business);
        _context.SaveChanges();

        var result = _businessRepository.Find(b => b.Id == business.Id);

        result.Should().NotBeNull();
        result.Id.Should().Be(business.Id);
        result.Name.Should().Be(business.Name);
    }
    #endregion
}

internal sealed class TestDbContext(DbContextOptions options)
    : DbContext(options)
{
    public DbSet<EntityTest> EntitiesTest { get; set; }
}

internal sealed record class EntityTest()
{
    public string Id { get; init; } = Guid.NewGuid().ToString();

    public string Name { get; init; } = null!;

    public EntityTest(string name)
        : this()
    {
        Name = name;
    }
}
