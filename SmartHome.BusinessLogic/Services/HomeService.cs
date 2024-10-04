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
public sealed class HomeService : IHomeLogic, IHomeMemberLogic, INotificationLogic
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Home> _homeRepository;
    private readonly IGenericRepository<HomePermission> _homePermissionRepository;
    private readonly IGenericRepository<HomeDevice> _homeDeviceRepository;
    public HomeService(IGenericRepository<HomeDevice> homeDeviceRepository, IGenericRepository<Home> homeRepository, IGenericRepository<User> userRepository, IGenericRepository<HomePermission> homePermissionRepository)
    {
        _homeRepository = homeRepository;
        _userRepository = userRepository;
        _homePermissionRepository = homePermissionRepository;
        _homeDeviceRepository = homeDeviceRepository;
    }

    public void AddDeviceToHome(Guid homeId, Guid deviceId)
    {
        throw new NotImplementedException();
    }

    public void AddHomeMemberToHome(Guid homeId, Guid? userId)
    {
        var user = _userRepository.Find(x => x.Id == userId);
        if (user == null)
        {
            throw new HomeException("User Id does not match any user");
        }

        var homeMember = new HomeMember(user);
        var home = _homeRepository.Find(x => x.Id == homeId);
        if (home == null)
        {
            throw new HomeException("Home Id does not match any home");
        }

        home.Members.Add(homeMember);
        _homeRepository.Update(home);
    }

    private void AddPermissionsToOwner(HomeMember homeOwnerMember)
    {
        var PermissionsList = _homePermissionRepository.FindAll().ToList();
        homeOwnerMember.HomePermissions = PermissionsList;
    }

    public Home CreateHome(Home home, Guid? userId)
    {
        ValidateHome(home);
        var owner = _userRepository.Find(x => x.Id == userId);
        if (owner == null)
        {
            throw new HomeException("User Id does not match any user");
        }

        home.Owner = owner;
        var homeOwnerMember = new HomeMember(owner);
        AddPermissionsToOwner(homeOwnerMember);
        home.Members.Add(homeOwnerMember);
        _homeRepository.Add(home);
        return home;
    }

    public IEnumerable<HomeDevice> GetAllHomeDevices(Guid homeId)
    {
        var home = _homeRepository.Find(x => x.Id == homeId);
        if (home == null)
        {
            throw new HomeException("Home Id does not match any home");
        }

        if (home.Devices == null)
        {
            throw new HomeException("Home devices was not found");
        }

        return home.Devices.ToList();
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

    public IEnumerable<Home> GetAllHomesByUserId(Guid userId)
    {
        throw new NotImplementedException();
    }

    public void AddHomePermissionsToHomeMember(Guid homeMemberId, List<HomePermission> permissions)
    {
        throw new NotImplementedException();
    }

    public void UpdateHomePermissionsOfHomeMember(Guid homeMemberId, List<HomePermission> permissions)
    {
        throw new NotImplementedException();
    }

    public List<Notification> GetNotificationsByHomeMemberId(Guid homeMemberId)
    {
        throw new NotImplementedException();
    }

    public Notification CreateMovementDetectionNotification(Guid homeDeviceId)
    {
        var homeDevice = _homeDeviceRepository.Find(x => x.Id == homeDeviceId);
        if (homeDevice == null)
        {
            throw new HomeException("Home Device Id does not match any home device");
        }

        if (!homeDevice.Online)
        {
            throw new HomeDeviceException("Device is offline");
        }

        var home = _homeRepository.Find(x => x.Id == homeDevice.HomeId);
        if (home == null)
        {
            throw new HomeException("Home Id does not match any home");
        }

        var notification = new Notification
        {
            Date = DateTime.Today,
            Event = "Movement Detection",
            HomeDevice = homeDevice,
            Time = DateTime.Now
        };

        var homeMembers = home.Members;
        foreach (var homeMember in homeMembers)
        {
            homeMember.Notifications.Add(notification);
        }

        _homeRepository.Update(home);

        return notification;
    }

    public Notification CreatePersonDetectionNotification(Guid homeDeviceId)
    {
        throw new NotImplementedException();
    }
}
