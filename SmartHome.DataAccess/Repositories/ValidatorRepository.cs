using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;

namespace SmartHome.DataAccess.Repositories;
public sealed class ValidatorRepository : IGenericRepository<Validator>
{
    private readonly SmartHomeEFCoreContext _context;

    public ValidatorRepository(SmartHomeEFCoreContext context)
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

    public Validator Add(Validator entity)
    {
        try
        {
            _context.Validators.Add(entity);
            _context.SaveChanges();
            return _context.Validators.FirstOrDefault(v => v.Id == entity.Id);
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
            Validator validatorToDelete = _context.Validators.FirstOrDefault(v => v.Id == id);
            if (validatorToDelete != null)
            {
                _context.Validators.Remove(validatorToDelete);
                _context.SaveChanges();
            }
            else
            {
                throw new DatabaseException("The Validator does not exist in the Data Base.");
            }
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public Validator? Find(Func<Validator, bool> filter)
    {
        try
        {
            return _context.Validators.FirstOrDefault(filter);
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<Validator> FindAll()
    {
        try
        {
            return _context.Validators.ToList();
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<Validator> FindAllFiltered(Expression<Func<Validator, bool>> filter, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public IList<Validator> FindAllFiltered(Expression<Func<Validator, bool>> filter)
    {
        throw new NotImplementedException();
    }

    public Validator? Update(Validator updatedEntity)
    {
        try
        {
            Validator validatorToUpdate = _context.Validators.FirstOrDefault(v => v.Id == updatedEntity.Id);
            if (validatorToUpdate != null)
            {
                validatorToUpdate.Name = updatedEntity.Name;
                _context.SaveChanges();
                return _context.Validators.FirstOrDefault(v => v.Id == updatedEntity.Id);
            }
            else
            {
                throw new DatabaseException("The Validator does not exist in the Data Base.");
            }
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }
}
