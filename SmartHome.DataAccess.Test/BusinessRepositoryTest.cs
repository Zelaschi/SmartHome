using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;
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

    #region FindAll
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

    [TestMethod]
    public void Delete_WhenBusinessDoesNotExists_ShouldRemoveFromDatabase()
    {
        var business = new Business
        {
            Id = Guid.NewGuid(),
            Name = "Business to Delete",
            Logo = "Test Logo",
            RUT = "Test RUT"
        };

        Action action = () => _businessRepository.Delete(business.Id);

        action.Should().Throw<DatabaseException>()
            .WithMessage("The Business does not exist in the Data Base.");
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

    #region Update
    [TestMethod]
    public void Update_WhenBusinessExists_ShouldUpdateInDatabase()
    {
        var business = new Business
        {
            Id = Guid.NewGuid(),
            Name = "Original Name",
            Logo = "Original Logo",
            RUT = "Original RUT"
        };
        _context.Businesses.Add(business);
        _context.SaveChanges();

        var updatedBusiness = new Business
        {
            Id = business.Id,
            Name = "Updated Name",
            Logo = "Updated Logo",
            RUT = "Updated RUT"
        };

        var result = _businessRepository.Update(updatedBusiness);

        result.Should().NotBeNull();
        result.Id.Should().Be(business.Id);
        result.Name.Should().Be("Updated Name");

        var updatedEntityInDb = _context.Businesses.FirstOrDefault(b => b.Id == business.Id);
        updatedEntityInDb.Should().NotBeNull();
        updatedEntityInDb.Name.Should().Be("Updated Name");
    }

    [TestMethod]
    public void Update_WhenBusinessDoesNotExist_ShouldThrowDatabaseException()
    {
        var nonExistingBusiness = new Business
        {
            Id = Guid.NewGuid(),
            Name = "Non-existing Business",
            Logo = "Non-existing Logo",
            RUT = "Non-existing RUT"
        };

        Action action = () => _businessRepository.Update(nonExistingBusiness);

        action.Should().Throw<DatabaseException>()
            .WithMessage("The Business does not exist in the Data Base.");
    }

    #endregion

    #region Filtered
    [TestMethod]
    public void FindAllFiltered_ShouldReturnAllBusinesses_WhenNoFilterIsProvided()
    {
        var business = new Business
        {
            Id = Guid.NewGuid(),
            Name = "Original Name",
            Logo = "Original Logo",
            RUT = "Original RUT"
        };

        var business2 = new Business
        {
            Id = Guid.NewGuid(),
            Name = "Original Name 2",
            Logo = "Original Logo 2",
            RUT = "Original RUT 2"
        };
        _context.Businesses.Add(business2);
        _context.Businesses.Add(business);
        _context.SaveChanges();

        var result = _businessRepository.FindAllFiltered(null, 1, 10);

        result.Should().HaveCount(2);
    }

    [TestMethod]
    public void FindAllFiltered_ShouldReturnFilteredBusinesses_WhenFilterIsProvided()
    {
        var business = new Business
        {
            Id = Guid.NewGuid(),
            Name = "Original Name",
            Logo = "Original Logo",
            RUT = "Original RUT"
        };

        var business2 = new Business
        {
            Id = Guid.NewGuid(),
            Name = "Original Name 2",
            Logo = "Original Logo 2",
            RUT = "Original RUT 2"
        };
        _context.Businesses.Add(business2);
        _context.Businesses.Add(business);
        _context.SaveChanges();

        var result = _businessRepository.FindAllFiltered(b => b.Name == "Original Name", 1, 10);

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Original Name");
        _businessRepository.FindAll().Should().HaveCount(2);
    }

    [TestMethod]
    public void FindAllFiltered_ShouldReturnPagedBusinesses_WhenPageNumberAndPageSizeAreProvided()
    {
        var business = new Business
        {
            Id = Guid.NewGuid(),
            Name = "Original Name",
            Logo = "Original Logo",
            RUT = "Original RUT"
        };

        var business2 = new Business
        {
            Id = Guid.NewGuid(),
            Name = "Original Name 2",
            Logo = "Original Logo 2",
            RUT = "Original RUT 2"
        };
        _context.Businesses.Add(business2);
        _context.Businesses.Add(business);
        _context.SaveChanges();

        var result = _businessRepository.FindAllFiltered(null, 1, 1);

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Original Name");
    }

    [TestMethod]
    public void FindAllFiltered_ShouldReturnEmptyList_WhenNoBusinessesAreFound()
    {
        var result = _businessRepository.FindAllFiltered(null, 1, 10);

        result.Should().BeEmpty();
    }

    [TestMethod]
    public void FindAllFiltered_OneParam_ShouldReturnAllDevices_WhenNoFilterIsProvided()
    {
        var business = new Business
        {
            Id = Guid.NewGuid(),
            Name = "Original Name",
            Logo = "Original Logo",
            RUT = "Original RUT"
        };

        var business2 = new Business
        {
            Id = Guid.NewGuid(),
            Name = "Original Name 2",
            Logo = "Original Logo 2",
            RUT = "Original RUT 2"
        };
        _context.Businesses.Add(business2);
        _context.Businesses.Add(business);
        _context.SaveChanges();

        var result = _businessRepository.FindAllFiltered(null);

        result.Should().HaveCount(2);
    }

    [TestMethod]
    public void FindAllFiltered_OneParam_ShouldReturnBusinessesFiltered_WhenFilterIsProvided()
    {
        var business = new Business
        {
            Id = Guid.NewGuid(),
            Name = "Original Name",
            Logo = "Original Logo",
            RUT = "Original RUT"
        };

        var business2 = new Business
        {
            Id = Guid.NewGuid(),
            Name = "Original Name 2",
            Logo = "Original Logo 2",
            RUT = "Original RUT 2"
        };
        _context.Businesses.Add(business2);
        _context.Businesses.Add(business);
        _context.SaveChanges();

        var result = _businessRepository.FindAllFiltered(b => b.Name == "Original Name");

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Original Name");
    }
    #endregion
}
