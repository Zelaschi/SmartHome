using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.GenericRepositoryInterface;

namespace SmartHome.BusinessLogic.Services;
public sealed class UserService : IHomeOwnerLogic
{
    private readonly IGenericRepository<User> _userRepository;
    public UserService(IGenericRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public HomeMember CreateHomeMember(HomeMember homeMember)
    {
        throw new NotImplementedException();
    }

    public User CreateHomeOwner(User user)
    {
        User newHomeOwner = _userRepository.Add(user);
        return newHomeOwner;
    }
}
