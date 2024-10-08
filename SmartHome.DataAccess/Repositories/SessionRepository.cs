using Microsoft.Data.SqlClient;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;

namespace SmartHome.DataAccess.Repositories;
public class SessionRepository : IGenericRepository<Session>
{
    private readonly SmartHomeEFCoreContext _repository;
    public SessionRepository(SmartHomeEFCoreContext repository)
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

    public Session Add(Session entity)
    {
        try
        {
            _repository.Sessions.Add(entity);
            _repository.SaveChanges();
            return _repository.Sessions.FirstOrDefault(s => s.SessionId == entity.SessionId);
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
            Session sessionToDelete = _repository.Sessions.FirstOrDefault(s => s.SessionId == id);
            if (sessionToDelete != null)
            {
                _repository.Sessions.Remove(sessionToDelete);
                _repository.SaveChanges();
            }
            else
            {
                throw new DatabaseException("The Session does not exist in the Data Base.");
            }
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public Session? Find(Func<Session, bool> filter)
    {
        try
        {
            return _repository.Sessions.FirstOrDefault(filter);
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<Session> FindAll()
    {
        try
        {
            return _repository.Sessions.ToList();
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public Session? Update(Session updatedEntity)
    {
        try
        {
            Session foundSession = _repository.Sessions.FirstOrDefault(s => s.SessionId == updatedEntity.SessionId);

            if (foundSession != null)
            {
                _repository.Entry(foundSession).CurrentValues.SetValues(updatedEntity);
                _repository.SaveChanges();
                return _repository.Sessions.FirstOrDefault(s => s.SessionId == updatedEntity.SessionId);
            }
            else
            {
                throw new DatabaseException("The Session does not exist in the Data Base.");
            }
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }
}
