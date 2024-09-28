using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IGenericRepository<T>
{
    T Add(T oneElement);

    T? Find(Func<T, bool> filter);

    IList<T> FindAll();

    T? Update(T updatedEntity);

    void Delete(int id);
}
