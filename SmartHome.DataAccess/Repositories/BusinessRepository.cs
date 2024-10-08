using Microsoft.Data.SqlClient;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;

namespace SmartHome.DataAccess.Repositories;
public sealed class BusinessRepository : IGenericRepository<Business>
{
    private readonly SmartHomeEFCoreContext _repository;
    public BusinessRepository(SmartHomeEFCoreContext repository)
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

    public Business Add(Business entity)
    {
        try
        {
            _repository.Businesses.Add(entity);
            _repository.SaveChanges();
            return _repository.Businesses.FirstOrDefault(b => b.Id == entity.Id);
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
            Business businessToDelete = _repository.Businesses.FirstOrDefault(b => b.Id == id);
            if (businessToDelete != null)
            {
                _repository.Businesses.Remove(businessToDelete);
                _repository.SaveChanges();
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
            return _repository.Businesses.FirstOrDefault(filter);
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
            return _repository.Businesses.ToList();
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public Business? Update(Business updatedEntity)
    {
        try
        {
            Business foundBusiness = _repository.Businesses.FirstOrDefault(b => b.Id == updatedEntity.Id);

            if (foundBusiness != null)
            {
                _repository.Entry(foundBusiness).CurrentValues.SetValues(updatedEntity);
                _repository.SaveChanges();
                return _repository.Businesses.FirstOrDefault(b => b.Id == updatedEntity.Id);
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
