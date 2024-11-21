using ModeloValidador.Abstracciones;
using Moq;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.LoadAssembly;
using SmartHome.BusinessLogic.Services;

namespace SmartHome.BusinessLogic.Test;

[TestClass]
public class ValidatorServiceTest
{
    private Mock<IGenericRepository<ModelNumberValidator>>? validatorRepositoryMock;
    private ValidatorService? validatorService;

    [TestInitialize]

    public void Initialize()
    {
        validatorRepositoryMock = new Mock<IGenericRepository<ModelNumberValidator>>();
        validatorService = new ValidatorService(validatorRepositoryMock.Object);
    }

    [TestMethod]
    public void LoadAssembliesAndGetImplementations_ShouldLoadAndAddValidators()
    {
        var mockValidatorRepository = new Mock<IGenericRepository<ModelNumberValidator>>();

        var mockAssemblyLoader = new MockAssemblyLoader(@"..\SmartHome.BusinessLogic\ModelValidators");

        var validatorService = new ValidatorService(mockValidatorRepository.Object);

        var result = validatorService.LoadAssembliesAndGetImplementations();

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("SixLettersModelValidator", result[0]);
        Assert.AreEqual("LetterNumberModelValidator", result[1]);
        mockValidatorRepository.Verify(x => x.Add(It.Is<ModelNumberValidator>(v => v.Name == "SixLettersModelValidator")), Times.Once);
        mockValidatorRepository.Verify(x => x.Add(It.Is<ModelNumberValidator>(v => v.Name == "LetterNumberModelValidator")), Times.Once);
    }

    [TestMethod]
    public void GetAllValidators_ShouldReturnListOfValidatorNames()
    {
        var mockValidatorRepository = new Mock<IGenericRepository<ModelNumberValidator>>();

        var validatorService = new ValidatorService(mockValidatorRepository.Object);

        var result = validatorService.GetAllValidators();

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("SixLettersModelValidator", result[0]);
        Assert.AreEqual("LetterNumberModelValidator", result[1]);
    }

    [TestMethod]
    public void IsValidModelNumber_ShouldReturnTrue_WhenModelIsValid()
    {
        var mockValidatorRepository = new Mock<IGenericRepository<ModelNumberValidator>>();

        var mockAssemblyLoader = new MockAssemblyLoader(@"..\SmartHome.BusinessLogic\ModelValidators");

        var mockValidator = new Mock<IModeloValidador>();
        mockValidator.Setup(v => v.EsValido(It.IsAny<Modelo>()))
            .Returns(true);

        var validatorService = new ValidatorService(mockValidatorRepository.Object);

        var validator = new ModelNumberValidator { Id = Guid.NewGuid(), Name = "SixLettersModelValidator" };
        mockValidatorRepository.Setup(x => x.Find(It.IsAny<Func<ModelNumberValidator, bool>>()))
                               .Returns(validator);

        var result = validatorService.IsValidModelNumber("ABCDEF", validator.Id);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsValidModelNumber_ShouldThrowValidatorException_WhenValidatorNameNotFound()
    {
        var mockValidatorRepository = new Mock<IGenericRepository<ModelNumberValidator>>();

        var mockAssemblyLoader = new MockAssemblyLoader(@"..\SmartHome.BusinessLogic\ModelValidators");

        var mockValidator = new Mock<IModeloValidador>();
        mockValidator.Setup(v => v.EsValido(It.IsAny<Modelo>()))
            .Returns(true);

        var validatorService = new ValidatorService(mockValidatorRepository.Object);

        var validatorId = Guid.NewGuid();

        mockValidatorRepository.Setup(x => x.Find(It.IsAny<Func<ModelNumberValidator, bool>>()))
                               .Returns<ModelNumberValidator>(null);

        try
        {
            validatorService.IsValidModelNumber("ABCDEF", validatorId);
        }
        catch (ValidatorException ex)
        {
            Assert.AreEqual("Validator name not found", ex.Message);
        }
    }

    [TestMethod]
    public void IsValidModelNumber_ShouldThrowValidatorException_WhenValidatorDoesNotExist()
    {
        var mockValidatorRepository = new Mock<IGenericRepository<ModelNumberValidator>>();

        var mockAssemblyLoader = new MockAssemblyLoader(@"..\SmartHome.BusinessLogic\ModelValidators");

        var validator = new ModelNumberValidator { Id = Guid.NewGuid(), Name = "SixLettersModelValidator" };
        mockValidatorRepository.Setup(x => x.Find(It.IsAny<Func<ModelNumberValidator, bool>>()))
                               .Returns(validator);

        var validatorService = new ValidatorService(mockValidatorRepository.Object);

        mockAssemblyLoader.GetImplementation(It.IsAny<string>(), It.IsAny<object[]>());

        try
        {
            validatorService.IsValidModelNumber("ABCDEF", validator.Id);
        }
        catch (ValidatorException ex)
        {
            Assert.AreEqual("Validator does not exists", ex.Message);
        }
    }

    [TestMethod]
    public void GetImplementation_ShouldThrowValidatorException_NoImplementationWithThisName()
    {
        var mockValidatorRepository = new Mock<IGenericRepository<ModelNumberValidator>>();

        var mockAssemblyLoader = new MockAssemblyLoader(@"..\SmartHome.BusinessLogic\ModelValidators");

        var validator = new ModelNumberValidator { Id = Guid.NewGuid(), Name = "Test" };
        mockValidatorRepository.Setup(x => x.Find(It.IsAny<Func<ModelNumberValidator, bool>>()))
                               .Returns(validator);

        var validatorService = new ValidatorService(mockValidatorRepository.Object);

        mockAssemblyLoader.GetImplementation(It.IsAny<string>(), It.IsAny<object[]>());

        try
        {
            validatorService.GetImplementation("Test", validator.Id);
        }
        catch (ValidatorException ex)
        {
            Assert.AreEqual($"There is no implementation with this name: {validator.Name}", ex.Message);
        }
    }
}

internal class MockAssemblyLoader : LoadAssemblyClass<IModeloValidador>
{
    public MockAssemblyLoader(string path)
        : base(path) { }

    public new List<string> GetImplementations()
    {
        return new List<string> { "SixLettersModelValidator", "LetterNumberModelValidator" };
    }

    public new IModeloValidador GetImplementation(string name, params object[] args)
    {
        return new Mock<IModeloValidador>().Object;
    }
}
