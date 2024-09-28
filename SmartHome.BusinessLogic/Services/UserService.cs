using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.BusinessLogic.Services;
public sealed class UserService : IHomeOwnerLogic
{
    private readonly IGenericRepository<User> _genericRepository;
    public UserService(IGenericRepository<User> genericRepository)
    {
        _genericRepository = genericRepository;
    }

    public HomeMember CreateHomeMember(HomeMember homeMember)
    {
        throw new NotImplementedException();
    }

    public User CreateHomeOwner(User user)
    {
        throw new NotImplementedException();
    }
}
