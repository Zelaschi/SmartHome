using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;

namespace SmartHome.DataAccess.Repositories;
public class SessionRepository : IGenericRepository<Session>
{
    public Session Add(Session entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Session? Find(Func<Session, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IList<Session> FindAll()
    {
        throw new NotImplementedException();
    }

    public Session? Update(Session updatedEntity)
    {
        throw new NotImplementedException();
    }
}
