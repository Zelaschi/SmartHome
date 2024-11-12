using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.GenericRepositoryInterface;
public interface IUpdateMultipleElementsRepository<T>
{
    IList<T>? UpdateMultiplElements(List<T> updatedEntity);
}
