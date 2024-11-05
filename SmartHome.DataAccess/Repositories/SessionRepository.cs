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
public class SessionRepository : IGenericRepository<Session>
{
    private readonly SmartHomeEFCoreContext _context;
    public SessionRepository(SmartHomeEFCoreContext context)
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

    public Session Add(Session entity)
    {
        try
        {
            _context.Sessions.Add(entity);
            _context.SaveChanges();
            return _context.Sessions.FirstOrDefault(s => s.SessionId == entity.SessionId);
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
            Session sessionToDelete = _context.Sessions.FirstOrDefault(s => s.SessionId == id);
            if (sessionToDelete != null)
            {
                _context.Sessions.Remove(sessionToDelete);
                _context.SaveChanges();
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
            return _context.Sessions.FirstOrDefault(filter);
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
            return _context.Sessions.ToList();
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<Session> FindAllFiltered(Expression<Func<Session, bool>> filter, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public IList<Session> FindAllFiltered(Expression<Func<Session, bool>> filter)
    {
        throw new NotImplementedException();
    }

    public Session? Update(Session updatedEntity)
    {
        try
        {
            Session foundSession = _context.Sessions.FirstOrDefault(s => s.SessionId == updatedEntity.SessionId);

            if (foundSession != null)
            {
                _context.Entry(foundSession).CurrentValues.SetValues(updatedEntity);
                _context.SaveChanges();
                return _context.Sessions.FirstOrDefault(s => s.SessionId == updatedEntity.SessionId);
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
