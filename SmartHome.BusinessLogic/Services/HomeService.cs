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
public sealed class HomeService : IHomeLogic
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

    public void AddHomeMemberToHome(Guid homeId, Guid userId)
    {
        var user = _userRepository.Find(x => x.Id == userId);
        if (user == null)
        {
            throw new HomeException("User Id does not match any user");
        }

        var homeMember = new HomeMember(user, false);
        var home = _homeRepository.Find(x => x.Id == homeId);
        if (home == null)
        {
            throw new HomeException("Home Id does not match any home");
        }

        home.Members.Add(homeMember);
        _homeRepository.Update(home);
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

    public IEnumerable<HomeDevice> GetAllHomeDevices(Guid homeId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<HomeMember> GetAllHomeMembers(Guid homeId)
    {
        var home = _homeRepository.Find(x => x.Id == homeId);
        if (home.Members == null)
        {
            throw new HomeException("Home Id does not match any home");
        }

        return home.Members.ToList();
    }

    public IEnumerable<Home> GetAllHomesByUserId(Guid userId)
    {
        throw new NotImplementedException();
    }

    private void ValidateHome(Home home)
    {
        if (string.IsNullOrEmpty(home.MainStreet))
        {
            throw new HomeArgumentException("Invalid main street, cannot be empty");
        }

        if (string.IsNullOrEmpty(home.DoorNumber))
        {
            throw new HomeArgumentException("Invalid door number, cannot be empty");
        }

        if (string.IsNullOrEmpty(home.Latitude))
        {
            throw new HomeArgumentException("Invalid latitude, cannot be empty");
        }

        if (string.IsNullOrEmpty(home.Longitude))
        {
            throw new HomeArgumentException("Invalid longitude, cannot be empty");
        }

        if (home.MaxMembers < 1)
        {
            throw new HomeArgumentException("Invalid max members, must be at least 1");
        }
    }
}
