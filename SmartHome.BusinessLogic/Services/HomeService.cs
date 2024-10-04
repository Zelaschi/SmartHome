using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.BusinessLogic.Services;
public sealed class HomeService : IHomeLogic, IHomeMemberLogic, INotificationLogic
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Home> _homeRepository;
    private readonly IGenericRepository<HomePermission> _homePermissionRepository;
    private readonly IGenericRepository<HomeDevice> _homeDeviceRepository;
    private readonly IGenericRepository<HomeMember> _homeMemberRepository;
    private readonly IGenericRepository<Device> _deviceRepository;

    public HomeService(IGenericRepository<HomeMember> homeMemberRepository, IGenericRepository<HomeDevice> homeDeviceRepository,
        IGenericRepository<Home> homeRepository, IGenericRepository<User> userRepository,
        IGenericRepository<HomePermission> homePermissionRepository, IGenericRepository<Device> deviceRepository)
    {
        _homeMemberRepository = homeMemberRepository;
        _homeRepository = homeRepository;
        _userRepository = userRepository;
        _homePermissionRepository = homePermissionRepository;
        _homeDeviceRepository = homeDeviceRepository;
        _deviceRepository = deviceRepository;
    }

    public void AddDeviceToHome(Guid homeId, Guid deviceId)
    {
        var home = FindHomeById(homeId);

        var device = _deviceRepository.Find(x => x.Id == deviceId);

        if (device == null)
        {
            throw new HomeDeviceException("Device Id does not match any device");
        }

        var homeDevice = new HomeDevice
        {
            Id = Guid.NewGuid(),
            Device = device,
            HomeId = homeId,
            Online = true
        };

        home.Devices.Add(homeDevice);

        _homeRepository.Update(home);
    }

    public HomeMember AddHomeMemberToHome(Guid homeId, Guid? userId)
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
        return homeMember;
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

    public bool HasPermission(Guid homeMemberid, Guid homePermissionId)
    {
        var homeMember = _homeMemberRepository.Find(x => x.HomeMemberId == homeMemberid);
        var homePermission = _homePermissionRepository.Find(x => x.Id == homePermissionId);
        if (homeMember == null)
        {
            throw new HomeException("Home Member Id does not match any home member");
        }

        if (homePermission == null)
        {
            throw new HomeException("Home Permission Id does not match any home permission");
        }

        if (homeMember.HomePermissions.Contains(homePermission))
        {
            return true;
        }

        return false;
    }

    private HomeDevice FindHomeDeviceById(Guid homeDeviceId)
    {
        var homeDevice = _homeDeviceRepository.Find(x => x.Id == homeDeviceId);
        if (homeDevice == null)
        {
            throw new HomeException("Home Device Id does not match any home device");
        }

        return homeDevice;
    }

    private Home FindHomeById(Guid homeId)
    {
        var home = _homeRepository.Find(x => x.Id == homeId);
        if (home == null)
        {
            throw new HomeException("Home Id does not match any home");
        }

        return home;
    }

    private Notification CreateNotification(string eventString, HomeDevice homeDevice)
    {
        return new Notification
        {
            Date = DateTime.Today,
            Event = eventString,
            HomeDevice = homeDevice,
            Time = DateTime.Now
        };
    }

    private void CheckDeviceOnline(HomeDevice device)
    {
        if (!device.Online)
        {
            throw new HomeDeviceException("Device is offline");
        }
    }

    public Notification CreateMovementDetectionNotification(Guid homeDeviceId)
    {
        var notificationPermission = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID);
        var homeDevice = FindHomeDeviceById(homeDeviceId);

        CheckDeviceOnline(homeDevice);

        var home = FindHomeById(homeDevice.HomeId);
        var homeMembers = home.Members;

        var notification = CreateNotification("Movement Detection", homeDevice);

        foreach (var homeMember in homeMembers)
        {
            if (HasPermission(homeMember.HomeMemberId, notificationPermission))
            {
                homeMember.Notifications.Add(notification);
            }
        }

        _homeRepository.Update(home);

        return notification;
    }

    public Notification CreatePersonDetectionNotification(Guid homeDeviceId)
    {
        throw new NotImplementedException();
    }
}
