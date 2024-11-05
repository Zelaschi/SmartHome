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
public sealed class RoomRepository : IGenericRepository<Room>
{
    private readonly SmartHomeEFCoreContext _context;
    public RoomRepository(SmartHomeEFCoreContext context)
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

    public Room Add(Room entity)
    {
        try
        {
            _context.Rooms.Add(entity);
            _context.SaveChanges();
            return _context.Rooms.FirstOrDefault(r => r.Id == entity.Id);
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
            Room roomToDelete = _context.Rooms.FirstOrDefault(r => r.Id == id);
            if (roomToDelete != null)
            {
                _context.Rooms.Remove(roomToDelete);
                _context.SaveChanges();
            }
            else
            {
                throw new DatabaseException("The Room does not exist in the Data Base.");
            }
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public Room? Find(Func<Room, bool> filter)
    {
        try
        {
            return _context.Rooms.FirstOrDefault(filter);
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<Room> FindAll()
    {
        try
        {
            return _context.Rooms.ToList();
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<Room> FindAllFiltered(Expression<Func<Room, bool>> filter, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public IList<Room> FindAllFiltered(Expression<Func<Room, bool>> filter)
    {
        throw new NotImplementedException();
    }

    public Room? Update(Room updatedEntity)
    {
        try
        {
            Room roomToUpdate = _context.Rooms.FirstOrDefault(r => r.Id == updatedEntity.Id);
            if (roomToUpdate != null)
            {
                roomToUpdate.Name = updatedEntity.Name;
                _context.SaveChanges();
                return _context.Rooms.FirstOrDefault(r => r.Id == updatedEntity.Id);
            }
            else
            {
                throw new DatabaseException("The Room does not exist in the Data Base.");
            }
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }
}
