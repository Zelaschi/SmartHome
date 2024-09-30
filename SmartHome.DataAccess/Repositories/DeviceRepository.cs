using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;

namespace SmartHome.DataAccess.Repositories;
public class DeviceRepository : IGenericRepository<Device>
{
    public Device Add(Device entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Device? Find(Func<Device, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IList<Device> FindAll()
    {
        throw new NotImplementedException();
    }

    public Device? Update(Device updatedEntity)
    {
        throw new NotImplementedException();
    }
}
