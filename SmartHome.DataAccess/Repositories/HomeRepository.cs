﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Domain;
using SmartHome.DataAccess.Contexts;
using Microsoft.Data.SqlClient;
using SmartHome.DataAccess.CustomExceptions;

namespace SmartHome.DataAccess.Repositories;
public class HomeRepository : IGenericRepository<Home>
{
    private readonly SmartHomeEFCoreContext _repository;

    public HomeRepository(SmartHomeEFCoreContext repository)
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

    public Home Add(Home entity)
    {
        try
        {
            _repository.Homes.Add(entity);
            _repository.SaveChanges();
            return _repository.Homes.FirstOrDefault(h => h.Id == entity.Id);
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
            Home homeToDelete = _repository.Homes.FirstOrDefault(h => h.Id == id);
            if (homeToDelete != null)
            {
                _repository.Homes.Remove(homeToDelete);
                _repository.SaveChanges();
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
            return _repository.Homes.FirstOrDefault(filter);
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
            return _repository.Homes.ToList();
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
            Home foundHome = _repository.Homes.FirstOrDefault(h => h.Id == updatedEntity.Id);

            if (foundHome != null)
            {
                _repository.Entry(foundHome).CurrentValues.SetValues(updatedEntity);
                _repository.SaveChanges();
                return _repository.Homes.FirstOrDefault(b => b.Id == updatedEntity.Id);
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
}
