using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;

namespace SmartHome.DataAccess.Repositories;
public sealed class HomeMemberRepository : IGenericRepository<HomeMember>
{
    public readonly SmartHomeEFCoreContext _repository;
    public HomeMemberRepository(SmartHomeEFCoreContext repository)
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

    public HomeMember Add(HomeMember entity)
    {
        try
        {
            _repository.HomeMembers.Add(entity);
            _repository.SaveChanges();
            return _repository.HomeMembers.FirstOrDefault(b => b.HomeMemberId == entity.HomeMemberId);
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
            HomeMember homeMemberToDelete = _repository.HomeMembers.FirstOrDefault(b => b.HomeMemberId == id);
            if (homeMemberToDelete != null)
            {
                _repository.HomeMembers.Remove(homeMemberToDelete);
                _repository.SaveChanges();
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
            return _repository.HomeMembers.Include(x=>x.HomePermissions).Include(x=> x.Notifications).FirstOrDefault(filter);
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
            return _repository.HomeMembers.ToList();
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
            HomeMember foundHomeMember = _repository.HomeMembers.FirstOrDefault(b => b.HomeMemberId == updatedEntity.HomeMemberId);

            if (foundHomeMember != null)
            {
                foundHomeMember.HomePermissions = updatedEntity.HomePermissions;
                _repository.Entry(foundHomeMember).CurrentValues.SetValues(updatedEntity);
                _repository.SaveChanges();
                return _repository.HomeMembers.FirstOrDefault(b => b.HomeMemberId == updatedEntity.HomeMemberId);
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
}
