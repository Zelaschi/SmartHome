using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.ExtraRepositoryInterfaces;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;

namespace SmartHome.DataAccess.Repositories;
public class DeviceRepository : IGenericRepository<Device>
{
    private readonly SmartHomeEFCoreContext _repository;
    public DeviceRepository(SmartHomeEFCoreContext repository)
    {
        try
        {
            _repository = repository;
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public Device Add(Device entity)
    {
        try
        {
            _repository.Devices.Add(entity);
            _repository.SaveChanges();
            return _repository.Devices.FirstOrDefault(d => d.Id == entity.Id);
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public void Delete(Guid id)
    {
        try
        {
            Device deviceToDelete = _repository.Devices.FirstOrDefault(d => d.Id == id);
            if (deviceToDelete != null)
            {
                _repository.Devices.Remove(deviceToDelete);
                _repository.SaveChanges();
            }
            else
            {
                throw new DatabaseException("The Device does not exist in the Data Base.");
            }
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public Device? Find(Func<Device, bool> filter)
    {
        try
        {
            return _repository.Devices.Include(x => x.Business).FirstOrDefault(filter);
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<Device> FindAll()
    {
        try
        {
            return _repository.Devices.Include(x => x.Business).ToList();
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<Device> FindAllFiltered(Expression<Func<Device, bool>> filter, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public IList<Device> FindAllFiltered(Expression<Func<Device, bool>> filter)
    {
        throw new NotImplementedException();
    }

    public Device? Update(Device updatedEntity)
    {
        try
        {
            Device foundDevice = _repository.Devices.FirstOrDefault(b => b.Id == updatedEntity.Id);

            if (foundDevice != null)
            {
                _repository.Entry(foundDevice).CurrentValues.SetValues(updatedEntity);
                _repository.SaveChanges();
                return _repository.Devices.FirstOrDefault(b => b.Id == updatedEntity.Id);
            }
            else
            {
                throw new DatabaseException("The Business does not exist in the Data Base.");
            }
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }
}
