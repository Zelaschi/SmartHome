using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.GenericRepositoryInterface;

public interface IGenericRepository<T>
{
    T Add(T entity);
    T? Update(T updatedEntity);
    void Delete(Guid id);
    T? Find(Func<T, bool> filter);
    IList<T> FindAll();
}
