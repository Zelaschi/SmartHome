using System.Reflection;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;

namespace SmartHome.BusinessLogic.Services;

public sealed class ValidatorService
{
    private readonly IGenericRepository<Validator> _validatorRepository;
    private readonly string _validatorsFilesPath = @"..\SmartHome.BusinessLogic\DeviceImporter\ModelValidators";
    private readonly DirectoryInfo _directory;
    public ValidatorService(IGenericRepository<Validator> validatorRepository)
    {
        _validatorRepository = validatorRepository;
        _directory = new DirectoryInfo(_validatorsFilesPath);
    }

    public List<string> LoadAssemblyAndGetImplementations<IInterface>()
        where IInterface : class
    {
        var implementations = new List<Type>();

        var files = _directory.GetFiles("*.dll").ToList();

        files.ForEach(file =>
        {
            var assemblyLoaded = Assembly.LoadFile(file.FullName);

            var loadedTypes = assemblyLoaded
                .GetTypes()
                .Where(t => t.IsClass && typeof(IInterface).IsAssignableFrom(t))
                .ToList();

            implementations = implementations.Union(loadedTypes).ToList();

            if (_validatorRepository.Find(filter: x => x.Name == file.Name) != null)
            {
                var validator = new Validator
                {
                    Id = Guid.NewGuid(),
                    Name = file.Name
                };
                _validatorRepository.Add(validator);
            }
        });

        return implementations.ConvertAll(t => t.Name);
    }

    public void GetAllValidators()
    {
        _validatorRepository.FindAll();
    }
}
