using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;

namespace SmartHome.DataAccess.Repositories;
public sealed class HomeDeviceRepository : IGenericRepository<HomeDevice>
{
    public readonly SmartHomeEFCoreContext _repository;

    public HomeDeviceRepository(SmartHomeEFCoreContext repository)
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

    public HomeDevice Add(HomeDevice entity)
    {
        try
        {
            _repository.HomeDevices.Add(entity);
            _repository.SaveChanges();
            return _repository.HomeDevices.FirstOrDefault(b => b.Id == entity.Id);
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
            HomeDevice homeDeviceToDelete = _repository.HomeDevices.FirstOrDefault(b => b.Id == id);
            if (homeDeviceToDelete != null)
            {
                _repository.HomeDevices.Remove(homeDeviceToDelete);
                _repository.SaveChanges();
            }
            else
            {
                throw new DatabaseException("The Home Device does not exist in the Data Base.");
            }
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public HomeDevice? Find(Func<HomeDevice, bool> filter)
    {
        try
        {
            return _repository.HomeDevices.Include(homeDevice => homeDevice.Device).FirstOrDefault(filter);
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<HomeDevice> FindAll()
    {
        try
        {
            return _repository.HomeDevices.ToList();
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<HomeDevice> FindAllFiltered(Expression<Func<HomeDevice, bool>> filter, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public IList<HomeDevice> FindAllFiltered(Expression<Func<HomeDevice, bool>> filter)
    {
        throw new NotImplementedException();
    }

    public HomeDevice? Update(HomeDevice updatedEntity)
    {
        try
        {
            HomeDevice foundHomeDevice = _repository.HomeDevices.FirstOrDefault(b => b.Id == updatedEntity.Id);

            if (foundHomeDevice != null)
            {
                _repository.Entry(foundHomeDevice).CurrentValues.SetValues(updatedEntity);
                _repository.SaveChanges();
                return _repository.HomeDevices.FirstOrDefault(b => b.Id == updatedEntity.Id);
            }
            else
            {
                throw new DatabaseException("The Home Device does not exist in the Data Base.");
            }
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }
}
