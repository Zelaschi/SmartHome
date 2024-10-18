using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.ExtraRepositoryInterfaces;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.BusinessLogic.Services;
public sealed class HomeService : IHomeLogic, IHomeMemberLogic, INotificationLogic, IHomePermissionLogic
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Home> _homeRepository;
    private readonly IGenericRepository<HomePermission> _homePermissionRepository;
    private readonly IGenericRepository<HomeDevice> _homeDeviceRepository;
    private readonly IGenericRepository<HomeMember> _homeMemberRepository;
    private readonly IGenericRepository<Device> _deviceRepository;
    private readonly IHomesFromUserRepository _homesFromUserRepository;

    public HomeService(IGenericRepository<HomeMember> homeMemberRepository, IGenericRepository<HomeDevice> homeDeviceRepository,
        IGenericRepository<Home> homeRepository, IGenericRepository<User> userRepository,
        IGenericRepository<HomePermission> homePermissionRepository, IGenericRepository<Device> deviceRepository,
        IHomesFromUserRepository homesFromUserRepository)
    {
        _homeMemberRepository = homeMemberRepository;
        _homeRepository = homeRepository;
        _userRepository = userRepository;
        _homePermissionRepository = homePermissionRepository;
        _homeDeviceRepository = homeDeviceRepository;
        _deviceRepository = deviceRepository;
        _homesFromUserRepository = homesFromUserRepository;
    }

    private User FindUserById(Guid? userId)
    {
        var user = _userRepository.Find(x => x.Id == userId);
        if (user == null)
        {
            throw new HomeException("User Id does not match any user");
        }

        return user;
    }

    private void CheckHomeAvailability(Home home)
    {
        if (home.Members.Count >= home.MaxMembers)
        {
            throw new HomeException("Home has no more space");
        }
    }

    private void CheckUserIsNotAlreadyInHome(Home home, User user)
    {
        if (home.Members.Any(member => member.User.Id == user.Id))
        {
            throw new HomeException("User is already in home");
        }
    }

    public HomeMember AddHomeMemberToHome(Guid homeId, Guid userId)
    {
        var user = FindUserById(userId);
        var homeMember = new HomeMember(user);
        var home = FindHomeById(homeId);
        CheckUserIsNotAlreadyInHome(home, user);
        CheckHomeAvailability(home);
        homeMember.HomeId = homeId;
        _homeMemberRepository.Add(homeMember);
        return homeMember;
    }

    private void AddPermissionsToOwner(HomeMember homeOwnerMember)
    {
        var PermissionsList = _homePermissionRepository.FindAll().ToList();
        homeOwnerMember.HomePermissions = PermissionsList;
    }

    private bool RepeatedHome(Home home)
    {
        var reapeatedHome = _homeRepository.Find(x => x.MainStreet == home.MainStreet && x.DoorNumber == home.DoorNumber && x.Latitude == home.Latitude && x.Longitude == home.Longitude);
        if (reapeatedHome != null) return true;
        return false;
    }

    public Home CreateHome(Home home, Guid? userId)
    {
        ValidateHome(home);
        var owner = _userRepository.Find(x => x.Id == userId);
        if (owner == null)
        {
            throw new HomeException("User Id does not match any user");
        }

        if (RepeatedHome(home))
        {
            throw new HomeException("Home already exists");
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
        if (home == null)
        {
            throw new HomeException("Home Id does not match any home");
        }

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
        var homes = _homesFromUserRepository.GetAllHomesByUserId(userId);

        if (homes == null)
        {
            throw new UserException("This user id does not correspond to any house");
        }

        return homes;
    }

    private HomeMember FindHomeMemberById(Guid id)
    {
        var member = _homeMemberRepository.Find(x => x.HomeMemberId == id);
        if (member == null)
        {
            throw new HomeException("Home Member not found");
        }

        return member;
    }

    public void UpdateHomePermissionsOfHomeMember(Guid homeMemberId, List<HomePermission> permissions)
    {
        var member = FindHomeMemberById(homeMemberId);
        var allPermissions = _homePermissionRepository.FindAll().ToList();
        var foundPermissions = allPermissions
                .Where(permission => permissions.Any(p => p.Id == permission.Id))  // Comparación por Id o cualquier otra propiedad
                .ToList();
        if (member.HomePermissions.Count > 0)
        {
            member.HomePermissions.Clear();
            _homeMemberRepository.Update(member);
        }

        member.HomePermissions = foundPermissions;
        _homeMemberRepository.Update(member);
    }

    public List<Notification> GetUsersNotifications(User user)
    {
        var userHomeMembers = _homeRepository
            .FindAll()
            .SelectMany(home => home.Members)
            .Where(member => member.User.Id == user.Id)
            .ToList();

        var notifications = new List<Notification>();

        foreach (var homeMember in userHomeMembers)
        {
            var unReadNotifications = homeMember.HomeMemberNotifications
                .Where(hmn => !hmn.Read)
                .Select(hmn => hmn.Notification)
                .ToList();

            if (unReadNotifications.Any())
            {
               MarkNotificationsAsRead(homeMember.HomeMemberNotifications, unReadNotifications);

                notifications.AddRange(unReadNotifications);
            }

            _homeMemberRepository.Update(homeMember);
        }

        return notifications;
    }

    private void MarkNotificationsAsRead(IEnumerable<HomeMemberNotification> homeMemberNotifications, List<Notification> notifications)
    {
        foreach (var notification in notifications)
        {
            var homeMemberNotification = homeMemberNotifications
                .FirstOrDefault(hmn => hmn.NotificationId == notification.Id);

            if (homeMemberNotification != null)
            {
                homeMemberNotification.Read = true;
            }
        }
    }

    public bool HasPermission(Guid userId, Guid homeId, Guid permissionId)
    {
        var home = _homeRepository.Find(x => x.Id == homeId);
        if (home == null)
        {
            throw new HomeException("Home Id does not match any home");
        }

        var homeMember = home.Members.FirstOrDefault(x => x.User.Id == userId);
        if (homeMember == null)
        {
            throw new HomeException("User is not a member of this home");
        }

        var homePermission = _homePermissionRepository.Find(x => x.Id == permissionId);
        if (homePermission == null)
        {
            throw new HomeException("HomePermission was not found");
        }

        if (homeMember.HomePermissions.Contains(homePermission))
        {
            return true;
        }

        return false;
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

    private Device FindDeviceById(Guid deviceId)
    {
        var device = _deviceRepository.Find(x => x.Id == deviceId);

        if (device == null)
        {
            throw new HomeDeviceException("Device Id does not match any device");
        }

        return device;
    }

    private HomeDevice CreateHomeDevice(Guid homeId, Device device)
    {
        return new HomeDevice
        {
            Name = device.Name,
            Id = Guid.NewGuid(),
            Device = device,
            HomeId = homeId,
            Online = true
        };
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

    private void VerifyHomeExistance(Guid homeId)
    {
        var home = _homeRepository.Find(x => x.Id == homeId);
        if (home == null)
        {
            throw new HomeException("Home Id does not match any home");
        }
    }

    public HomeDevice AddDeviceToHome(Guid homeId, Guid deviceId)
    {
        var device = FindDeviceById(deviceId);

        var homeDevice = CreateHomeDevice(homeId, device);
        VerifyHomeExistance(homeId);

        _homeDeviceRepository.Add(homeDevice);
        return homeDevice;
    }

    private Notification CreateDetectedPersonNotification(HomeDevice homeDevice, Guid detectedPersonId)
    {
        var detectedPerson = _userRepository.Find(x => x.Id == detectedPersonId);
        if (detectedPerson == null)
        {
            return new Notification
            {
                Date = DateTime.Today,
                Event = "Undetected person",
                HomeDevice = homeDevice,
                Time = DateTime.Now,
                DetectedPerson = detectedPerson
            };
        }

        return new Notification
        {
            Date = DateTime.Today,
            Event = detectedPerson.Name + " detected!",
            HomeDevice = homeDevice,
            Time = DateTime.Now,
            DetectedPerson = detectedPerson
        };
    }

    public Notification CreatePersonDetectionNotification(Guid homeDeviceId, Guid userId)
    {
        var notificationPermission = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID);
        var homeDevice = FindHomeDeviceById(homeDeviceId);

        CheckDeviceOnline(homeDevice);

        var home = FindHomeById(homeDevice.HomeId);
        var homeMembers = home.Members;

        var notification = CreateDetectedPersonNotification(homeDevice, userId);

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

    public Notification CreateOpenCloseWindowNotification(Guid homeDeviceId, bool opened)
    {
        var notificationPermission = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID);
        var homeDevice = FindHomeDeviceById(homeDeviceId);

        CheckDeviceOnline(homeDevice);

        var home = FindHomeById(homeDevice.HomeId);
        var homeMembers = home.Members;

        if (opened)
        {
            var notificationOpened = CreateNotification("Window Opened", homeDevice);

            foreach (var homeMember in homeMembers)
            {
                if (HasPermission(homeMember.HomeMemberId, notificationPermission))
                {
                    homeMember.Notifications.Add(notificationOpened);
                }
            }

            _homeRepository.Update(home);

            return notificationOpened;
        }
        else
        {
            var notificationClosed = CreateNotification("Window Closed", homeDevice);

            foreach (var homeMember in homeMembers)
            {
                if (HasPermission(homeMember.HomeMemberId, notificationPermission))
                {
                    homeMember.Notifications.Add(notificationClosed);
                }
            }

            _homeRepository.Update(home);

            return notificationClosed;
        }
    }

    public void UpdateHomeDeviceName(Guid homeDeviceId, string newName)
    {
        var homeDevice = FindHomeDeviceById(homeDeviceId);
        homeDevice.Name = newName;
        _homeDeviceRepository.Update(homeDevice);
    }

    public void UpdateHomeName(Guid homeId, string newName)
    {
        var home = FindHomeById(homeId);
        home.Name = newName;
        _homeRepository.Update(home);
    }
}
