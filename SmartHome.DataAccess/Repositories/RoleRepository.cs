using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;

namespace SmartHome.DataAccess.Repositories;
public sealed class RoleRepository : IGenericRepository<Role>
{
    public Role Add(Role entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Role? Find(Func<Role, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IList<Role> FindAll()
    {
        throw new NotImplementedException();
    }

    public Role? Update(Role updatedEntity)
    {
        throw new NotImplementedException();
    }
}
