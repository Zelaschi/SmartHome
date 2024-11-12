using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Services;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Test;

[TestClass]
public class ValidatorRepositoryTest
{
    private readonly SmartHomeEFCoreContext _context;
    private readonly ValidatorRepository _validatorRepository;

    public ValidatorRepositoryTest()
    {
        _context = DbContextBuilder.BuildTestDbContext();
        _validatorRepository = new ValidatorRepository(_context);
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
    public void AddValidator_WhenValidatorIsCorrect_ShouldAdd()
    {
        var validator = new Validator
        {
            Name = "test"
        };
        _validatorRepository.Add(validator);
        _context.SaveChanges();

        using var otherContext = DbContextBuilder.BuildTestDbContext();
        var validatorsSaved = otherContext.Validators.ToList();

        validatorsSaved.Count.Should().Be(1);
        var validatorSaved = validatorsSaved[0];
        validatorSaved.Id.Should().Be(validator.Id);
        validatorSaved.Name.Should().Be(validator.Name);
    }
    #endregion

    #region Delete
    #endregion

    #region Update
    #endregion

    #region Find
    #endregion

    #region GetAl
    #endregion

}
