using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Domain;
using SmartHome.DataAccess.Contexts;
using Microsoft.Data.SqlClient;
using SmartHome.DataAccess.CustomExceptions;
using SmartHome.BusinessLogic.ExtraRepositoryInterfaces;
using SmartHome.BusinessLogic.CustomExceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SmartHome.DataAccess.Repositories;
public class HomeRepository : IGenericRepository<Home>, IHomesFromUserRepository
{
    private readonly SmartHomeEFCoreContext _context;

    public HomeRepository(SmartHomeEFCoreContext context)
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

    public Home Add(Home entity)
    {
        try
        {
            _context.Homes.Add(entity);
            _context.SaveChanges();
            return _context.Homes.FirstOrDefault(h => h.Id == entity.Id);
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
            Home homeToDelete = _context.Homes.FirstOrDefault(h => h.Id == id);
            if (homeToDelete != null)
            {
                _context.Homes.Remove(homeToDelete);
                _context.SaveChanges();
            }
            else
            {
                throw new DatabaseException("The Home does not exist in the Data Base.");
            }
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public Home? Find(Func<Home, bool> filter)
    {
        try
        {
            return _context.Homes.
                Include(x => x.Members).
                ThenInclude(x => x.User).
                Include(x => x.Members).
                ThenInclude(x => x.HomePermissions).
                Include(x => x.Devices).
                ThenInclude(x=>x.Device).
                ThenInclude(x=> x.Photos).
                Include(x => x.Rooms).
                ThenInclude(x => x.HomeDevices).
                FirstOrDefault(filter);
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<Home> FindAll()
    {
        try
        {
            return _context.Homes
                .Include(home => home.Members)
                    .ThenInclude(member => member.User)
                .Include(home => home.Members)
                    .ThenInclude(member => member.HomeMemberNotifications)
                        .ThenInclude(hmn => hmn.Notification)
                            .ThenInclude(notification => notification.HomeDevice)
                .ToList();
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<Home> FindAllFiltered(Expression<Func<Home, bool>> filter, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public IList<Home> FindAllFiltered(Expression<Func<Home, bool>> filter)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Home> GetAllHomesByUserId(Guid userId)
    {
        try
        {
            return _context.Homes
                        .Where(home => home.Members.Any(member => member.User != null && member.User.Id == userId))
                        .ToList();
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public Home? Update(Home updatedEntity)
    {
        try
        {
            _context.Update(updatedEntity);
            _context.SaveChanges();
            return updatedEntity;
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }
}
