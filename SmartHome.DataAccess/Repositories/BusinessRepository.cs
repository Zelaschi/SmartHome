using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;

namespace SmartHome.DataAccess.Repositories;
public sealed class BusinessRepository : IGenericRepository<Business>
{
    public Business Add(Business entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Business? Find(Func<Business, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IList<Business> FindAll()
    {
        throw new NotImplementedException();
    }

    public Business? Update(Business updatedEntity)
    {
        throw new NotImplementedException();
    }
}
