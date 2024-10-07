using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;

namespace SmartHome.DataAccess.Repositories;
public sealed class RoleRepository : IGenericRepository<Role>
{
    private readonly SmartHomeEFCoreContext _repository;

    public RoleRepository(SmartHomeEFCoreContext repository)
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

    public Role Add(Role entity)
    {
        try
        {
            _repository.Roles.Add(entity);
            _repository.SaveChanges();
            return _repository.Roles.FirstOrDefault(r => r.Id == entity.Id);
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
            Role roleToDelete = _repository.Roles.FirstOrDefault(r => r.Id == id);
            if (roleToDelete != null)
            {
                _repository.Roles.Remove(roleToDelete);
                _repository.SaveChanges();
            }
            else
            {
                throw new DatabaseException("The Role does not exist in the Data Base.");
            }
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public Role? Find(Func<Role, bool> filter)
    {
        try
        {
            return _repository.Roles.Include(r => r.SystemPermissions).FirstOrDefault(filter);
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<Role> FindAll()
    {
        try
        {
            return _repository.Roles.ToList();
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public Role? Update(Role updatedEntity)
    {
        try
        {
            Role foundRole = _repository.Roles.FirstOrDefault(r => r.Id == updatedEntity.Id);

            if (foundRole != null)
            {
                _repository.Entry(foundRole).CurrentValues.SetValues(updatedEntity);
                _repository.SaveChanges();
                return _repository.Roles.FirstOrDefault(r => r.Id == updatedEntity.Id);
            }
            else
            {
                throw new DatabaseException("The Role does not exist in the Data Base.");
            }
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }
}
