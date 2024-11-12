using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.CustomExceptions;

namespace SmartHome.BusinessLogic.LoadAssembly;
public sealed class LoadAssemblyClass<IInterface>
        where IInterface : class
{
    private readonly DirectoryInfo _directory;
    private readonly List<Type> _implementations;

    public LoadAssemblyClass(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ValidatorException("Path cannot be empty");
        }

        _directory = new DirectoryInfo(path);

        if (!_directory.Exists)
        {
            throw new ValidatorException($"The directory dont exists: {path}");
        }

        _implementations = new List<Type>();
    }

    public List<string> GetImplementations()
    {
        var files = _directory.GetFiles("*.dll").ToList();

        foreach (var file in files)
        {
            try
            {
                var assemblyLoaded = Assembly.LoadFile(file.FullName);
                var loadedTypes = assemblyLoaded
                    .GetTypes()
                    .Where(t => t.IsClass && typeof(IInterface).IsAssignableFrom(t))
                    .ToList();

                _implementations.AddRange(loadedTypes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in the assembly {file.FullName}: {ex.Message}");
            }
        }

        var distinctImplementations = _implementations
            .GroupBy(t => t.FullName)
            .Select(g => g.First())
            .ToList();

        _implementations.Clear();
        _implementations.AddRange(distinctImplementations);

        return _implementations.ConvertAll(t => t.Name);
    }

    public IInterface GetImplementation(string name, params object[] args)
    {
        var type = _implementations.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (type == null)
        {
            throw new ValidatorException($"There is no implementation with this name: {name}");
        }

        return Activator.CreateInstance(type, args) as IInterface;
    }
}

