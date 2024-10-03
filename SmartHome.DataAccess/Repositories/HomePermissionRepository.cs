using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;

namespace SmartHome.DataAccess.Repositories;
public class HomePermissionRepository : IGenericRepository<HomePermission>
{
    public HomePermission Add(HomePermission entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public HomePermission? Find(Func<HomePermission, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IList<HomePermission> FindAll()
    {
        throw new NotImplementedException();
    }

    public HomePermission? Update(HomePermission updatedEntity)
    {
        throw new NotImplementedException();
    }
}
