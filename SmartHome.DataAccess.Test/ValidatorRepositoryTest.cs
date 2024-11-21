using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
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
        var validator = new ModelNumberValidator
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
    [TestMethod]
    public void Delete_WhenValidatorExists_ShouldRemoveFromDatabase()
    {
        _context.Validators.RemoveRange(_context.Validators);
        var validator = new ModelNumberValidator
        {
            Name = "Admin"
        };
        _validatorRepository.Add(validator);
        _context.SaveChanges();

        _validatorRepository.Delete(validator.Id);

        _context.Validators.FirstOrDefault(v => v.Id == validator.Id).Should().BeNull();
    }

    [TestMethod]
    public void Delete_WhenValidatorDoesNotExist_ShouldThrowDatabaseException()
    {
        _context.Validators.RemoveRange(_context.Validators);
        var validator = new ModelNumberValidator
        {
            Name = "Test"
        };
        _context.Validators.Add(validator);
        _context.SaveChanges();

        Action action = () => _validatorRepository.Delete(Guid.NewGuid());
        action.Should().Throw<DatabaseException>()
            .WithMessage("The Validator does not exist in the Data Base.");
    }
    #endregion

    #region Update
    [TestMethod]
    public void Update_WhenValidatorExists_ShouldUpdateRole()
    {
        _context.Validators.RemoveRange(_context.Validators);
        var validator = new ModelNumberValidator
        {
            Name = "Test"
        };
        _context.Validators.Add(validator);
        _context.SaveChanges();

        var updatedValidator = new ModelNumberValidator
        {
            Id = validator.Id,
            Name = "Updated validator"
        };

        var result = _validatorRepository.Update(updatedValidator);

        result.Should().NotBeNull();
        result.Id.Should().Be(validator.Id);
        result.Name.Should().Be("Updated validator");

        var updatedEntityInDb = _context.Validators.FirstOrDefault(v => v.Id == validator.Id);
        updatedEntityInDb.Should().NotBeNull();
        updatedEntityInDb.Name.Should().Be("Updated validator");
    }

    [TestMethod]
    public void Update_WhenValidatorDoesNotExist_ShouldThrowDatabaseException()
    {
        _context.Validators.RemoveRange(_context.Validators);
        var validator = new ModelNumberValidator
        {
            Name = "Test"
        };
        _context.Validators.Add(validator);
        _context.SaveChanges();

        var updatedValidator = new ModelNumberValidator
        {
            Id = Guid.NewGuid(),
            Name = "Updated Validator"
        };

        Action action = () => _validatorRepository.Update(updatedValidator);
        action.Should().Throw<DatabaseException>()
            .WithMessage("The Validator does not exist in the Data Base.");
    }
    #endregion

    #region Find
    [TestMethod]
    public void Find_WhenValidatorExists_ShouldReturnValidator()
    {
        _context.Validators.RemoveRange(_context.Validators);
        var validator = new ModelNumberValidator
        {
            Name = "Test"
        };
        _context.Validators.Add(validator);
        _context.SaveChanges();

        var validatorFound = _validatorRepository.Find(v => v.Id == validator.Id);

        validatorFound.Should().NotBeNull();
        validatorFound!.Id.Should().Be(validator.Id);
        validatorFound.Name.Should().Be(validator.Name);
    }
    #endregion

    #region GetAl
    [TestMethod]
    public void FindAll_WhenExistOnlyOne_ShouldReturnOne()
    {
        _context.Validators.RemoveRange(_context.Validators);
        var validator = new ModelNumberValidator
        {
            Name = "Test"
        };
        _validatorRepository.Add(validator);
        _context.SaveChanges();

        var validators = _validatorRepository.FindAll();

        validators.Count.Should().Be(1);
        var validatorFound = validators[0];
        validatorFound.Id.Should().Be(validator.Id);
        validatorFound.Name.Should().Be(validator.Name);
    }
    #endregion
}
