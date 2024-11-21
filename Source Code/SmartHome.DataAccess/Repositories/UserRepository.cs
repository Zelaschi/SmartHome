using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;

namespace SmartHome.DataAccess.Repositories;

public sealed class UserRepository : IGenericRepository<User>
{
    private readonly SmartHomeEFCoreContext _context;
    public UserRepository(SmartHomeEFCoreContext context)
    {
        _context = context;
    }

    public User Add(User user)
    {
        try
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return _context.Users.FirstOrDefault(u => u.Id == user.Id);
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
            User userToDelete = _context.Users.FirstOrDefault(b => b.Id == id);
            if (userToDelete != null)
            {
                _context.Users.Remove(userToDelete);
                _context.SaveChanges();
            }
            else
            {
                throw new DatabaseException("The User does not exist in the Data Base.");
            }
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public User? Find(Func<User, bool> filter)
    {
        try
        {
            return _context.Users.Include(u => u.Role).FirstOrDefault(filter);
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<User> FindAll()
    {
        try
        {
            return _context.Users.Include(u => u.Role.SystemPermissions).ToList();
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<User> FindAllFiltered(Expression<Func<User, bool>> filter, int pageNumber, int pageSize)
    {
        var query = _context.Users.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public IList<User> FindAllFiltered(Expression<Func<User, bool>> filter)
    {
        var query = _context.Users.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return query.ToList();
    }

    public User? Update(User updatedEntity)
    {
        try
        {
            User foundUser = _context.Users.FirstOrDefault(u => u.Id == updatedEntity.Id);

            if (foundUser != null)
            {
                _context.Entry(foundUser).CurrentValues.SetValues(updatedEntity);
                _context.SaveChanges();
                return _context.Users.FirstOrDefault(u => u.Id == updatedEntity.Id);
            }
            else
            {
                throw new DatabaseException("The User does not exist in the Data Base.");
            }
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }
}
