using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;

namespace SmartHome.DataAccess.Repositories;
public sealed class BusinessRepository : IGenericRepository<Business>
{
    private readonly SmartHomeEFCoreContext _context;
    public BusinessRepository(SmartHomeEFCoreContext context)
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

    public Business Add(Business entity)
    {
        try
        {
            _context.Businesses.Add(entity);
            _context.SaveChanges();
            return _context.Businesses.FirstOrDefault(b => b.Id == entity.Id);
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
            Business businessToDelete = _context.Businesses.FirstOrDefault(b => b.Id == id);
            if (businessToDelete != null)
            {
                _context.Businesses.Remove(businessToDelete);
                _context.SaveChanges();
            }
            else
            {
                throw new DatabaseException("The Business does not exist in the Data Base.");
            }
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public Business? Find(Func<Business, bool> filter)
    {
        try
        {
            return _context.Businesses.FirstOrDefault(filter);
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<Business> FindAll()
    {
        try
        {
            return _context.Businesses.ToList();
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<Business> FindAllFiltered(Expression<Func<Business, bool>> filter, int pageNumber, int pageSize)
    {
        var query = _context.Businesses.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public IList<Business> FindAllFiltered(Expression<Func<Business, bool>> filter)
    {
        var query = _context.Businesses.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return query.ToList();
    }

    public Business? Update(Business updatedEntity)
    {
        try
        {
            Business foundBusiness = _context.Businesses.FirstOrDefault(b => b.Id == updatedEntity.Id);

            if (foundBusiness != null)
            {
                _context.Entry(foundBusiness).CurrentValues.SetValues(updatedEntity);
                _context.SaveChanges();
                return _context.Businesses.FirstOrDefault(b => b.Id == updatedEntity.Id);
            }
            else
            {
                throw new DatabaseException("The Business does not exist in the Data Base.");
            }
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }
}
