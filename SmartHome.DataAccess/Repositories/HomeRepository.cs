using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.DataAccess.Repositories;
public class HomeRepository : IGenericRepository<Home>
{
    public Home Add(Home entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Home? Find(Func<Home, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IList<Home> FindAll()
    {
        throw new NotImplementedException();
    }

    public Home? Update(Home updatedEntity)
    {
        throw new NotImplementedException();
    }
}
