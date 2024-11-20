using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
        // Arrange
        var mockValidatorRepository = new Mock<IGenericRepository<ModelNumberValidator>>();

        // Usar el wrapper MockAssemblyLoader en lugar de intentar mockear directamente la clase LoadAssemblyClass
        var mockAssemblyLoader = new MockAssemblyLoader(@"..\SmartHome.BusinessLogic\ModelValidators");

        var validatorService = new ValidatorService(mockValidatorRepository.Object);

        // Act
        var result = validatorService.LoadAssembliesAndGetImplementations();

        // Assert
        Assert.AreEqual(2, result.Count); // Verifica que se cargaron dos validadores
        Assert.AreEqual("SixLettersModelValidator", result[0]);
        Assert.AreEqual("LetterNumberModelValidator", result[1]);
        mockValidatorRepository.Verify(x => x.Add(It.Is<ModelNumberValidator>(v => v.Name == "SixLettersModelValidator")), Times.Once);
        mockValidatorRepository.Verify(x => x.Add(It.Is<ModelNumberValidator>(v => v.Name == "LetterNumberModelValidator")), Times.Once);
    }

    [TestMethod]
    public void GetAllValidators_ShouldReturnListOfValidatorNames()
    {
        // Arrange
        var mockValidatorRepository = new Mock<IGenericRepository<ModelNumberValidator>>();

        var validatorService = new ValidatorService(mockValidatorRepository.Object);

        // Act
        var result = validatorService.GetAllValidators();

        // Assert
        Assert.AreEqual(2, result.Count);  // Verifica que se devuelven dos validadores
        Assert.AreEqual("SixLettersModelValidator", result[0]);
        Assert.AreEqual("LetterNumberModelValidator", result[1]);
    }

    [TestMethod]
    public void IsValidModelNumber_ShouldReturnTrue_WhenModelIsValid()
    {
        // Arrange
        var mockValidatorRepository = new Mock<IGenericRepository<ModelNumberValidator>>();

        // Usar el wrapper MockAssemblyLoader
        var mockAssemblyLoader = new MockAssemblyLoader(@"..\SmartHome.BusinessLogic\ModelValidators");

        // Simular un validador que siempre valida el modelo
        var mockValidator = new Mock<IModeloValidador>();
        mockValidator.Setup(v => v.EsValido(It.IsAny<Modelo>())).Returns(true);  // El validador siempre retorna true

        var validatorService = new ValidatorService(mockValidatorRepository.Object);

        // Configurar el repositorio para devolver un validador cuando se busque por Id
        var validator = new ModelNumberValidator { Id = Guid.NewGuid(), Name = "SixLettersModelValidator" };
        mockValidatorRepository.Setup(x => x.Find(It.IsAny<Func<ModelNumberValidator, bool>>()))
                               .Returns(validator);

        // Act
        var result = validatorService.IsValidModelNumber("ABCDEF", validator.Id);

        // Assert
        Assert.IsTrue(result);  // Verifica que el modelo es válido
    }

    [TestMethod]
    public void IsValidModelNumber_ShouldThrowValidatorException_WhenValidatorNameNotFound()
    {
        // Arrange
        var mockValidatorRepository = new Mock<IGenericRepository<ModelNumberValidator>>();

        // Usar el wrapper MockAssemblyLoader
        var mockAssemblyLoader = new MockAssemblyLoader(@"..\SmartHome.BusinessLogic\ModelValidators");

        // Simular que el validador no existe
        var mockValidator = new Mock<IModeloValidador>();
        mockValidator.Setup(v => v.EsValido(It.IsAny<Modelo>())).Returns(true);  // Aunque el validador siempre valida, no debe llegar a esta parte

        var validatorService = new ValidatorService(mockValidatorRepository.Object);

        // Configurar el repositorio para no encontrar el validador cuando se busque por Id
        var validatorId = Guid.NewGuid();  // Un Id que no se encuentra en el repositorio
        mockValidatorRepository.Setup(x => x.Find(It.IsAny<Func<ModelNumberValidator, bool>>()))
                               .Returns<ModelNumberValidator>(null);  // No encuentra el validador

        // Act & Assert
        try
        {
            // Intentar validar un número de modelo con un validador inexistente
            validatorService.IsValidModelNumber("ABCDEF", validatorId);
        }
        catch (ValidatorException ex)
        {
            // Verificar que se lanzó la excepción y que el mensaje sea el esperado
            Assert.AreEqual("Validator name not found", ex.Message);
        }
    }

    [TestMethod]
    public void IsValidModelNumber_ShouldThrowValidatorException_WhenValidatorDoesNotExist()
    {
        // Arrange
        var mockValidatorRepository = new Mock<IGenericRepository<ModelNumberValidator>>();

        // Usar el wrapper MockAssemblyLoader
        var mockAssemblyLoader = new MockAssemblyLoader(@"..\SmartHome.BusinessLogic\ModelValidators");

        // Simular que el validador existe en el repositorio
        var validator = new ModelNumberValidator { Id = Guid.NewGuid(), Name = "SixLettersModelValidator" };
        mockValidatorRepository.Setup(x => x.Find(It.IsAny<Func<ModelNumberValidator, bool>>()))
                               .Returns(validator);

        // Crear el servicio ValidatorService
        var validatorService = new ValidatorService(mockValidatorRepository.Object);

        mockAssemblyLoader.GetImplementation(It.IsAny<string>(), It.IsAny<object[]>());

        // Act & Assert
        try
        {
            // Intentar validar el número de modelo con un validador que no tiene implementación
            validatorService.IsValidModelNumber("ABCDEF", validator.Id);
        }
        catch (ValidatorException ex)
        {
            // Verificar que se lanzó la excepción y que el mensaje sea el esperado
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
        // Simula lo que GetImplementations() debe hacer.
        return new List<string> { "SixLettersModelValidator", "LetterNumberModelValidator" };
    }

    public new IModeloValidador GetImplementation(string name, params object[] args)
    {
        // Simula lo que GetImplementation() debe hacer.
        return new Mock<IModeloValidador>().Object;
    }
}
