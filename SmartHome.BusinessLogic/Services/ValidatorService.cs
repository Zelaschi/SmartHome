using ModeloValidador.Abstracciones;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.LoadAssembly;

namespace SmartHome.BusinessLogic.Services;

public sealed class ValidatorService : IValidatorLogic
{
    private readonly IGenericRepository<Validator> _validatorRepository;
    private readonly string _validatorsFilesPath = @"..\SmartHome.BusinessLogic\DeviceImporter\ModelValidators";
    private readonly LoadAssembly<IModeloValidador> _assemblyLoader;
    private List<Type> _implementations;

    public ValidatorService(IGenericRepository<Validator> validatorRepository)
    {
        _validatorRepository = validatorRepository;
        _assemblyLoader = new LoadAssembly<IModeloValidador>(_validatorsFilesPath);
        _implementations = new List<Type>();
    }

    public List<string> LoadAssembliesAndGetImplementations()
    {
        var implementationNames = _assemblyLoader.GetImplementations();

        foreach (var name in implementationNames)
        {
            if (_validatorRepository.Find(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) == null)
            {
                var validator = new Validator
                {
                    Id = Guid.NewGuid(),
                    Name = name
                };
                _validatorRepository.Add(validator);
            }
        }

        _implementations = _assemblyLoader
            .GetImplementations()
            .Select(name => _assemblyLoader.GetImplementation(name))
            .Select(instance => instance.GetType())
            .ToList();

        return implementationNames;
    }

    public IModeloValidador GetImplementation(string name, params object[] args)
    {
        if (_implementations == null || !_implementations.Any())
        {
            LoadAssembliesAndGetImplementations();
        }

        var validator = _assemblyLoader.GetImplementation(name, args);

        if (validator == null)
        {
            throw new ValidatorException($"There is no implementation with this name: {name}");
        }

        return validator;
    }

    public bool IsValidModelNumber(string modelNumber, Guid? validatorId)
    {
        var validatorName = _validatorRepository.Find(x => x.Id == validatorId).Name;
        var validator = GetImplementation(validatorName);

        if (validator == null)
        {
            throw new ValidatorException("Validator does not exists");
        }

        var modelo = new Modelo
        {
            Value = modelNumber
        };

        return validator.EsValido(modelo);
    }

    public List<string> GetAllValidators()
    {
        return _assemblyLoader.GetImplementations();
    }
}
