using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;

namespace SmartHome.DataAccess.Repositories;

public class SystemPermissionRepository : IGenericRepository<SystemPermission>
{
    private readonly SmartHomeEFCoreContext _repository;

    public SystemPermissionRepository(SmartHomeEFCoreContext repository)
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

    public SystemPermission Add(SystemPermission entity)
    {
        try
        {
            _repository.SystemPermissions.Add(entity);
            _repository.SaveChanges();
            return _repository.SystemPermissions.FirstOrDefault(sp => sp.Id == entity.Id);
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
            SystemPermission permissionToDelete = _repository.SystemPermissions.FirstOrDefault(sp => sp.Id == id);
            if (permissionToDelete != null)
            {
                _repository.SystemPermissions.Remove(permissionToDelete);
                _repository.SaveChanges();
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
            return _repository.SystemPermissions.FirstOrDefault(filter);
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
            return _repository.SystemPermissions.ToList();
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public SystemPermission? Update(SystemPermission updatedEntity)
    {
        try
        {
            SystemPermission foundPermission = _repository.SystemPermissions.FirstOrDefault(sp => sp.Id == updatedEntity.Id);

            if (foundPermission != null)
            {
                _repository.Entry(foundPermission).CurrentValues.SetValues(updatedEntity);
                _repository.SaveChanges();
                return _repository.SystemPermissions.FirstOrDefault(sp => sp.Id == updatedEntity.Id);
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
