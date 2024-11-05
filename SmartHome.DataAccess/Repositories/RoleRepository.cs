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
public sealed class RoleRepository : IGenericRepository<Role>
{
    private readonly SmartHomeEFCoreContext _context;

    public RoleRepository(SmartHomeEFCoreContext context)
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

    public Role Add(Role entity)
    {
        try
        {
            _context.Roles.Add(entity);
            _context.SaveChanges();
            return _context.Roles.FirstOrDefault(r => r.Id == entity.Id);
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
            Role roleToDelete = _context.Roles.FirstOrDefault(r => r.Id == id);
            if (roleToDelete != null)
            {
                _context.Roles.Remove(roleToDelete);
                _context.SaveChanges();
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
            return _context.Roles.Include(r => r.SystemPermissions).FirstOrDefault(filter);
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
            return _context.Roles.ToList();
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<Role> FindAllFiltered(Expression<Func<Role, bool>> filter, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public IList<Role> FindAllFiltered(Expression<Func<Role, bool>> filter)
    {
        throw new NotImplementedException();
    }

    public Role? Update(Role updatedEntity)
    {
        try
        {
            Role foundRole = _context.Roles.FirstOrDefault(r => r.Id == updatedEntity.Id);

            if (foundRole != null)
            {
                _context.Entry(foundRole).CurrentValues.SetValues(updatedEntity);
                _context.SaveChanges();
                return _context.Roles.FirstOrDefault(r => r.Id == updatedEntity.Id);
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
