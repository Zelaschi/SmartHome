using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;

namespace SmartHome.DataAccess.Repositories;
public sealed class HomeMemberRepository : IGenericRepository<HomeMember>, IUpdateMultipleElementsRepository<HomeMember>
{
    public readonly SmartHomeEFCoreContext _context;
    public HomeMemberRepository(SmartHomeEFCoreContext context)
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

    public HomeMember Add(HomeMember entity)
    {
        try
        {
            _context.HomeMembers.Add(entity);
            _context.SaveChanges();
            return _context.HomeMembers.FirstOrDefault(b => b.HomeMemberId == entity.HomeMemberId);
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
            HomeMember homeMemberToDelete = _context.HomeMembers.FirstOrDefault(b => b.HomeMemberId == id);
            if (homeMemberToDelete != null)
            {
                _context.HomeMembers.Remove(homeMemberToDelete);
                _context.SaveChanges();
            }
            else
            {
                throw new DatabaseException("The HomeMember does not exist in the Data Base.");
            }
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public HomeMember? Find(Func<HomeMember, bool> filter)
    {
        try
        {
            return _context.HomeMembers.Include(x => x.HomePermissions).Include(x => x.Notifications).Include(x => x.User).FirstOrDefault(filter);
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<HomeMember> FindAll()
    {
        try
        {
            return _context.HomeMembers.ToList();
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<HomeMember> FindAllFiltered(Expression<Func<HomeMember, bool>> filter, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public IList<HomeMember> FindAllFiltered(Expression<Func<HomeMember, bool>> filter)
    {
        throw new NotImplementedException();
    }

    public HomeMember? Update(HomeMember updatedEntity)
    {
        try
        {
            HomeMember foundHomeMember = _context.HomeMembers.FirstOrDefault(b => b.HomeMemberId == updatedEntity.HomeMemberId);

            if (foundHomeMember != null)
            {
                foundHomeMember.HomePermissions = updatedEntity.HomePermissions;
                _context.Entry(foundHomeMember).CurrentValues.SetValues(updatedEntity);
                _context.SaveChanges();
                return _context.HomeMembers.FirstOrDefault(b => b.HomeMemberId == updatedEntity.HomeMemberId);
            }
            else
            {
                throw new DatabaseException("The HomeMember does not exist in the Data Base.");
            }
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }

    public IList<HomeMember>? UpdateMultiplElements(List<HomeMember> updatedEntitys)
    {
        try
        {
            var updatedHomeMembers = new List<HomeMember>();
            updatedEntitys.ForEach(updatedEntity =>
            {
                HomeMember foundHomeMember = _context.HomeMembers.FirstOrDefault(b => b.HomeMemberId == updatedEntity.HomeMemberId);

                if (foundHomeMember != null)
                {
                    _context.Entry(foundHomeMember).CurrentValues.SetValues(updatedEntity);
                    var foundUpdatedHomeOwner = _context.HomeMembers.FirstOrDefault(b => b.HomeMemberId == updatedEntity.HomeMemberId);
                    if (foundUpdatedHomeOwner != null) updatedHomeMembers.Add(foundUpdatedHomeOwner);
                }
                else
                {
                    throw new DatabaseException("The HomeMember does not exist in the Data Base.");
                }
            });
            _context.SaveChanges();
            return updatedHomeMembers;
        }
        catch (SqlException)
        {
            throw new DatabaseException("Error related to the Data Base, please validate the connection.");
        }
    }
}
