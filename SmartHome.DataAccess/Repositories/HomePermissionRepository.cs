using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;

namespace SmartHome.DataAccess.Repositories;
public class HomePermissionRepository : IGenericRepository<HomePermission>
{
    public readonly SmartHomeEFCoreContext _repository;
    public HomePermissionRepository(SmartHomeEFCoreContext repository)
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

    public HomePermission Add(HomePermission entity)
    {
        try
        {
            _repository.HomePermissions.Add(entity);
            _repository.SaveChanges();
            return _repository.HomePermissions.FirstOrDefault(b => b.Id == entity.Id);
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
            HomePermission homePermissionToDelete = _repository.HomePermissions.FirstOrDefault(p => p.Id == id);
            if (homePermissionToDelete != null)
            {
                _repository.HomePermissions.Remove(homePermissionToDelete);
                _repository.SaveChanges();
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
            return _repository.HomePermissions.FirstOrDefault(filter);
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
            return _repository.HomePermissions.ToList();
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public HomePermission? Update(HomePermission updatedEntity)
    {
        try
        {
            HomePermission foundHomePermission = _repository.HomePermissions.FirstOrDefault(p => p.Id == updatedEntity.Id);

            if (foundHomePermission != null)
            {
                _repository.Entry(foundHomePermission).CurrentValues.SetValues(updatedEntity);
                _repository.SaveChanges();
                return _repository.HomePermissions.FirstOrDefault(p => p.Id == updatedEntity.Id);
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
