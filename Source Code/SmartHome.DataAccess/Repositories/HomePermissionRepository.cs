using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;

namespace SmartHome.DataAccess.Repositories;
public class HomePermissionRepository : IGenericRepository<HomePermission>
{
    public readonly SmartHomeEFCoreContext _context;
    public HomePermissionRepository(SmartHomeEFCoreContext context)
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

    public HomePermission Add(HomePermission entity)
    {
        try
        {
            _context.HomePermissions.Add(entity);
            _context.SaveChanges();
            return _context.HomePermissions.FirstOrDefault(b => b.Id == entity.Id);
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
            HomePermission homePermissionToDelete = _context.HomePermissions.FirstOrDefault(p => p.Id == id);
            if (homePermissionToDelete != null)
            {
                _context.HomePermissions.Remove(homePermissionToDelete);
                _context.SaveChanges();
            }
            else
            {
                throw new DatabaseException("The HomePermission does not exist in the Data Base.");
            }
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public HomePermission? Find(Func<HomePermission, bool> filter)
    {
        try
        {
            return _context.HomePermissions.FirstOrDefault(filter);
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<HomePermission> FindAll()
    {
        try
        {
            return _context.HomePermissions.ToList();
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<HomePermission> FindAllFiltered(Expression<Func<HomePermission, bool>> filter, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public IList<HomePermission> FindAllFiltered(Expression<Func<HomePermission, bool>> filter)
    {
        throw new NotImplementedException();
    }

    public HomePermission? Update(HomePermission updatedEntity)
    {
        try
        {
            HomePermission foundHomePermission = _context.HomePermissions.FirstOrDefault(p => p.Id == updatedEntity.Id);

            if (foundHomePermission != null)
            {
                _context.Entry(foundHomePermission).CurrentValues.SetValues(updatedEntity);
                _context.SaveChanges();
                return _context.HomePermissions.FirstOrDefault(p => p.Id == updatedEntity.Id);
            }
            else
            {
                throw new DatabaseException("The HomePermission does not exist in the Data Base.");
            }
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }
}
