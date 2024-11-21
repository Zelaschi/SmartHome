using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;

namespace SmartHome.DataAccess.Repositories;
public sealed class HomeDeviceRepository : IGenericRepository<HomeDevice>
{
    public readonly SmartHomeEFCoreContext _context;

    public HomeDeviceRepository(SmartHomeEFCoreContext context)
    {
        try
        {
            _context = context;
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
            _context.HomeDevices.Add(entity);
            _context.SaveChanges();
            return _context.HomeDevices.FirstOrDefault(b => b.Id == entity.Id);
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
            HomeDevice homeDeviceToDelete = _context.HomeDevices.FirstOrDefault(b => b.Id == id);
            if (homeDeviceToDelete != null)
            {
                _context.HomeDevices.Remove(homeDeviceToDelete);
                _context.SaveChanges();
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
            return _context.HomeDevices.Include(homeDevice => homeDevice.Device).ThenInclude(x => x.Photos).FirstOrDefault(filter);
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
            return _context.HomeDevices.ToList();
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
            HomeDevice foundHomeDevice = _context.HomeDevices.FirstOrDefault(b => b.Id == updatedEntity.Id);

            if (foundHomeDevice != null)
            {
                _context.Entry(foundHomeDevice).CurrentValues.SetValues(updatedEntity);
                _context.SaveChanges();
                return _context.HomeDevices.FirstOrDefault(b => b.Id == updatedEntity.Id);
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
