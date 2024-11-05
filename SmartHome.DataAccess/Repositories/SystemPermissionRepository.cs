using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;

namespace SmartHome.DataAccess.Repositories;

public class SystemPermissionRepository : IGenericRepository<SystemPermission>
{
    private readonly SmartHomeEFCoreContext _context;

    public SystemPermissionRepository(SmartHomeEFCoreContext context)
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

    public SystemPermission Add(SystemPermission entity)
    {
        try
        {
            _context.SystemPermissions.Add(entity);
            _context.SaveChanges();
            return _context.SystemPermissions.FirstOrDefault(sp => sp.Id == entity.Id);
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
            SystemPermission permissionToDelete = _context.SystemPermissions.FirstOrDefault(sp => sp.Id == id);
            if (permissionToDelete != null)
            {
                _context.SystemPermissions.Remove(permissionToDelete);
                _context.SaveChanges();
            }
            else
            {
                throw new DatabaseException("The SystemPermission does not exist in the Data Base.");
            }
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public SystemPermission? Find(Func<SystemPermission, bool> filter)
    {
        try
        {
            return _context.SystemPermissions.FirstOrDefault(filter);
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<SystemPermission> FindAll()
    {
        try
        {
            return _context.SystemPermissions.ToList();
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<SystemPermission> FindAllFiltered(Expression<Func<SystemPermission, bool>> filter, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public IList<SystemPermission> FindAllFiltered(Expression<Func<SystemPermission, bool>> filter)
    {
        throw new NotImplementedException();
    }

    public SystemPermission? Update(SystemPermission updatedEntity)
    {
        try
        {
            SystemPermission foundPermission = _context.SystemPermissions.FirstOrDefault(sp => sp.Id == updatedEntity.Id);

            if (foundPermission != null)
            {
                _context.Entry(foundPermission).CurrentValues.SetValues(updatedEntity);
                _context.SaveChanges();
                return _context.SystemPermissions.FirstOrDefault(sp => sp.Id == updatedEntity.Id);
            }
            else
            {
                throw new DatabaseException("The SystemPermission does not exist in the Data Base.");
            }
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }
}
