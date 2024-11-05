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

public sealed class UserRepository : IGenericRepository<User>
{
    private readonly SmartHomeEFCoreContext _repository;
    public UserRepository(SmartHomeEFCoreContext repository)
    {
        _repository = repository;
    }

    public User Add(User user)
    {
        try
        {
            _repository.Users.Add(user);
            _repository.SaveChanges();
            return _repository.Users.FirstOrDefault(u => u.Id == user.Id);
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
            User userToDelete = _repository.Users.FirstOrDefault(b => b.Id == id);
            if (userToDelete != null)
            {
                _repository.Users.Remove(userToDelete);
                _repository.SaveChanges();
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
            return _repository.Users.Include(u => u.Role).FirstOrDefault(filter);
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
            return _repository.Users.ToList();
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<User> FindAllFiltered(Expression<Func<User, bool>> function, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public IList<User> FindAllFiltered(Expression<Func<User, bool>> filter)
    {
        throw new NotImplementedException();
    }

    public User? Update(User updatedEntity)
    {
        try
        {
            User foundUser = _repository.Users.FirstOrDefault(u => u.Id == updatedEntity.Id);

            if (foundUser != null)
            {
                _repository.Entry(foundUser).CurrentValues.SetValues(updatedEntity);
                _repository.SaveChanges();
                return _repository.Users.FirstOrDefault(u => u.Id == updatedEntity.Id);
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
