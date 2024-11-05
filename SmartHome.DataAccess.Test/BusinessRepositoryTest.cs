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
    }

    #endregion
    #endregion

    #region GetAll
    ////[TestMethod]
    ////public void GetAll_WhenExistOnlyOne_ShouldReturnOne()
    ////{
    ////    var expectedEntity = new EntityTest
    ////    {
    ////        Name = "dummy"
    ////    };
    ////    using var context = DbContextBuilder.BuildTestDbContext();
    ////    context.Add(expectedEntity);
    ////    context.SaveChanges();

    ////    var entitiesSaved = _repository.GetAll();

    ////    entitiesSaved.Count.Should().Be(1);

    ////    var entitySaved = entitiesSaved[0];
    ////    entitySaved.Id.Should().Be(expectedEntity.Id);
    ////    entitySaved.Name.Should().Be(expectedEntity.Name);
    ////}
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
