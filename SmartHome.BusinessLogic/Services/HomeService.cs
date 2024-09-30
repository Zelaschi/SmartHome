using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.BusinessLogic.Services;
public sealed class HomeService : IHomeMemberLogic, IHomeLogic
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Home> _homeRepository;
    private readonly IGenericRepository<HomeMember> _homeMemberRepository;
    public HomeService(IGenericRepository<Home> homeRepository, IGenericRepository<HomeMember> homeMemberRepository, IGenericRepository<User> userRepository)
    {
        _homeRepository = homeRepository;
        _homeMemberRepository = homeMemberRepository;
        _userRepository = userRepository;
    }

    public void AddDeviceToHome(Guid homeId, Guid deviceId)
    {
        throw new NotImplementedException();
    }

    public Home CreateHome(Home home, Guid userId)
    {
        ValidateHome(home);
        var owner = _userRepository.Find(x => x.Id == userId);
        if (owner == null)
        {
            throw new HomeException("User Id does not match any user");
        }

        home.Owner = owner;
        var homeOwnerMember = new HomeMember(owner, true);
        home.Members.Add(homeOwnerMember);
        _homeRepository.Add(home);
        return home;
    }

    public HomeMember CreateHomeMember(HomeMember homeMember)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<HomeMember> GetAllHomeMembers()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Home> GetAllHomesByUserId(Guid userId)
    {
        throw new NotImplementedException();
    }

    private void ValidateHome(Home home)
    {
        if (string.IsNullOrEmpty(home.MainStreet))
        {
            throw new HomeException("Invalid main street, cannot be empty");
        }

        if (string.IsNullOrEmpty(home.DoorNumber))
        {
            throw new HomeException("Invalid door number, cannot be empty");
        }

        if (string.IsNullOrEmpty(home.Latitude))
        {
            throw new HomeException("Invalid latitude, cannot be empty");
        }

        if (string.IsNullOrEmpty(home.Longitude))
        {
            throw new HomeException("Invalid longitude, cannot be empty");
        }

        if (home.MaxMembers < 1)
        {
            throw new HomeException("Invalid max members, must be at least 1");
        }
    }
}
