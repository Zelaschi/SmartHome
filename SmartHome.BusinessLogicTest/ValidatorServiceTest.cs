using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModeloValidador.Abstracciones;
using Moq;
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

}

public class MockAssemblyLoader : LoadAssemblyClass<IModeloValidador>
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
