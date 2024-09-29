using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;

namespace SmartHome.DataAccess.Repositories;

public sealed class UserRepository : IGenericRepository<User>
{
    public User Add(User user)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public User? Find(Func<User, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IList<User> FindAll()
    {
        throw new NotImplementedException();
    }

    public User? Update(User updatedEntity)
    {
        throw new NotImplementedException();
    }
}
