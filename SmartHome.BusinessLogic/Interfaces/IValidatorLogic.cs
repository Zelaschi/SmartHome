using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModeloValidador.Abstracciones;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IValidatorLogic
{
    List<string> LoadAssembliesAndGetImplementations();
    IModeloValidador GetImplementation(string name, params object[] args);
    bool IsValidModelNumber(string modelNumber, Guid? validatorId);
    List<string> GetAllValidators();
}
