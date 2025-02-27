﻿using SmartHome.BusinessLogic.Constants;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.DTOs;
using SmartHome.BusinessLogic.ExtraRepositoryInterfaces;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.BusinessLogic.Services;
public sealed class HomeService : IHomeLogic, IHomeMemberLogic, INotificationLogic, IHomePermissionLogic, IRoomLogic
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Home> _homeRepository;
    private readonly IGenericRepository<HomePermission> _homePermissionRepository;
    private readonly IGenericRepository<HomeDevice> _homeDeviceRepository;
    private readonly IGenericRepository<HomeMember> _homeMemberRepository;
    private readonly IGenericRepository<Device> _deviceRepository;
    private readonly IGenericRepository<Room> _roomRepository;
    private readonly IHomesFromUserRepository _homesFromUserRepository;
    private readonly IUpdateMultipleElementsRepository<HomeMember> _updateMultipleElementsRepository;

    public HomeService(
        IGenericRepository<HomeMember> homeMemberRepository,
        IGenericRepository<HomeDevice> homeDeviceRepository,
        IGenericRepository<Home> homeRepository,
        IGenericRepository<User> userRepository,
        IGenericRepository<HomePermission> homePermissionRepository,
        IGenericRepository<Device> deviceRepository,
        IGenericRepository<Room> roomRepository,
        IHomesFromUserRepository homesFromUserRepository,
        IUpdateMultipleElementsRepository<HomeMember> updateMultipleElementsRepository
    )
    {
        _homeMemberRepository = homeMemberRepository;
        _homeRepository = homeRepository;
        _userRepository = userRepository;
        _homePermissionRepository = homePermissionRepository;
        _homeDeviceRepository = homeDeviceRepository;
        _deviceRepository = deviceRepository;
        _homesFromUserRepository = homesFromUserRepository;
        _roomRepository = roomRepository;
        _updateMultipleElementsRepository = updateMultipleElementsRepository;
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
        var home = GetHomeById(homeId);
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

    public IEnumerable<HomeDevice> GetAllHomeDevices(Guid homeId, string? room)
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

        var homeDevicesToReturn = home.Devices;

        if (room != null && homeDevicesToReturn != null)
        {
            homeDevicesToReturn = (List<HomeDevice>)homeDevicesToReturn.Where(x => x.Room != null && x.Room.Name == room).ToList();
        }

        return homeDevicesToReturn;
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

        return home.Members;
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

    public void UpdateHomePermissionsOfHomeMember(Guid homeMemberId, List<HomePermission> permissions, Guid? homeOwner)
    {
        if (homeOwner == null)
        {
            throw new HomeException("Home Owner Id is null");
        }

        var owner = _homeMemberRepository.Find(x => x.User.Id == homeOwner);

        if (owner == null)
        {
            throw new HomeException("Owner was not found");
        }

        if (owner.HomePermissions.FirstOrDefault(x => x.Id == Guid.Parse(SeedDataConstants.ADD_PERMISSIONS_TO_HOMEMEMBER_ID)) == null)
        {
            throw new HomeException("User does not have permission to add permissions to home member");
        }

        var member = FindHomeMemberById(homeMemberId);
        var allPermissions = _homePermissionRepository.FindAll().ToList();
        var foundPermissions = allPermissions
                .Where(permission => permissions.Any(p => p.Id == permission.Id))
                .ToList();
        if (member.HomePermissions.Count > 0)
        {
            member.HomePermissions.Clear();
            _homeMemberRepository.Update(member);
        }

        member.HomePermissions = foundPermissions;
        _homeMemberRepository.Update(member);
    }

    public List<DTONotification> GetUsersNotifications(User user)
    {
        var userHomeMembers = _homeRepository
             .FindAll()
             .SelectMany(home => home.Members)
             .Where(member => member.User.Id == user.Id)
             .ToList();

        var notifications = new List<Notification>();
        var updatedHomeOwners = new List<HomeMember>();
        var DtoNotifications = new List<DTONotification>();

        userHomeMembers.ForEach(homeMember =>
        {
            var homeOwnersNotifications = homeMember.HomeMemberNotifications
                .Where(hmn => hmn.Read)
                .Select(hmn => hmn.Notification)
                .ToList();
            notifications.AddRange(homeOwnersNotifications);
            DtoNotifications.AddRange(notifications.Select(noti => new DTONotification() { Notification = noti, Read = true }));

            var unReadNotifications = homeMember.HomeMemberNotifications
                .Where(hmn => !hmn.Read)
                .Select(hmn => hmn.Notification)
                .ToList();
            if (unReadNotifications.Any())
            {
                DtoNotifications.AddRange(unReadNotifications.Select(noti => new DTONotification() { Notification = noti, Read = false }));
                MarkNotificationsAsRead(homeMember.HomeMemberNotifications, unReadNotifications);
                updatedHomeOwners.Add(homeMember);
            }
        });

        _updateMultipleElementsRepository.UpdateMultiplElements(updatedHomeOwners);

        return DtoNotifications;
    }

    public Home GetHomeById(Guid homeId)
    {
        var home = _homeRepository.Find(x => x.Id == homeId);
        if (home == null)
        {
            throw new HomeException("Home Id does not match any home");
        }

        return home;
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
        HomeDevice homedevice;
        switch (device.Type)
        {
            case DeviceTypesStatic.WindowSensor:
                homedevice = new HomeDevice
                {
                    Name = device.Name,
                    Id = Guid.NewGuid(),
                    Device = device,
                    HomeId = homeId,
                    Online = true,
                    IsOpen = true
                };
                break;
            case DeviceTypesStatic.InteligentLamp:
                homedevice = new HomeDevice
                {
                    Name = device.Name,
                    Id = Guid.NewGuid(),
                    Device = device,
                    HomeId = homeId,
                    Online = true,
                    IsOn = true
                };

                break;
            default:
                homedevice = new HomeDevice
                {
                    Name = device.Name,
                    Id = Guid.NewGuid(),
                    Device = device,
                    HomeId = homeId,
                    Online = true
                };
                break;
        }

        return homedevice;
    }

    public Notification CreateMovementDetectionNotification(Guid homeDeviceId)
    {
        var notificationPermission = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID);
        var homeDevice = FindHomeDeviceById(homeDeviceId);

        if (homeDevice.Device.Type != DeviceTypesStatic.SecurityCamera)
        {
            throw new DeviceException("The device type is not Security Camera");
        }

        CheckDeviceOnline(homeDevice);

        var home = GetHomeById(homeDevice.HomeId);
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

        if (homeDevice.Device.Type != DeviceTypesStatic.SecurityCamera)
        {
            throw new DeviceException("The device type is not Security Camera");
        }

        CheckDeviceOnline(homeDevice);

        var home = GetHomeById(homeDevice.HomeId);
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

    public Notification CreateOpenCloseWindowNotification(Guid homeDeviceId)
    {
        var notificationPermission = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID);
        var homeDevice = FindHomeDeviceById(homeDeviceId);

        if (homeDevice.Device.Type != DeviceTypesStatic.WindowSensor)
        {
            throw new DeviceException("The device type is not Window Sensor");
        }

        CheckDeviceOnline(homeDevice);

        var home = GetHomeById(homeDevice.HomeId);
        var homeMembers = home.Members;

        if (homeDevice.IsOpen == true)
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
        var home = GetHomeById(homeId);
        home.Name = newName;
        _homeRepository.Update(home);
    }

    public Room CreateRoom(Room room, Guid homeId)
    {
        var home = GetHomeById(homeId);

        if (home.Rooms == null)
        {
            home.Rooms = new List<Room>();
        }

        if (home.Rooms.Any(x => x.Name == room.Name))
        {
            throw new RoomException("Room already exists");
        }

        room.Home = home;
        home.Rooms.Add(room);
        _homeRepository.Update(home);
        return room;
    }

    public Room FindRoomById(Guid roomId)
    {
        var room = _roomRepository.Find(x => x.Id == roomId);
        if (room == null)
        {
            throw new RoomException("Room not found");
        }

        return room;
    }

    public HomeDevice AddDevicesToRoom(Guid homeDeviceId, Guid roomId)
    {
        var homeDevice = FindHomeDeviceById(homeDeviceId);
        var room = FindRoomById(roomId);

        if (room.HomeDevices == null)
        {
            room.HomeDevices = new List<HomeDevice>();
        }

        if (room.HomeDevices.Any(x => x.Id == homeDeviceId))
        {
            throw new RoomException("Device already exists in room");
        }

        room.HomeDevices.Add(homeDevice);
        _roomRepository.Update(room);
        return homeDevice;
    }

    public List<Room> GetAllRoomsFromHome(Guid homeId)
    {
        var home = GetHomeById(homeId);
        return home.Rooms;
    }

    public List<HomePermission> GetAllHomePermissions()
    {
        return _homePermissionRepository.FindAll().ToList();
    }

    public IEnumerable<User> UnRelatedHomeOwners(Guid homeId)
    {
        var home = GetHomeById(homeId);
        if (home == null)
        {
            throw new HomeException("Home was not found");
        }

        var homeOwners = _userRepository.FindAll()
            .Where(user => user.Role != null &&
                           (user.Role.Name == "BusinessOwnerHomeOwner" ||
                            user.Role.Name == "AdminHomeOwner" ||
                            user.Role.Name == "HomeOwner"))
            .ToList();
        if (home.Members == null)
        {
            throw new HomeException("Home members is null");
        }

        var homeUsers = home.Members
            .Select(member => member.User);
        var usersNotInHome = homeOwners
            .Where(owner => !homeUsers.Any(user => user.Id == owner.Id))
            .ToList();

        return usersNotInHome;
    }

    public bool TurnOnOffHomeDevice(Guid homeDeviceId)
    {
        var homeDevice = FindHomeDeviceById(homeDeviceId);
        if (homeDevice == null)
        {
            throw new HomeException("Home device was not found");
        }

        homeDevice.Online = !homeDevice.Online;
        _homeDeviceRepository.Update(homeDevice);
        return homeDevice.Online;
    }
}
