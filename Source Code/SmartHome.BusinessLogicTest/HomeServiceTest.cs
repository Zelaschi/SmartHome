﻿using FluentAssertions;
using Moq;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.ExtraRepositoryInterfaces;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Services;

namespace SmartHome.BusinessLogic.Test;

[TestClass]
public class HomeServiceTest
{
    private Mock<IGenericRepository<Home>>? homeRepositoryMock;
    private Mock<IGenericRepository<User>>? userRepositoryMock;
    private Mock<IGenericRepository<HomePermission>>? homePermissionRepositoryMock;
    private Mock<IGenericRepository<HomeDevice>>? homeDeviceRepositoryMock;
    private Mock<IGenericRepository<HomeMember>>? homeMemberRepositoryMock;
    private Mock<IGenericRepository<Device>>? deviceRepositoryMock;
    private Mock<IGenericRepository<Room>>? roomRepositoryMock;
    private Mock<IHomesFromUserRepository>? homesFromUserRepositoryMock;
    private Mock<IUpdateMultipleElementsRepository<HomeMember>>? updateMultipleElementsRepositoryMock;
    private HomeService? homeService;
    private Role? homeOwnerRole;
    private Guid ownerId;
    private User? owner;

    [TestInitialize]
    public void Initialize()
    {
        homeRepositoryMock = new Mock<IGenericRepository<Home>>(MockBehavior.Strict);
        userRepositoryMock = new Mock<IGenericRepository<User>>(MockBehavior.Strict);
        homePermissionRepositoryMock = new Mock<IGenericRepository<HomePermission>>(MockBehavior.Strict);
        homeDeviceRepositoryMock = new Mock<IGenericRepository<HomeDevice>>(MockBehavior.Strict);
        homeMemberRepositoryMock = new Mock<IGenericRepository<HomeMember>>(MockBehavior.Strict);
        deviceRepositoryMock = new Mock<IGenericRepository<Device>>(MockBehavior.Strict);
        roomRepositoryMock = new Mock<IGenericRepository<Room>>(MockBehavior.Strict);
        homesFromUserRepositoryMock = new Mock<IHomesFromUserRepository>(MockBehavior.Strict);
        updateMultipleElementsRepositoryMock = new Mock<IUpdateMultipleElementsRepository<HomeMember>>(MockBehavior.Strict);
        homeService = new HomeService(homeMemberRepositoryMock.Object, homeDeviceRepositoryMock.Object, homeRepositoryMock.Object,
                                      userRepositoryMock.Object, homePermissionRepositoryMock.Object, deviceRepositoryMock.Object,
                                      roomRepositoryMock.Object, homesFromUserRepositoryMock.Object, updateMultipleElementsRepositoryMock.Object);
        homeOwnerRole = new Role
        {
            Name = "HomeOwner"
        };
        ownerId = Guid.NewGuid();
        owner = new User
        {
            Email = "owner@blank.com",
            Name = "ownerName",
            Surname = "ownerSurname",
            Password = "ownerPassword",
            Id = ownerId,
            Role = homeOwnerRole
        };
    }

    [TestMethod]
    public void Create_Home_OK_Test()
    {
        var home = new Home
        {
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name"
        };

        homeRepositoryMock.Setup(x => x.Add(It.IsAny<Home>())).Returns(home);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns((Home)null);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(owner);
        homePermissionRepositoryMock.Setup(x => x.FindAll()).Returns(new List<HomePermission>());

        var result = homeService.CreateHome(home, ownerId);

        homeRepositoryMock.VerifyAll();
        userRepositoryMock.VerifyAll();
        homeRepositoryMock.VerifyAll();
        Assert.AreEqual(home, result);
    }

    [TestMethod]
    public void Register_HomeMemberToHouse_OK_Test()
    {
        var home = new Home
        {
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name"
        };
        var memberId = Guid.NewGuid();
        var member = new User
        {
            Email = "blankEmail1@blank.com",
            Name = "blankName1",
            Surname = "blanckSurname1",
            Password = "blankPassword",
            Id = memberId,
            Role = homeOwnerRole
        };

        homeMemberRepositoryMock.Setup(x => x.Add(It.IsAny<HomeMember>())).Returns(new HomeMember(member));
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(owner);

        homeService.AddHomeMemberToHome(home.Id, memberId);
        homeRepositoryMock.VerifyAll();
        userRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void GetAll_HomeMembers_Test()
    {
        var member1 = new User
        {
            Email = "blankEmail1@blank.com",
            Name = "blankName1",
            Surname = "blanckSurname1",
            Password = "blankPassword",
            Id = new Guid(),
            Role = homeOwnerRole
        };
        var member2 = new User
        {
            Email = "blankEmail2@blank.com",
            Name = "blankName2",
            Surname = "blanckSurname2",
            Password = "blankPassword",
            Id = new Guid(),
            Role = homeOwnerRole
        };
        var homeOwner = new HomeMember(owner);
        var homeMember1 = new HomeMember(member1);
        var homeMember2 = new HomeMember(member2);

        var home = new Home
        {
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name"
        };
        home.Members.Add(homeOwner);
        home.Members.Add(homeMember1);
        home.Members.Add(homeMember2);

        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);

        IEnumerable<HomeMember> homeMembers = homeService.GetAllHomeMembers(home.Id);
        homeRepositoryMock.VerifyAll();

        Assert.AreEqual(home.Members.First(), homeMembers.First());
        Assert.AreEqual(home.Members.Last(), homeMembers.Last());
    }

    [TestMethod]
    public void GetAll_HomeDevices_Test()
    {
        var home = new Home
        {
            Devices = new List<HomeDevice>(),
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "Home Name"
        };
        var businessOwnerRole = new Role
        {
            Name = "BusinessOwner"
        };
        var businessOwner = new User
        {
            Email = "blankEmail@blank.com",
            Name = "blankName",
            Surname = "blanckSurname",
            Password = "blankPassword",
            Id = new Guid(),
            Role = businessOwnerRole
        };
        var business = new Business
        {
            BusinessOwner = businessOwner,
            Id = Guid.NewGuid(),
            Name = "bName",
            Logo = "logo",
            RUT = "111222333"
        };

        var device1 = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Device1",
            Description = "A Device",
            Business = business,
            ModelNumber = "123",
            Photos = []
        };
        var device2 = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Device2",
            Description = "A Device",
            Business = business,
            ModelNumber = "123",
            Photos = []
        };

        var homeDevice1 = new HomeDevice
        {
            Device = device1,
            Id = Guid.NewGuid(),
            Online = true,
            Name = device1.Name
        };
        var homeDevice2 = new HomeDevice
        {
            Device = device2,
            Id = Guid.NewGuid(),
            Online = false,
            Name = device2.Name
        };

        home.Devices.Add(homeDevice1);
        home.Devices.Add(homeDevice2);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);

        IEnumerable<HomeDevice> homeDevices = homeService.GetAllHomeDevices(home.Id, null);
        homeRepositoryMock.VerifyAll();

        Assert.AreEqual(home.Devices.First(), homeDevices.First());
        Assert.AreEqual(home.Devices.Last(), homeDevices.Last());
    }

    [TestMethod]
    public void Create_Person_Detecion_Notification()
    {
        var home = new Home
        {
            Devices = new List<HomeDevice>(),
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name"
        };
        var homeMember = new HomeMember(owner);
        var businessOwnerRole = new Role
        {
            Name = "BusinessOwner"
        };
        var businessOwner = new User
        {
            Email = "blankEmail@blank.com",
            Name = "blankName",
            Surname = "blanckSurname",
            Password = "blankPassword",
            Id = new Guid(),
            Role = businessOwnerRole
        };
        var business = new Business
        {
            BusinessOwner = businessOwner,
            Id = Guid.NewGuid(),
            Name = "bName",
            Logo = "logo",
            RUT = "111222333"
        };
        var securityCamera = new SecurityCamera
        {
            Name = "DeviceName",
            Business = business,
            Description = "DeviceDescription",
            Photos = [],
            ModelNumber = "a"
        };
        var homeDevice = new HomeDevice
        {
            Device = securityCamera,
            Id = Guid.NewGuid(),
            Online = true,
            Name = securityCamera.Name
        };
        var notificationPermission = new HomePermission
        {
            Id = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID),
            Name = "NotificationPermission"
        };

        var notification = new Notification
        {
            Date = DateTime.Today,
            Event = "Test",
            HomeDevice = homeDevice,
            Time = DateTime.Now,
            DetectedPerson = owner
        };
        home.Devices.Add(homeDevice);
        home.Members.Add(homeMember);

        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(owner);
        homeDeviceRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeDevice, bool>>())).Returns(homeDevice);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        homeRepositoryMock.Setup(x => x.Update(It.IsAny<Home>())).Returns(home);
        homeMemberRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeMember, bool>>())).Returns(homeMember);
        homePermissionRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomePermission, bool>>())).Returns(notificationPermission);

        homeService.CreatePersonDetectionNotification(homeDevice.Id, ownerId);

        homeMember.Notifications.Add(notification);
        homeRepositoryMock.VerifyAll();

        IEnumerable<HomeMember> homeMembers = (List<HomeMember>)homeService.GetAllHomeMembers(home.Id);

        Assert.AreEqual(homeMember.Notifications.First(), homeMembers.First().Notifications.First());
    }

    [TestMethod]
    public void Create_Movement_Detection_Notification()
    {
        var home = new Home
        {
            Devices = new List<HomeDevice>(),
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "Home Name"
        };
        var homeMember = new HomeMember(owner);
        var businessOwnerRole = new Role
        {
            Name = "BusinessOwner"
        };
        var businessOwner = new User
        {
            Email = "blankEmail@blank.com",
            Name = "blankName",
            Surname = "blanckSurname",
            Password = "blankPassword",
            Id = new Guid(),
            Role = businessOwnerRole
        };
        var business = new Business
        {
            BusinessOwner = businessOwner,
            Id = Guid.NewGuid(),
            Name = "bName",
            Logo = "logo",
            RUT = "111222333"
        };
        var securityCamera = new SecurityCamera
        {
            Name = "DeviceName",
            Business = business,
            Description = "DeviceDescription",
            Photos = [],
            ModelNumber = "a"
        };
        var homeDevice = new HomeDevice
        {
            Device = securityCamera,
            Id = Guid.NewGuid(),
            Online = true,
            Name = securityCamera.Name
        };
        var notificationPermission = new HomePermission
        {
            Id = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID),
            Name = "NotificationPermission"
        };

        var notification = new Notification
        {
            Date = DateTime.Today,
            Event = "Test",
            HomeDevice = homeDevice,
            Time = DateTime.Now
        };
        home.Devices.Add(homeDevice);
        home.Members.Add(homeMember);

        homeDeviceRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeDevice, bool>>())).Returns(homeDevice);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        homeRepositoryMock.Setup(x => x.Update(It.IsAny<Home>())).Returns(home);
        homeMemberRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeMember, bool>>())).Returns(homeMember);
        homePermissionRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomePermission, bool>>())).Returns(notificationPermission);

        homeService.CreateMovementDetectionNotification(homeDevice.Id);

        homeMember.Notifications.Add(notification);
        homeRepositoryMock.VerifyAll();

        IEnumerable<HomeMember> homeMembers = (List<HomeMember>)homeService.GetAllHomeMembers(home.Id);

        Assert.AreEqual(homeMember.Notifications.First(), homeMembers.First().Notifications.First());
    }

    [TestMethod]
    public void Create_Movement_Detection_Notification_On_Off_Device_Throws_Exception()
    {
        var home = new Home
        {
            Devices = new List<HomeDevice>(),
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name"
        };
        var homeMember = new HomeMember(owner);
        var businessOwnerRole = new Role
        {
            Name = "BusinessOwner"
        };
        var businessOwner = new User
        {
            Email = "blankEmail@blank.com",
            Name = "blankName",
            Surname = "blanckSurname",
            Password = "blankPassword",
            Id = new Guid(),
            Role = businessOwnerRole
        };
        var business = new Business
        {
            BusinessOwner = businessOwner,
            Id = Guid.NewGuid(),
            Name = "bName",
            Logo = "logo",
            RUT = "111222333"
        };
        var securityCamera = new SecurityCamera
        {
            Name = "DeviceName",
            Business = business,
            Description = "DeviceDescription",
            Photos = [],
            ModelNumber = "a"
        };
        var homeDevice = new HomeDevice
        {
            Device = securityCamera,
            Id = Guid.NewGuid(),
            Online = false,
            Name = securityCamera.Name
        };

        var notification = new Notification
        {
            Date = DateTime.Today,
            Event = "Test",
            HomeDevice = homeDevice,
            Time = DateTime.Now
        };
        home.Devices.Add(homeDevice);
        home.Members.Add(homeMember);

        homeDeviceRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeDevice, bool>>())).Returns(homeDevice);

        var ex = new HomeDeviceException("PlaceHolder");
        try
        {
            homeService.CreateMovementDetectionNotification(homeDevice.Id);
        }
        catch (Exception e)
        {
            ex = (HomeDeviceException)e;
        }

        homeRepositoryMock.VerifyAll();
        Assert.AreEqual("Device is offline", ex.Message);
    }

    [TestMethod]
    public void Create_Movement_Detection_Notification_Should_Only_Be_Added_To_HomeMember_If_He_Has_Notification_Permission()
    {
        var home = new Home
        {
            Devices = new List<HomeDevice>(),
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name"
        };
        var notificationPermission = new HomePermission
        {
            Id = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID),
            Name = "NotificationPermission"
        };

        homeRepositoryMock.Setup(x => x.Add(It.IsAny<Home>())).Returns(home);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns((Home)null);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(owner);
        homePermissionRepositoryMock.Setup(x => x.FindAll()).Returns(new List<HomePermission> { notificationPermission });

        var result = homeService.CreateHome(home, ownerId);

        var memberId = Guid.NewGuid();
        var member = new User
        {
            Email = "blankEmail1@blank.com",
            Name = "blankName1",
            Surname = "blanckSurname1",
            Password = "blankPassword",
            Id = memberId,
            Role = homeOwnerRole
        };

        homeRepositoryMock.Setup(x => x.Update(It.IsAny<Home>())).Returns(home);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(member);
        homeMemberRepositoryMock.Setup(x => x.Add(It.IsAny<HomeMember>())).Returns(new HomeMember(member));

        HomeMember homeMember = homeService.AddHomeMemberToHome(home.Id, memberId);
        home.Members.Add(homeMember);

        var businessOwnerRole = new Role
        {
            Name = "BusinessOwner"
        };
        var businessOwner = new User
        {
            Email = "blankEmail@blank.com",
            Name = "blankName",
            Surname = "blanckSurname",
            Password = "blankPassword",
            Id = new Guid(),
            Role = businessOwnerRole
        };
        var business = new Business
        {
            BusinessOwner = businessOwner,
            Id = Guid.NewGuid(),
            Name = "bName",
            Logo = "logo",
            RUT = "111222333"
        };
        var securityCamera = new SecurityCamera
        {
            Name = "DeviceName",
            Business = business,
            Description = "DeviceDescription",
            Photos = [],
            ModelNumber = "a"
        };
        var homeDevice = new HomeDevice
        {
            Device = securityCamera,
            Id = Guid.NewGuid(),
            Online = true,
            Name = securityCamera.Name
        };

        home.Devices.Add(homeDevice);

        var homeOwner = result.Members.Find(x => x.User == owner);

        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        homeDeviceRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeDevice, bool>>())).Returns(homeDevice);
        homeMemberRepositoryMock
            .SetupSequence(x => x.Find(It.IsAny<Func<HomeMember, bool>>()))
            .Returns(homeOwner)
            .Returns(homeMember);
        homePermissionRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomePermission, bool>>())).Returns(notificationPermission);

        homeService.CreateMovementDetectionNotification(homeDevice.Id);

        IEnumerable<HomeMember> homeMembers = (List<HomeMember>)homeService.GetAllHomeMembers(home.Id);
        var ownerFound = homeMembers.First(x => x.User == owner);
        var memberFound = homeMembers.First(x => x.User == member);

        Assert.IsTrue(ownerFound.Notifications.Count > 0);
        Assert.IsTrue(memberFound.Notifications.Count == 0);

        homeRepositoryMock.VerifyAll();
        userRepositoryMock.VerifyAll();
        homeRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void Register_Default_Device_To_Home_Test()
    {
        var homeId = Guid.NewGuid();
        var home = new Home
        {
            Id = homeId,
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name"
        };
        var businessOwnerRole = new Role
        {
            Name = "BusinessOwner"
        };
        var businessOwner = new User
        {
            Email = "blankEmail@blank.com",
            Name = "blankName",
            Surname = "blanckSurname",
            Password = "blankPassword",
            Id = Guid.NewGuid(),
            Role = businessOwnerRole
        };
        var business = new Business
        {
            BusinessOwner = businessOwner,
            Id = Guid.NewGuid(),
            Name = "bName",
            Logo = "logo",
            RUT = "111222333"
        };
        var deviceId = Guid.NewGuid();
        var device = new Device
        {
            Id = deviceId,
            Name = "Window sensor",
            ModelNumber = "1234",
            Description = "Window sensor for home",
            Photos = [],
            Business = business
        };
        var homeDeviceId = Guid.NewGuid();
        var homeDevice = new HomeDevice
        {
            Id = homeDeviceId,
            Online = true,
            Device = device,
            HomeId = homeId,
            Name = device.Name
        };
        home.Devices.Add(homeDevice);

        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        deviceRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Device, bool>>())).Returns(device);
        homeDeviceRepositoryMock.Setup(x => x.Add(It.IsAny<HomeDevice>())).Returns(homeDevice);

        homeService.AddDeviceToHome(homeId, deviceId);
        homeRepositoryMock.VerifyAll();
        userRepositoryMock.VerifyAll();

        Assert.IsNotNull(home.Devices);
        Assert.AreEqual(1, home.Devices.Count);
    }

    [TestMethod]
    public void Register_Device_To_Home_Throws_Exception_If_Device_Does_Not_Exist()
    {
        var homeId = Guid.NewGuid();
        var home = new Home
        {
            Id = homeId,
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name"
        };
        var deviceId = Guid.NewGuid();

        deviceRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Device, bool>>())).Returns((Device)null);

        var ex = new HomeDeviceException("PlaceHolder");
        try
        {
            homeService.AddDeviceToHome(homeId, deviceId);
        }
        catch (Exception e)
        {
            ex = (HomeDeviceException)e;
        }

        homeRepositoryMock.VerifyAll();
        Assert.AreEqual("Device Id does not match any device", ex.Message);
    }

    [TestMethod]
    public void Register_Device_To_Home_Throws_Exception_If_Home_Does_Not_Exist()
    {
        var homeId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var device = new Device
        {
            Id = deviceId,
            Name = "Window sensor",
            ModelNumber = "1234",
            Description = "Window sensor for home",
            Photos = [],
            Business = new Business
            {
                BusinessOwner = new User
                {
                    Email = "blankEmail@blank.com",
                    Name = "blankName",
                    Surname = "blanckSurname",
                    Password = "blankPassword",
                    Id = Guid.NewGuid(),
                    Role = new Role
                    {
                        Name = "BusinessOwner"
                    }
                },
                Id = Guid.NewGuid(),
                Name = "bName",
                Logo = "logo",
                RUT = "111222333"
            }
        };
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns((Home)null);
        deviceRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Device, bool>>()))
            .Returns(device);

        var ex = new HomeException("PlaceHolder");
        try
        {
            homeService.AddDeviceToHome(homeId, deviceId);
        }
        catch (Exception e)
        {
            ex = (HomeException)e;
        }

        homeRepositoryMock.VerifyAll();
        Assert.AreEqual("Home Id does not match any home", ex.Message);
    }

    [TestMethod]
    public void Get_Users_Notifications()
    {
        var owner = new User
        {
            Email = "owner@blank.com",
            Name = "ownerName",
            Surname = "ownerSurname",
            Password = "ownerPassword",
            Id = ownerId,
            Role = homeOwnerRole
        };
        var home1Id = Guid.NewGuid();
        var home1 = new Home
        {
            Id = home1Id,
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name"
        };
        var home2Id = Guid.NewGuid();
        var home2 = new Home
        {
            Id = home2Id,
            MainStreet = "Street2",
            DoorNumber = "1235",
            Latitude = "-32",
            Longitude = "11",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name"
        };
        var notificationPermission = new HomePermission
        {
            Id = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID),
            Name = "NotificationPermission"
        };

        var businessOwnerRole = new Role
        {
            Name = "BusinessOwner"
        };
        var businessOwner = new User
        {
            Email = "blankEmail@blank.com",
            Name = "blankName",
            Surname = "blanckSurname",
            Password = "blankPassword",
            Id = Guid.NewGuid(),
            Role = businessOwnerRole
        };
        var business = new Business
        {
            BusinessOwner = businessOwner,
            Id = Guid.NewGuid(),
            Name = "bName",
            Logo = "logo",
            RUT = "111222333"
        };
        var deviceId = Guid.NewGuid();
        var securityCamera = new SecurityCamera
        {
            Id = deviceId,
            Name = "Window sensor",
            ModelNumber = "1234",
            Description = "Window sensor for home",
            Photos = [],
            Business = business
        };
        var homeDeviceId = Guid.NewGuid();

        homeRepositoryMock.Setup(x => x.Add(It.IsAny<Home>())).Returns(home1);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns((Home)null);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(owner);
        homePermissionRepositoryMock.Setup(x => x.FindAll()).Returns(new List<HomePermission> { notificationPermission });
        homePermissionRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomePermission, bool>>())).Returns(notificationPermission);

        homeService.CreateHome(home1, ownerId);
        homeService.CreateHome(home2, ownerId);

        homeRepositoryMock.SetupSequence(x => x.Update(It.IsAny<Home>())).Returns(home1).Returns(home2);
        homeRepositoryMock.SetupSequence(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home1).Returns(home2);
        deviceRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Device, bool>>())).Returns(securityCamera);
        homeDeviceRepositoryMock.Setup(x => x.Add(It.IsAny<HomeDevice>())).Returns(new HomeDevice { Device = securityCamera, Id = homeDeviceId, HomeId = home1Id, Online = true, Name = securityCamera.Name });

        var homeDevice1 = homeService.AddDeviceToHome(home1Id, deviceId);
        var homeDevice2 = homeService.AddDeviceToHome(home2Id, deviceId);

        homeDeviceRepositoryMock.SetupSequence(x => x.Find(It.IsAny<Func<HomeDevice, bool>>())).Returns(homeDevice1).Returns(homeDevice2);
        homeRepositoryMock.SetupSequence(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home1).Returns(home2);
        homeMemberRepositoryMock.SetupSequence(x => x.Find(It.IsAny<Func<HomeMember, bool>>())).Returns(new HomeMember(owner)).Returns(new HomeMember(owner));

        var noti1 = homeService.CreatePersonDetectionNotification(home1.Id, ownerId);
        var noti2 = homeService.CreateMovementDetectionNotification(home2.Id);
        var homeMember1 = home1.Members.FirstOrDefault(x => x.User.Id == ownerId);
        var homeNoti1 = new HomeMemberNotification
        {
            HomeMemberId = ownerId,
            NotificationId = noti1.Id,
            Read = false,
            Notification = noti1,
            HomeMember = homeMember1
        };
        homeMember1.Notifications.Add(noti1);
        homeMember1.HomeMemberNotifications.Add(homeNoti1);
        var homeMember2 = home2.Members.FirstOrDefault(x => x.User.Id == ownerId);
        var homeNoti2 = new HomeMemberNotification
        {
            HomeMemberId = ownerId,
            NotificationId = noti2.Id,
            Read = false,
            Notification = noti2,
            HomeMember = homeMember2
        };
        homeMember2.Notifications.Add(noti2);
        homeMember2.HomeMemberNotifications.Add(homeNoti2);
        home1.Members.Clear();
        home1.Members.Add(homeMember1);
        home2.Members.Clear();
        home2.Members.Add(homeMember2);

        homeRepositoryMock.Setup(x => x.FindAll()).Returns(new List<Home> { home1, home2 });
        var expectedReturnedList = new List<HomeMember>();
        expectedReturnedList.Add(homeMember1);
        expectedReturnedList.Add(homeMember2);
        updateMultipleElementsRepositoryMock.Setup(x => x.UpdateMultiplElements(It.IsAny<List<HomeMember>>())).Returns(expectedReturnedList);

        var notifications = homeService.GetUsersNotifications(owner);
        Assert.IsTrue(notifications.Count == 2);
    }

    [TestMethod]
    public void GetAll_Users_Homes_Test()
    {
        var member1Id = Guid.NewGuid();
        var member1 = new User
        {
            Email = "blankEmail1@blank.com",
            Name = "blankName1",
            Surname = "blanckSurname1",
            Password = "blankPassword",
            Id = member1Id,
            Role = homeOwnerRole
        };
        var member2 = new User
        {
            Email = "blankEmail2@blank.com",
            Name = "blankName2",
            Surname = "blanckSurname2",
            Password = "blankPassword",
            Id = new Guid(),
            Role = homeOwnerRole
        };
        var homeOwner = new HomeMember(owner);
        var homeMember1 = new HomeMember(member1);
        var homeMember2 = new HomeMember(member2);

        var home1 = new Home
        {
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name"
        };
        var home2 = new Home
        {
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name"
        };
        home1.Members.Add(homeOwner);
        home1.Members.Add(homeMember1);
        home1.Members.Add(homeMember2);

        home2.Members.Add(homeMember1);

        var homesFromMember1 = new List<Home> { home1, home2 };

        homesFromUserRepositoryMock.Setup(x => x.GetAllHomesByUserId(member1Id)).Returns(homesFromMember1);

        var homes = homeService.GetAllHomesByUserId(member1Id);

        Assert.AreEqual(homesFromMember1.Count, homes.Count());

        CollectionAssert.AreEqual(homesFromMember1, homes.ToList());
    }

    [TestMethod]
    public void Create_Opened_Window_Notification_Test()
    {
        var home = new Home
        {
            Devices = new List<HomeDevice>(),
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name"
        };
        var homeMember = new HomeMember(owner);
        var businessOwnerRole = new Role
        {
            Name = "BusinessOwner"
        };
        var businessOwner = new User
        {
            Email = "blankEmail@blank.com",
            Name = "blankName",
            Surname = "blanckSurname",
            Password = "blankPassword",
            Id = new Guid(),
            Role = businessOwnerRole
        };
        var business = new Business
        {
            BusinessOwner = businessOwner,
            Id = Guid.NewGuid(),
            Name = "bName",
            Logo = "logo",
            RUT = "111222333"
        };
        var windowSensor = new Device
        {
            Type = "Window Sensor",
            Name = "DeviceName",
            Business = business,
            Description = "DeviceDescription",
            Photos = [],
            ModelNumber = "a"
        };
        var homeDevice = new HomeDevice
        {
            Device = windowSensor,
            Id = Guid.NewGuid(),
            Online = true,
            Name = windowSensor.Name
        };
        var notificationPermission = new HomePermission
        {
            Id = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID),
            Name = "NotificationPermission"
        };

        homeDevice.IsOpen = false;
        var notification = new Notification
        {
            Date = DateTime.Today,
            Event = "Test",
            HomeDevice = homeDevice,
            Time = DateTime.Now
        };
        home.Devices.Add(homeDevice);
        home.Members.Add(homeMember);

        homeDeviceRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeDevice, bool>>())).Returns(homeDevice);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        homeRepositoryMock.Setup(x => x.Update(It.IsAny<Home>())).Returns(home);
        homeMemberRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeMember, bool>>())).Returns(homeMember);
        homePermissionRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomePermission, bool>>())).Returns(notificationPermission);

        homeService.CreateOpenCloseWindowNotification(homeDevice.Id);

        homeMember.Notifications.Add(notification);
        homeRepositoryMock.VerifyAll();

        IEnumerable<HomeMember> homeMembers = (List<HomeMember>)homeService.GetAllHomeMembers(home.Id);

        Assert.AreEqual(homeMember.Notifications.First(), homeMembers.First().Notifications.First());
    }

    [TestMethod]
    public void Create_Closed_Window_Notification_Test()
    {
        var home = new Home
        {
            Devices = new List<HomeDevice>(),
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "Home Name"
        };
        var homeMember = new HomeMember(owner);
        var businessOwnerRole = new Role
        {
            Name = "BusinessOwner"
        };
        var businessOwner = new User
        {
            Email = "blankEmail@blank.com",
            Name = "blankName",
            Surname = "blanckSurname",
            Password = "blankPassword",
            Id = new Guid(),
            Role = businessOwnerRole
        };
        var business = new Business
        {
            BusinessOwner = businessOwner,
            Id = Guid.NewGuid(),
            Name = "bName",
            Logo = "logo",
            RUT = "111222333"
        };
        var windowSensor = new Device
        {
            Type = "Window Sensor",
            Name = "DeviceName",
            Business = business,
            Description = "DeviceDescription",
            Photos = [],
            ModelNumber = "a"
        };
        var homeDevice = new HomeDevice
        {
            Device = windowSensor,
            Id = Guid.NewGuid(),
            Online = true,
            Name = windowSensor.Name
        };
        var notificationPermission = new HomePermission
        {
            Id = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID),
            Name = "NotificationPermission"
        };

        homeDevice.IsOpen = true;
        var notification = new Notification
        {
            Date = DateTime.Today,
            Event = "Test",
            HomeDevice = homeDevice,
            Time = DateTime.Now
        };
        home.Devices.Add(homeDevice);
        home.Members.Add(homeMember);

        homeDeviceRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeDevice, bool>>())).Returns(homeDevice);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        homeRepositoryMock.Setup(x => x.Update(It.IsAny<Home>())).Returns(home);
        homeMemberRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeMember, bool>>())).Returns(homeMember);
        homePermissionRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomePermission, bool>>())).Returns(notificationPermission);

        homeService.CreateOpenCloseWindowNotification(homeDevice.Id);

        homeMember.Notifications.Add(notification);
        homeRepositoryMock.VerifyAll();

        IEnumerable<HomeMember> homeMembers = (List<HomeMember>)homeService.GetAllHomeMembers(home.Id);

        Assert.AreEqual(homeMember.Notifications.First(), homeMembers.First().Notifications.First());
    }

    [TestMethod]
    public void Create_Window_Sensor_Notification_On_Off_Device_Throws_Exception()
    {
        var home = new Home
        {
            Devices = new List<HomeDevice>(),
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "Home Name"
        };
        var homeMember = new HomeMember(owner);
        var businessOwnerRole = new Role
        {
            Name = "BusinessOwner"
        };
        var businessOwner = new User
        {
            Email = "blankEmail@blank.com",
            Name = "blankName",
            Surname = "blanckSurname",
            Password = "blankPassword",
            Id = new Guid(),
            Role = businessOwnerRole
        };
        var business = new Business
        {
            BusinessOwner = businessOwner,
            Id = Guid.NewGuid(),
            Name = "bName",
            Logo = "logo",
            RUT = "111222333"
        };
        var windowSensor = new Device
        {
            Type = "Window Sensor",
            Name = "DeviceName",
            Business = business,
            Description = "DeviceDescription",
            Photos = [],
            ModelNumber = "a"
        };
        var homeDevice = new HomeDevice
        {
            Device = windowSensor,
            Id = Guid.NewGuid(),
            Online = false,
            Name = windowSensor.Name
        };

        var notification = new Notification
        {
            Date = DateTime.Today,
            Event = "Test",
            HomeDevice = homeDevice,
            Time = DateTime.Now
        };
        home.Devices.Add(homeDevice);
        home.Members.Add(homeMember);

        homeDeviceRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeDevice, bool>>())).Returns(homeDevice);

        var ex = new HomeDeviceException("PlaceHolder");
        try
        {
            homeService.CreateOpenCloseWindowNotification(homeDevice.Id);
        }
        catch (Exception e)
        {
            ex = (HomeDeviceException)e;
        }

        homeRepositoryMock.VerifyAll();
        Assert.AreEqual("Device is offline", ex.Message);
    }

    [TestMethod]
    public void Create_Window_Sensor_Notification_Should_Only_Be_Added_To_HomeMember_If_He_Has_Notification_Permission()
    {
        var home = new Home
        {
            Devices = new List<HomeDevice>(),
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "Home Name"
        };
        var notificationPermission = new HomePermission
        {
            Id = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID),
            Name = "NotificationPermission"
        };

        homeRepositoryMock.Setup(x => x.Add(It.IsAny<Home>())).Returns(home);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns((Home)null);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(owner);
        homePermissionRepositoryMock.Setup(x => x.FindAll()).Returns(new List<HomePermission> { notificationPermission });

        var result = homeService.CreateHome(home, ownerId);

        var memberId = Guid.NewGuid();
        var member = new User
        {
            Email = "blankEmail1@blank.com",
            Name = "blankName1",
            Surname = "blanckSurname1",
            Password = "blankPassword",
            Id = memberId,
            Role = homeOwnerRole
        };

        homeMemberRepositoryMock.Setup(x => x.Add(It.IsAny<HomeMember>())).Returns(new HomeMember(member));
        homeRepositoryMock.Setup(x => x.Update(It.IsAny<Home>())).Returns(home);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(member);

        HomeMember homeMember = homeService.AddHomeMemberToHome(home.Id, memberId);
        home.Members.Add(homeMember);

        var businessOwnerRole = new Role
        {
            Name = "BusinessOwner"
        };
        var businessOwner = new User
        {
            Email = "blankEmail@blank.com",
            Name = "blankName",
            Surname = "blanckSurname",
            Password = "blankPassword",
            Id = new Guid(),
            Role = businessOwnerRole
        };
        var business = new Business
        {
            BusinessOwner = businessOwner,
            Id = Guid.NewGuid(),
            Name = "bName",
            Logo = "logo",
            RUT = "111222333"
        };
        var windowSensor = new Device
        {
            Type = "Window Sensor",
            Name = "DeviceName",
            Business = business,
            Description = "DeviceDescription",
            Photos = [],
            ModelNumber = "a"
        };
        var homeDevice = new HomeDevice
        {
            Device = windowSensor,
            Id = Guid.NewGuid(),
            Online = true,
            Name = windowSensor.Name
        };

        home.Devices.Add(homeDevice);

        var homeOwner = result.Members.Find(x => x.User == owner);

        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        homeDeviceRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeDevice, bool>>())).Returns(homeDevice);
        homeMemberRepositoryMock
            .SetupSequence(x => x.Find(It.IsAny<Func<HomeMember, bool>>()))
            .Returns(homeOwner)
            .Returns(homeMember);
        homePermissionRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomePermission, bool>>())).Returns(notificationPermission);

        homeService.CreateOpenCloseWindowNotification(homeDevice.Id);

        IEnumerable<HomeMember> homeMembers = (List<HomeMember>)homeService.GetAllHomeMembers(home.Id);
        var ownerFound = homeMembers.First(x => x.User == owner);
        var memberFound = homeMembers.First(x => x.User == member);

        Assert.IsTrue(ownerFound.Notifications.Count > 0);
        Assert.IsTrue(memberFound.Notifications.Count == 0);
        homeRepositoryMock.VerifyAll();
        userRepositoryMock.VerifyAll();
        homeRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void Create_Repeated_Adress_Home_Throws_Exception_Test()
    {
        var home = new Home
        {
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "Home Name"
        };
        var home2 = new Home
        {
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 3,
            Owner = owner,
            Name = "Home Name"
        };
        homeRepositoryMock.Setup(u => u.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        userRepositoryMock.Setup(u => u.Find(It.IsAny<Func<User, bool>>())).Returns(owner);
        Exception exception = null;

        try
        {
            homeService.CreateHome(home, ownerId);
        }
        catch (Exception e)
        {
            exception = e;
        }

        homeRepositoryMock.VerifyAll();
        userRepositoryMock.VerifyAll();
        Assert.IsInstanceOfType(exception, typeof(HomeException));
        Assert.AreEqual("Home already exists", exception.Message);
    }

    [TestMethod]
    public void Register_Repeated_HomeMemberToHouse_Throws_Exception_Test()
    {
        var home = new Home
        {
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "Home Name"
        };
        var memberId = Guid.NewGuid();
        var member = new User
        {
            Email = "blankEmail1@blank.com",
            Name = "blankName1",
            Surname = "blanckSurname1",
            Password = "blankPassword",
            Id = memberId,
            Role = homeOwnerRole
        };
        var homeMember = new HomeMember(member);

        homeMemberRepositoryMock.Setup(x => x.Add(It.IsAny<HomeMember>())).Returns(homeMember);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(member);
        Exception exception = null;

        try
        {
            homeService.AddHomeMemberToHome(home.Id, memberId);
            home.Members.Add(homeMember);
            homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
            homeService.AddHomeMemberToHome(home.Id, memberId);
        }
        catch (Exception e)
        {
            exception = e;
        }

        homeRepositoryMock.VerifyAll();
        userRepositoryMock.VerifyAll();
        Assert.IsInstanceOfType(exception, typeof(HomeException));
        Assert.AreEqual("User is already in home", exception.Message);
    }

    [TestMethod]
    public void Register_HomeMember_To_Full_House_Throws_Exception_Test()
    {
        var home = new Home
        {
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 1,
            Owner = owner,
            Name = "Home Name"
        };
        var member1Id = Guid.NewGuid();
        var member1 = new User
        {
            Email = "blankEmail1@blank.com",
            Name = "blankName1",
            Surname = "blanckSurname1",
            Password = "blankPassword",
            Id = member1Id,
            Role = homeOwnerRole
        };
        var homeMember = new HomeMember(member1);

        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(member1);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(owner);
        homeMemberRepositoryMock.Setup(x => x.Add(It.IsAny<HomeMember>())).Returns(homeMember);

        Exception exception = null;

        try
        {
            homeService.AddHomeMemberToHome(home.Id, member1Id);
            home.Members.Add(homeMember);
            homeService.AddHomeMemberToHome(home.Id, ownerId);
        }
        catch (Exception e)
        {
            exception = e;
        }

        homeRepositoryMock.VerifyAll();
        userRepositoryMock.VerifyAll();
        Assert.IsInstanceOfType(exception, typeof(HomeException));
        Assert.AreEqual("Home has no more space", exception.Message);
    }

    [TestMethod]
    public void Update_HomeDevice_Name_Test()
    {
        var home = new Home
        {
            Devices = new List<HomeDevice>(),
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "Home Name"
        };
        var homeMember = new HomeMember(owner);
        var businessOwnerRole = new Role
        {
            Name = "BusinessOwner"
        };
        var businessOwner = new User
        {
            Email = "blankEmail@blank.com",
            Name = "blankName",
            Surname = "blanckSurname",
            Password = "blankPassword",
            Id = new Guid(),
            Role = businessOwnerRole
        };
        var business = new Business
        {
            BusinessOwner = businessOwner,
            Id = Guid.NewGuid(),
            Name = "bName",
            Logo = "logo",
            RUT = "111222333"
        };
        var device = new Device
        {
            Name = "DeviceName",
            Business = business,
            Description = "DeviceDescription",
            Photos = [],
            ModelNumber = "a"
        };
        var homeDevice = new HomeDevice
        {
            Device = device,
            Id = Guid.NewGuid(),
            Online = false,
            Name = device.Name
        };
        var newName = "NewName";

        home.Devices.Add(homeDevice);
        home.Members.Add(homeMember);

        homeDeviceRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeDevice, bool>>())).Returns(homeDevice);
        homeDeviceRepositoryMock.Setup(x => x.Update(It.IsAny<HomeDevice>())).Returns(homeDevice);

        homeService.UpdateHomeDeviceName(homeDevice.Id, newName);

        homeRepositoryMock.VerifyAll();

        Assert.AreEqual(newName, homeDevice.Name);
    }

    [TestMethod]
    public void Update_Home_Name_Test()
    {
        var home = new Home
        {
            Devices = new List<HomeDevice>(),
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "Home Name"
        };
        var newName = "NewName";

        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        homeRepositoryMock.Setup(x => x.Update(It.IsAny<Home>())).Returns(home);

        homeService.UpdateHomeName(home.Id, newName);

        homeRepositoryMock.VerifyAll();

        Assert.AreEqual(newName, home.Name);
    }

    [TestMethod]
    public void Create_Home_With_Invalid_User_Throws_Exception_Test()
    {
        var home = new Home
        {
            Devices = new List<HomeDevice>(),
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = null,
            Name = "Home Name"
        };
        User invalidUser = home.Owner;
        userRepositoryMock.Setup(u => u.Find(It.IsAny<Func<User, bool>>())).Returns(invalidUser);
        Exception exception = null;

        try
        {
            homeService.CreateHome(home, Guid.NewGuid());
        }
        catch (Exception e)
        {
            exception = e;
        }

        userRepositoryMock.VerifyAll();
        Assert.IsInstanceOfType(exception, typeof(HomeException));
        Assert.AreEqual("User Id does not match any user", exception.Message);
    }

    [TestMethod]
    public void GetAll_HomeDevices_HomeNotFound_Throws_Exception_Test()
    {
        Home home = null;
        homeRepositoryMock.Setup(h => h.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        Exception exception = null;

        try
        {
            homeService.GetAllHomeDevices(Guid.NewGuid(), null);
        }
        catch (Exception e)
        {
            exception = e;
        }

        homeRepositoryMock.VerifyAll();
        Assert.IsInstanceOfType(exception, typeof(HomeException));
        Assert.AreEqual("Home Id does not match any home", exception.Message);
    }

    [TestMethod]
    public void GetAll_HomeDevices_DevicesNotFound_Throws_Exception_Test()
    {
        var home = new Home
        {
            Devices = new List<HomeDevice>(),
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = null,
            Name = "Home Name"
        };
        home.Devices = null;
        homeRepositoryMock.Setup(h => h.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        Exception exception = null;

        try
        {
            homeService.GetAllHomeDevices(home.Id, null);
        }
        catch (Exception e)
        {
            exception = e;
        }

        homeRepositoryMock.VerifyAll();
        Assert.IsInstanceOfType(exception, typeof(HomeException));
        Assert.AreEqual("Home devices was not found", exception.Message);
    }

    [TestMethod]
    public void GetAllHomeMembers_HomeMembersNotFound_Throws_Exception_Test()
    {
        var home = new Home
        {
            Devices = new List<HomeDevice>(),
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = null,
            Name = "Home Name"
        };
        home.Members = null;

        homeRepositoryMock.Setup(h => h.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        Exception exception = null;

        try
        {
            homeService.GetAllHomeMembers(home.Id);
        }
        catch (Exception e)
        {
            exception = e;
        }

        homeRepositoryMock.VerifyAll();
        Assert.IsInstanceOfType(exception, typeof(HomeException));
        Assert.AreEqual("Home Id does not match any home", exception.Message);
    }

    [TestMethod]
    public void GetAllHomeMembers_HomeNotFound_Throws_Exception_Test()
    {
        Home home = null;
        homeRepositoryMock.Setup(h => h.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        Exception exception = null;

        try
        {
            homeService.GetAllHomeMembers(Guid.NewGuid());
        }
        catch (Exception e)
        {
            exception = e;
        }

        homeRepositoryMock.VerifyAll();
        Assert.IsInstanceOfType(exception, typeof(HomeException));
        Assert.AreEqual("Home Id does not match any home", exception.Message);
    }

    [TestMethod]
    public void CreateHome_EmptyMainStreet_Throws_Exception_Test()
    {
        var home = new Home
        {
            Devices = new List<HomeDevice>(),
            Id = Guid.NewGuid(),
            MainStreet = string.Empty,
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = null,
            Name = "Home Name"
        };

        Exception exception = null;

        try
        {
            homeService.CreateHome(home, Guid.NewGuid());
        }
        catch (Exception e)
        {
            exception = e;
        }

        Assert.IsInstanceOfType(exception, typeof(HomeArgumentException));
        Assert.AreEqual("Invalid main street, cannot be empty", exception.Message);
    }

    [TestMethod]
    public void CreateHome_EmptyDoorNumber_Throws_Exception_Test()
    {
        var home = new Home
        {
            Devices = new List<HomeDevice>(),
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = string.Empty,
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = null,
            Name = "Home Name"
        };

        Exception exception = null;

        try
        {
            homeService.CreateHome(home, Guid.NewGuid());
        }
        catch (Exception e)
        {
            exception = e;
        }

        Assert.IsInstanceOfType(exception, typeof(HomeArgumentException));
        Assert.AreEqual("Invalid door number, cannot be empty", exception.Message);
    }

    [TestMethod]
    public void CreateHome_EmptyLatitude_Throws_Exception_Test()
    {
        var home = new Home
        {
            Devices = new List<HomeDevice>(),
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "0000",
            Latitude = string.Empty,
            Longitude = "31",
            MaxMembers = 6,
            Owner = null,
            Name = "Home Name"
        };

        Exception exception = null;

        try
        {
            homeService.CreateHome(home, Guid.NewGuid());
        }
        catch (Exception e)
        {
            exception = e;
        }

        Assert.IsInstanceOfType(exception, typeof(HomeArgumentException));
        Assert.AreEqual("Invalid latitude, cannot be empty", exception.Message);
    }

    [TestMethod]
    public void CreateHome_EmptyLongitude_Throws_Exception_Test()
    {
        var home = new Home
        {
            Devices = new List<HomeDevice>(),
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "0000",
            Latitude = "31",
            Longitude = string.Empty,
            MaxMembers = 6,
            Owner = null,
            Name = "Home Name"
        };

        Exception exception = null;

        try
        {
            homeService.CreateHome(home, Guid.NewGuid());
        }
        catch (Exception e)
        {
            exception = e;
        }

        Assert.IsInstanceOfType(exception, typeof(HomeArgumentException));
        Assert.AreEqual("Invalid longitude, cannot be empty", exception.Message);
    }

    [TestMethod]
    public void CreateHome_InvalidMaxMembers_Throws_Exception_Test()
    {
        var home = new Home
        {
            Devices = new List<HomeDevice>(),
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "0000",
            Latitude = "31",
            Longitude = "31",
            MaxMembers = 0,
            Owner = null,
            Name = "Home Name"
        };

        Exception exception = null;

        try
        {
            homeService.CreateHome(home, Guid.NewGuid());
        }
        catch (Exception e)
        {
            exception = e;
        }

        Assert.IsInstanceOfType(exception, typeof(HomeArgumentException));
        Assert.AreEqual("Invalid max members, must be at least 1", exception.Message);
    }

    [TestMethod]
    public void GetAllHomesByUserId_UserIdDoesNotCorrespondToAnyHouse_Throws_Exception_Test()
    {
        var id = Guid.NewGuid();

        homesFromUserRepositoryMock.Setup(x => x.GetAllHomesByUserId(id)).Returns((List<Home>)null);
        Exception exception = null;

        try
        {
            homeService.GetAllHomesByUserId(id);
        }
        catch (Exception e)
        {
            exception = e;
        }

        homesFromUserRepositoryMock.VerifyAll();
        Assert.IsInstanceOfType(exception, typeof(UserException));
        Assert.AreEqual("This user id does not correspond to any house", exception.Message);
    }

    [TestMethod]
    public void HasPermission_HomeNotFound_Throws_Exception_Test()
    {
        Home home = null;
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var id3 = Guid.NewGuid();
        homeRepositoryMock.Setup(h => h.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        Exception exception = null;

        try
        {
            homeService.HasPermission(id1, id2, id3);
        }
        catch (Exception e)
        {
            exception = e;
        }

        homeRepositoryMock.VerifyAll();
        Assert.IsInstanceOfType(exception, typeof(HomeException));
        Assert.AreEqual("Home Id does not match any home", exception.Message);
    }

    [TestMethod]
    public void HasPermission_User_Is_Not_HomeMember_Throws_Exception_Test()
    {
        var home = new Home
        {
            Devices = new List<HomeDevice>(),
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "0000",
            Latitude = "31",
            Longitude = "31",
            MaxMembers = 2,
            Owner = owner,
            Name = "Home Name"
        };
        var notMember = new User
        {
            Email = "blankEmail1@blank.com",
            Name = "blankName1",
            Surname = "blanckSurname1",
            Password = "blankPassword",
            Id = Guid.NewGuid(),
            Role = homeOwnerRole
        };
        homeRepositoryMock.Setup(h => h.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        Exception exception = null;

        try
        {
            homeService.HasPermission(Guid.NewGuid(), home.Id, Guid.NewGuid());
        }
        catch (Exception e)
        {
            exception = e;
        }

        homeRepositoryMock.VerifyAll();
        Assert.IsInstanceOfType(exception, typeof(HomeException));
        Assert.AreEqual("User is not a member of this home", exception.Message);
    }

    [TestMethod]
    public void HasPermission_HomePermission_Not_Found_Throws_Exception_Test()
    {
        var home = new Home
        {
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name"
        };
        var memberId = Guid.NewGuid();
        var member = new User
        {
            Email = "blankEmail1@blank.com",
            Name = "blankName1",
            Surname = "blanckSurname1",
            Password = "blankPassword",
            Id = memberId,
            Role = homeOwnerRole
        };
        home.Members.Add(new HomeMember(member));
        var permissionId = Guid.NewGuid();

        homeRepositoryMock.Setup(h => h.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        homePermissionRepositoryMock.Setup(hp => hp.Find(It.IsAny<Func<HomePermission, bool>>())).Returns((HomePermission)null);

        Exception exception = null;

        try
        {
            homeService.HasPermission(memberId, home.Id, permissionId);
        }
        catch (Exception e)
        {
            exception = e;
        }

        homeRepositoryMock.VerifyAll();
        homePermissionRepositoryMock.VerifyAll();
        Assert.IsInstanceOfType(exception, typeof(HomeException));
        Assert.AreEqual("HomePermission was not found", exception.Message);
    }

    [TestMethod]
    public void HasPermission_UserHasPermission_Test()
    {
        var home = new Home
        {
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name"
        };
        var memberId = Guid.NewGuid();
        var member = new User
        {
            Email = "blankEmail1@blank.com",
            Name = "blankName1",
            Surname = "blanckSurname1",
            Password = "blankPassword",
            Id = memberId,
            Role = homeOwnerRole
        };
        home.Members.Add(new HomeMember(member));
        var permissionId = Guid.NewGuid();

        var homePermission = new HomePermission
        {
            HomeMembers = home.Members,
            Name = "Permiso",
            Id = permissionId
        };
        home.Members.First().HomePermissions.Add(homePermission);

        homeRepositoryMock.Setup(h => h.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        homePermissionRepositoryMock.Setup(hp => hp.Find(It.IsAny<Func<HomePermission, bool>>())).Returns(homePermission);

        var result = homeService.HasPermission(memberId, home.Id, permissionId);

        homeRepositoryMock.VerifyAll();
        homePermissionRepositoryMock.VerifyAll();
        Assert.IsTrue(result, "The user should have the permission");
    }

    [TestMethod]
    public void HasPermission_UserDoesNotHavePermission_Returns_False_Test()
    {
        var home = new Home
        {
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name"
        };
        var memberId = Guid.NewGuid();
        var member = new User
        {
            Email = "blankEmail1@blank.com",
            Name = "blankName1",
            Surname = "blanckSurname1",
            Password = "blankPassword",
            Id = memberId,
            Role = homeOwnerRole
        };
        home.Members.Add(new HomeMember(member));
        var permissionId = Guid.NewGuid();

        var differentPermission = new HomePermission
        {
            Id = Guid.NewGuid(),
            Name = "different p"
        };
        var homePermission = new HomePermission
        {
            Id = permissionId,
            Name = "permission"
        };
        home.Members.First().HomePermissions.Add(differentPermission);

        homeRepositoryMock.Setup(h => h.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        homePermissionRepositoryMock.Setup(hp => hp.Find(It.IsAny<Func<HomePermission, bool>>())).Returns(homePermission);

        var result = homeService.HasPermission(memberId, home.Id, permissionId);

        homeRepositoryMock.VerifyAll();
        homePermissionRepositoryMock.VerifyAll();
        Assert.IsFalse(result, "The user should not have the permission");
    }

    [TestMethod]
    public void HasPermission_HomeMember_Not_Found_Throws_Exception_Test()
    {
        var homeMemberId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();

        HomeMember homeMember = null;
        var homePermission = new HomePermission
        {
            Id = permissionId,
            Name = "Permission"
        };
        homeMemberRepositoryMock.Setup(h => h.Find(It.IsAny<Func<HomeMember, bool>>())).Returns(homeMember);
        homePermissionRepositoryMock.Setup(hp => hp.Find(It.IsAny<Func<HomePermission, bool>>())).Returns(homePermission);

        Exception exception = null;

        try
        {
            homeService.HasPermission(homeMemberId, permissionId);
        }
        catch (Exception e)
        {
            exception = e;
        }

        homeMemberRepositoryMock.VerifyAll();
        Assert.IsInstanceOfType(exception, typeof(HomeException));
        Assert.AreEqual("Home Member Id does not match any home member", exception.Message);
    }

    [TestMethod]
    public void HasPermission_HomePermission_Does_Not_Match_Any_Throws_Exception_Test()
    {
        var homeMemberId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();

        var homeMember = new HomeMember
        {
            HomeMemberId = homeMemberId,
            HomePermissions = new List<HomePermission>()
        };

        HomePermission homePermission = null;

        homeMemberRepositoryMock.Setup(h => h.Find(It.IsAny<Func<HomeMember, bool>>())).Returns(homeMember);
        homePermissionRepositoryMock.Setup(hp => hp.Find(It.IsAny<Func<HomePermission, bool>>())).Returns(homePermission);

        Exception exception = null;

        try
        {
            homeService.HasPermission(homeMemberId, permissionId);
        }
        catch (Exception e)
        {
            exception = e;
        }

        homeMemberRepositoryMock.VerifyAll();
        homePermissionRepositoryMock.VerifyAll();
        Assert.IsInstanceOfType(exception, typeof(HomeException));
        Assert.AreEqual("Home Permission Id does not match any home permission", exception.Message);
    }

    [TestMethod]
    public void Create_Detected_Person_Notification_Person_Not_Detected_Test()
    {
        var home = new Home
        {
            Devices = new List<HomeDevice>(),
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name"
        };
        var businessOwnerRole = new Role
        {
            Name = "BusinessOwner"
        };
        var businessOwner = new User
        {
            Email = "blankEmail@blank.com",
            Name = "blankName",
            Surname = "blanckSurname",
            Password = "blankPassword",
            Id = new Guid(),
            Role = businessOwnerRole
        };
        var business = new Business
        {
            BusinessOwner = businessOwner,
            Id = Guid.NewGuid(),
            Name = "bName",
            Logo = "logo",
            RUT = "111222333"
        };
        var securityCamera = new SecurityCamera
        {
            Name = "DeviceName",
            Business = business,
            Description = "DeviceDescription",
            Photos = [],
            ModelNumber = "a"
        };
        var homeDevice = new HomeDevice
        {
            Device = securityCamera,
            Id = Guid.NewGuid(),
            Online = true,
            Name = securityCamera.Name
        };
        var notificationPermission = new HomePermission
        {
            Id = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID),
            Name = "NotificationPermission"
        };
        var detectedPersonId = Guid.NewGuid();
        User detectedPerson = null;
        HomeMember member = null;
        var notification = new Notification
        {
            Date = DateTime.Today,
            Event = "Undetected person",
            HomeDevice = homeDevice,
            Time = DateTime.Now,
            DetectedPerson = detectedPerson
        };
        home.Devices.Add(homeDevice);

        homeDeviceRepositoryMock.Setup(hd => hd.Find(It.IsAny<Func<HomeDevice, bool>>())).Returns(homeDevice);
        userRepositoryMock.Setup(u => u.Find(It.IsAny<Func<User, bool>>())).Returns(detectedPerson);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        homeRepositoryMock.Setup(x => x.Update(It.IsAny<Home>())).Returns(home);
        homeMemberRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeMember, bool>>())).Returns(member);
        homePermissionRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomePermission, bool>>())).Returns(notificationPermission);

        var result = homeService.CreatePersonDetectionNotification(homeDevice.Id, detectedPersonId);

        userRepositoryMock.VerifyAll();
        Assert.IsNotNull(result);
        Assert.AreEqual("Undetected person", result.Event);
        Assert.AreEqual(homeDevice, result.HomeDevice);
        Assert.AreEqual(DateTime.Today, result.Date);
        Assert.IsNull(result.DetectedPerson);
    }

    [TestMethod]
    public void Create_PersonDetectionNotification_AddsNotificationToAllMembers_Test()
    {
        var home = new Home
        {
            Devices = new List<HomeDevice>(),
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name"
        };
        var businessOwnerRole = new Role
        {
            Name = "BusinessOwner"
        };
        var businessOwner = new User
        {
            Email = "blankEmail@blank.com",
            Name = "blankName",
            Surname = "blanckSurname",
            Password = "blankPassword",
            Id = new Guid(),
            Role = businessOwnerRole
        };
        var business = new Business
        {
            BusinessOwner = businessOwner,
            Id = Guid.NewGuid(),
            Name = "bName",
            Logo = "logo",
            RUT = "111222333"
        };
        var securityCamera = new SecurityCamera
        {
            Name = "DeviceName",
            Business = business,
            Description = "DeviceDescription",
            Photos = [],
            ModelNumber = "a"
        };
        var homeDevice = new HomeDevice
        {
            Device = securityCamera,
            Id = Guid.NewGuid(),
            Online = true,
            Name = securityCamera.Name
        };
        var detectedPersonId = Guid.NewGuid();
        User detectedPerson = null;
        HomeMember member = null;
        var notification = new Notification
        {
            Date = DateTime.Today,
            Event = "Undetected person",
            HomeDevice = homeDevice,
            Time = DateTime.Now,
            DetectedPerson = detectedPerson
        };
        home.Devices.Add(homeDevice);
        var notificationPermission = new HomePermission
        {
            Id = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID),
            Name = "NotificationPermission"
        };
        var permission = new HomePermission
        {
            Id = Guid.NewGuid(),
            Name = "A"
        };
        var homeMembers = new List<HomeMember>
        {
            new HomeMember
            {
                HomeMemberId = Guid.NewGuid(),
                Notifications = new List<Notification>{notification},
                HomePermissions = new List<HomePermission> {permission}
            },
            new HomeMember
            {
                HomeMemberId = Guid.NewGuid(),
                Notifications = new List<Notification>{notification},
                HomePermissions = new List<HomePermission>{permission}
            },
            new HomeMember
            {
                HomeMemberId = Guid.NewGuid(),
                Notifications = new List<Notification>{notification},
                HomePermissions = new List<HomePermission>{permission}
            }
        };
        home.Members = homeMembers;

        homeDeviceRepositoryMock.Setup(hd => hd.Find(It.IsAny<Func<HomeDevice, bool>>())).Returns(homeDevice);
        homeRepositoryMock.Setup(h => h.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        homeMemberRepositoryMock.Setup(hm => hm.Find(It.IsAny<Func<HomeMember, bool>>())).Returns(homeMembers.First());
        homePermissionRepositoryMock.Setup(hp => hp.Find(It.IsAny<Func<HomePermission, bool>>())).Returns(notificationPermission);
        homeRepositoryMock.Setup(x => x.Update(It.IsAny<Home>())).Returns(home);
        userRepositoryMock.Setup(u => u.Find(It.IsAny<Func<User, bool>>())).Returns(detectedPerson);

        var result = homeService.CreatePersonDetectionNotification(homeDevice.Id, detectedPersonId);
        foreach (var homeMember in home.Members)
        {
            homeMember.Notifications.Add(notification);
        }

        homeRepositoryMock.VerifyAll();

        foreach (var homeMember in home.Members)
        {
            var addedNotification = homeMember.Notifications.FirstOrDefault(n =>
                n.Event == result.Event &&
                n.HomeDevice == result.HomeDevice &&
                n.Date == result.Date &&
                n.DetectedPerson == result.DetectedPerson);

            Assert.IsNotNull(addedNotification, $"Notification was not added to member {homeMember.HomeMemberId}");
        }

        Assert.AreEqual("Undetected person", result.Event);
        Assert.AreEqual(homeDevice, result.HomeDevice);
        Assert.AreEqual(DateTime.Today, result.Date);
        Assert.IsNull(result.DetectedPerson);
    }

    [TestMethod]
    public void Add_HomeMember_To_Home_UserNotFound_Throws_Exception()
    {
        var homeId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        User user = null;

        userRepositoryMock.Setup(u => u.Find(It.IsAny<Func<User, bool>>())).Returns(user);

        Exception exception = null;
        try
        {
            homeService.AddHomeMemberToHome(homeId, userId);
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(HomeException));
        Assert.AreEqual("User Id does not match any user", exception.Message);
    }

    [TestMethod]
    public void Update_HomePermissions_Of_HomeMember_NotFound_ThrowsHomeException()
    {
        var homeMemberId = Guid.NewGuid();
        var permissions = new List<HomePermission>();
        HomeMember member = null;

        homeMemberRepositoryMock.Setup(hm => hm.Find(It.IsAny<Func<HomeMember, bool>>())).Returns(member);

        Exception exception = null;
        try
        {
            homeService.UpdateHomePermissionsOfHomeMember(homeMemberId, permissions, ownerId);
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(HomeException));
        Assert.AreEqual("Owner was not found", exception.Message);
    }

    [TestMethod]
    public void Create_MovementDetectionNotification_HomeNotFound_ThrowsHomeException()
    {
        var homeDeviceId = Guid.NewGuid();
        var securityCamera = new SecurityCamera
        {
            Description = "test",
            ModelNumber = "test",
            Name = "test",
            Photos = [],
            Business = new Business
            {
                BusinessOwner = new User
                {
                    Email = "test",
                    Name = "test",
                    Password = "test",
                    Role = new Role
                    {
                        Name = "test"
                    },
                    Surname = "test"
                },
                Id = Guid.NewGuid(),
                Logo = "test",
                Name = "test",
                RUT = "test"
            }
        };
        Home home = null;

        homeRepositoryMock.Setup(h => h.Find(It.IsAny<Func<Home, bool>>())).Returns(home);

        var homeDevice = new HomeDevice
        {
            Id = homeDeviceId,
            HomeId = Guid.NewGuid(),
            Device = securityCamera,
            Name = "test",
            Online = true
        };
        homeDeviceRepositoryMock.Setup(hd => hd.Find(It.IsAny<Func<HomeDevice, bool>>())).Returns(homeDevice);

        Exception exception = null;
        try
        {
            homeService.CreateMovementDetectionNotification(homeDeviceId);
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(HomeException));
        Assert.AreEqual("Home Id does not match any home", exception.Message);
    }

    [TestMethod]
    public void Create_MovementDetectionNotification_HomeDevice_NotFound_ThrowsHomeException()
    {
        var homeDeviceId = Guid.NewGuid();
        HomeDevice homeDevice = null;

        homeDeviceRepositoryMock.Setup(hd => hd.Find(It.IsAny<Func<HomeDevice, bool>>())).Returns(homeDevice);

        Exception exception = null;
        try
        {
            homeService.CreateMovementDetectionNotification(homeDeviceId);
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(HomeException));
        Assert.AreEqual("Home Device Id does not match any home device", exception.Message);
    }

    [TestMethod]
    public void Create_MovementDetectionNotification_DeviceType_Not_SecurityCamera_ThrowsDeviceException()
    {
        var homeDeviceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var homeDevice = new HomeDevice
        {
            Id = homeDeviceId,
            Online = true,
            Name = "Test Device",
            Device = new Device
            {
                Id = Guid.NewGuid(),
                Name = "Test Device",
                Type = "Other Device",
                ModelNumber = "12345",
                Description = "Test Description",
                Photos = new List<Photo>()
            }
        };

        homeDeviceRepositoryMock.Setup(hd => hd.Find(It.IsAny<Func<HomeDevice, bool>>())).Returns(homeDevice);

        Exception exception = null;
        try
        {
            homeService.CreatePersonDetectionNotification(homeDeviceId, userId);
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        Assert.AreEqual("The device type is not Security Camera", exception.Message);
    }

    [TestMethod]
    public void Create_OpenCloseWindowNotification_DeviceType_Not_WindowSensor_ThrowsDeviceException()
    {
        var homeDeviceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var homeDevice = new HomeDevice
        {
            Id = homeDeviceId,
            Online = true,
            Name = "Test Device",
            Device = new Device
            {
                Id = Guid.NewGuid(),
                Name = "Test Device",
                Type = "Other Device",
                ModelNumber = "12345",
                Description = "Test Description",
                Photos = new List<Photo>()
            }
        };

        homeDeviceRepositoryMock.Setup(hd => hd.Find(It.IsAny<Func<HomeDevice, bool>>())).Returns(homeDevice);

        Exception exception = null;
        try
        {
            homeService.CreateOpenCloseWindowNotification(homeDeviceId);
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        Assert.AreEqual("The device type is not Window Sensor", exception.Message);
    }

    [TestMethod]
    public void Register_WindowSensor_Device_To_Home_Test()
    {
        var homeId = Guid.NewGuid();
        var home = new Home
        {
            Id = homeId,
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name"
        };
        var businessOwnerRole = new Role
        {
            Name = "BusinessOwner"
        };
        var businessOwner = new User
        {
            Email = "blankEmail@blank.com",
            Name = "blankName",
            Surname = "blanckSurname",
            Password = "blankPassword",
            Id = Guid.NewGuid(),
            Role = businessOwnerRole
        };
        var business = new Business
        {
            BusinessOwner = businessOwner,
            Id = Guid.NewGuid(),
            Name = "bName",
            Logo = "logo",
            RUT = "111222333"
        };
        var deviceId = Guid.NewGuid();
        var device = new Device
        {
            Id = deviceId,
            Name = "Window sensor",
            ModelNumber = "1234",
            Description = "Window sensor for home",
            Photos = [],
            Business = business
        };
        device.Type = "Window Sensor";
        var homeDeviceId = Guid.NewGuid();
        var homeDevice = new HomeDevice
        {
            Id = homeDeviceId,
            Online = true,
            Device = device,
            HomeId = homeId,
            Name = device.Name
        };
        home.Devices.Add(homeDevice);

        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        deviceRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Device, bool>>())).Returns(device);
        homeDeviceRepositoryMock.Setup(x => x.Add(It.IsAny<HomeDevice>())).Returns(homeDevice);

        homeService.AddDeviceToHome(homeId, deviceId);
        homeRepositoryMock.VerifyAll();
        userRepositoryMock.VerifyAll();

        Assert.IsNotNull(home.Devices);
        Assert.AreEqual(1, home.Devices.Count);
    }

    [TestMethod]
    public void Register_IntelligentLamp_Device_To_Home_Test()
    {
        var homeId = Guid.NewGuid();
        var home = new Home
        {
            Id = homeId,
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name"
        };
        var businessOwnerRole = new Role
        {
            Name = "BusinessOwner"
        };
        var businessOwner = new User
        {
            Email = "blankEmail@blank.com",
            Name = "blankName",
            Surname = "blanckSurname",
            Password = "blankPassword",
            Id = Guid.NewGuid(),
            Role = businessOwnerRole
        };
        var business = new Business
        {
            BusinessOwner = businessOwner,
            Id = Guid.NewGuid(),
            Name = "bName",
            Logo = "logo",
            RUT = "111222333"
        };
        var deviceId = Guid.NewGuid();
        var device = new Device
        {
            Id = deviceId,
            Name = "Inteligent Lamp",
            ModelNumber = "1234",
            Description = "Inteligent lamp for home",
            Photos = [],
            Business = business
        };
        device.Type = "Inteligent Lamp";
        var homeDeviceId = Guid.NewGuid();
        var homeDevice = new HomeDevice
        {
            Id = homeDeviceId,
            Online = true,
            Device = device,
            HomeId = homeId,
            Name = device.Name
        };
        home.Devices.Add(homeDevice);

        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        deviceRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Device, bool>>())).Returns(device);
        homeDeviceRepositoryMock.Setup(x => x.Add(It.IsAny<HomeDevice>())).Returns(homeDevice);

        homeService.AddDeviceToHome(homeId, deviceId);
        homeRepositoryMock.VerifyAll();
        userRepositoryMock.VerifyAll();

        Assert.IsNotNull(home.Devices);
        Assert.AreEqual(1, home.Devices.Count);
    }

    [TestMethod]
    public void Create_Room_Test()
    {
        var homeId = Guid.NewGuid();
        var home = new Home
        {
            Id = homeId,
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name",
            Rooms = new List<Room>()
        };

        var room = new Room
        {
            Id = Guid.NewGuid(),
            Name = "Room",
            Home = home
        };

        homeRepositoryMock.Setup(x => x.Find(It.Is<Func<Home, bool>>(predicate => predicate(home))))
            .Returns(home);

        homeRepositoryMock.Setup(x => x.Update(It.IsAny<Home>()))
            .Returns(home);

        var result = homeService.CreateRoom(room, homeId);

        result.Should().NotBeNull("the method must return the room created");
        result.Name.Should().Be("Room", "the returned room must have the assigned name");
        result.Home.Id.Should().Be(homeId, "the home's ID should match the provided homeId");
        home.Rooms.Should().ContainSingle(x => x.Name == "Room", "there should be only one room with the name 'Room' in the home");
        home.Rooms.Should().HaveCount(1, "the number of rooms in the home should be exactly 1");

        homeRepositoryMock.Verify(x => x.Update(It.IsAny<Home>()), Times.Once);
    }

    [TestMethod]
    public void CreateRoom_WhenHomeDoesNotExist_ThrowsHomeException()
    {
        var nonExistentHomeId = Guid.NewGuid();
        var nonExistentHome = new Home
        {
            Id = nonExistentHomeId,
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name",
            Rooms = new List<Room>()
        };
        var room = new Room
        {
            Id = Guid.NewGuid(),
            Name = "Room",
            Home = nonExistentHome
        };

        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>()))
            .Returns((Home)null);

        var exception = Assert.ThrowsException<HomeException>(
            () => homeService.CreateRoom(room, nonExistentHomeId)
        );

        exception.Message.Should().Be("Home Id does not match any home");
        homeRepositoryMock.Verify(x => x.Update(It.IsAny<Home>()), Times.Never);
    }

    [TestMethod]
    public void CreateRoom_WhenRoomWithSameNameExists_ThrowsRoomException()
    {
        var homeId = Guid.NewGuid();
        var homeRooms = new List<Room>();
        var home = new Home
        {
            Id = homeId,
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name",
            Rooms = homeRooms
        };
        var existingRoom = new Room
        {
            Id = Guid.NewGuid(),
            Name = "ExistingRoom",
            Home = home
        };
        homeRooms.Add(existingRoom);
        var newRoom = new Room
        {
            Id = Guid.NewGuid(),
            Name = "ExistingRoom",
            Home = home
        };

        homeRepositoryMock.Setup(x => x.Find(It.Is<Func<Home, bool>>(predicate => predicate(home))))
            .Returns(home);

        var exception = Assert.ThrowsException<RoomException>(
            () => homeService.CreateRoom(newRoom, homeId)
        );

        exception.Message.Should().Be("Room already exists");
        homeRepositoryMock.Verify(x => x.Update(It.IsAny<Home>()), Times.Never);
    }

    [TestMethod]
    public void Add_Device_To_Room_Test()
    {
        var homeId = Guid.NewGuid();
        var home = new Home
        {
            Id = homeId,
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name",
            Rooms = new List<Room>(),
            Devices = new List<HomeDevice>()
        };

        var room = new Room
        {
            Id = Guid.NewGuid(),
            Name = "Room",
            Home = home,
            HomeDevices = new List<HomeDevice>()
        };

        var deviceId = Guid.NewGuid();
        var device = new Device
        {
            Id = deviceId,
            Name = "Device",
            ModelNumber = "1234",
            Description = "Device for home",
            Photos = [],
            Business = new Business
            {
                BusinessOwner = new User
                {
                    Email = "test",
                    Name = "test",
                    Password = "test",
                    Role = new Role
                    {
                        Name = "test"
                    },
                    Surname = "test"
                },
                Id = Guid.NewGuid(),
                Logo = "test",
                Name = "test",
                RUT = "test"
            }
        };

        var homeDeviceId = Guid.NewGuid();
        var homeDevice = new HomeDevice
        {
            Id = homeDeviceId,
            Online = true,
            Device = device,
            HomeId = homeId,
            Name = device.Name
        };

        home.Devices.Add(homeDevice);
        home.Rooms.Add(room);

        roomRepositoryMock.Setup(x => x.Find(It.Is<Func<Room, bool>>(predicate => predicate(room))))
            .Returns(room);
        roomRepositoryMock.Setup(x => x.Update(room))
            .Returns(room)
            .Verifiable();

        homeDeviceRepositoryMock.Setup(x => x.Find(It.Is<Func<HomeDevice, bool>>(predicate => predicate(homeDevice))))
            .Returns(homeDevice);

        var result = homeService.AddDevicesToRoom(homeDeviceId, room.Id);

        result.Should().NotBeNull("the method must return the home device");
        result.Should().Be(homeDevice, "the returned device should be the one we added");
        result.Name.Should().Be("Device", "the returned device must have the assigned name");

        room.HomeDevices.Should().ContainSingle(x => x.Id == homeDeviceId,
            "there should be only one device with the specified ID in the room");
        room.HomeDevices.Should().HaveCount(1,
            "the number of devices in the room should be exactly 1");

        roomRepositoryMock.Verify(x => x.Find(It.IsAny<Func<Room, bool>>()), Times.Once(),
            "Find should be called exactly once");
        roomRepositoryMock.Verify(x => x.Update(room), Times.Once(),
            "Update should be called exactly once with the modified room");
        homeDeviceRepositoryMock.Verify(x => x.Find(It.IsAny<Func<HomeDevice, bool>>()), Times.Once(),
            "FindHomeDeviceById should be called exactly once");
    }

    [TestMethod]
    public void Add_Device_To_Room_When_Device_Already_Exists_Should_ThrowException()
    {
        var homeId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var homeDeviceId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();

        var owner = new User
        {
            Id = Guid.NewGuid(),
            Name = "Owner Name",
            Email = "owner@example.com",
            Password = "password",
            Role = new Role { Name = "Admin" },
            Surname = "OwnerSurname"
        };

        var home = new Home
        {
            Id = homeId,
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name",
            Rooms = new List<Room>(),
            Devices = new List<HomeDevice>()
        };

        var room = new Room
        {
            Id = roomId,
            Name = "Room",
            Home = home,
            HomeDevices = new List<HomeDevice>()
        };

        var device = new Device
        {
            Id = deviceId,
            Name = "Device",
            ModelNumber = "1234",
            Description = "Device for home",
            Photos = [],
            Business = new Business
            {
                BusinessOwner = new User
                {
                    Email = "businessowner@example.com",
                    Name = "Business Owner",
                    Password = "password",
                    Role = new Role { Name = "Owner" },
                    Surname = "BusinessOwnerSurname"
                },
                Id = Guid.NewGuid(),
                Logo = "logo.png",
                Name = "Business Name",
                RUT = "12345678-9"
            }
        };

        var homeDevice = new HomeDevice
        {
            Id = homeDeviceId,
            Online = true,
            Device = device,
            HomeId = homeId,
            Name = device.Name
        };

        room.HomeDevices.Add(homeDevice);

        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>()))
            .Returns((Func<Home, bool> predicate) => predicate(home) ? home : null);

        roomRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Room, bool>>()))
            .Returns((Func<Room, bool> predicate) => predicate(room) ? room : null);

        homeDeviceRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeDevice, bool>>()))
            .Returns((Func<HomeDevice, bool> predicate) => predicate(homeDevice) ? homeDevice : null);

        homeService.Invoking(s => s.AddDevicesToRoom(homeDeviceId, roomId))
            .Should().Throw<RoomException>()
            .WithMessage("Device already exists in room");

        roomRepositoryMock.Verify(x => x.Update(It.IsAny<Room>()), Times.Never(),
            "Update should not be called when device already exists");
    }

    [TestMethod]
    public void Add_Device_To_Room_When_Room_Not_Found_Should_ThrowException()
    {
        var roomId = Guid.NewGuid();
        var homeDeviceId = Guid.NewGuid();

        var owner = new User
        {
            Id = Guid.NewGuid(),
            Name = "Owner Name",
            Email = "owner@example.com",
            Password = "password",
            Role = new Role { Name = "Admin" },
            Surname = "OwnerSurname"
        };

        var home = new Home
        {
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name",
            Rooms = new List<Room>(),
            Devices = new List<HomeDevice>()
        };

        var device = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Device",
            ModelNumber = "1234",
            Description = "Device for home",
            Photos = [],
            Business = new Business
            {
                BusinessOwner = new User
                {
                    Email = "businessowner@example.com",
                    Name = "Business Owner",
                    Password = "password",
                    Role = new Role { Name = "Owner" },
                    Surname = "BusinessOwnerSurname"
                },
                Id = Guid.NewGuid(),
                Logo = "logo.png",
                Name = "Business Name",
                RUT = "12345678-9"
            }
        };

        var homeDevice = new HomeDevice
        {
            Id = homeDeviceId,
            Online = true,
            Device = device,
            HomeId = home.Id,
            Name = device.Name
        };

        home.Devices.Add(homeDevice);

        homeDeviceRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeDevice, bool>>()))
            .Returns((Func<HomeDevice, bool> predicate) => predicate(homeDevice) ? homeDevice : null);

        roomRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Room, bool>>()))
            .Returns((Room)null);

        homeRepositoryMock.Setup(x => x.Update(It.IsAny<Home>()))
            .Verifiable();

        homeService.Invoking(s => s.AddDevicesToRoom(homeDeviceId, roomId))
            .Should().Throw<RoomException>()
            .WithMessage("Room not found");

        roomRepositoryMock.Verify(x => x.Update(It.IsAny<Room>()), Times.Never());
    }

    [TestMethod]
    public void GetAllHomeDevices_WithoutRoomFilter_ReturnsAllDevices()
    {
        var homeId = Guid.NewGuid();
        var home = new Home
        {
            Id = homeId,
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name",
            Rooms = new List<Room>(),
            Devices = new List<HomeDevice>()
        };

        var device1 = new HomeDevice
        {
            Online = true,
            Id = Guid.NewGuid(),
            Name = "Device 1",
            Device = new Device
            {
                Id = Guid.NewGuid(),
                Name = "Device 1",
                ModelNumber = "1234",
                Description = "Device 1 for home",
                Photos = new List<Photo>(),
                Business = new Business
                {
                    Id = Guid.NewGuid(),
                    Name = "Business 1",
                    Logo = "logo1.png",
                    RUT = "12345678-9",
                    BusinessOwner = new User
                    {
                        Email = "mail@mail.com",
                        Name = "Owner 1",
                        Password = "password",
                        Role = new Role { Name = "Admin" },
                        Surname = "Surname 1"
                    }
                }
            }
        };

        var device2 = new HomeDevice
        {
            Online = true,
            Id = Guid.NewGuid(),
            Name = "Device 2",
            Device = new Device
            {
                Id = Guid.NewGuid(),
                Name = "Device 2",
                ModelNumber = "1234",
                Description = "Device 2 for home",
                Photos = new List<Photo>(),
                Business = new Business
                {
                    Id = Guid.NewGuid(),
                    Name = "Business 2",
                    Logo = "logo2.png",
                    RUT = "12345678-9",
                    BusinessOwner = new User
                    {
                        Email = "mail@mail.com",
                        Name = "Owner 1",
                        Password = "password",
                        Role = new Role { Name = "Admin" },
                        Surname = "Surname 1"
                    }
                }
            }
        };

        home.Devices = new List<HomeDevice>() { device1, device2 };

        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);

        var result = homeService.GetAllHomeDevices(homeId, null);

        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public void GetAllHomeDevices_WithRoomFilter_ReturnsFilteredDevices()
    {
        var homeId = Guid.NewGuid();
        var home = new Home
        {
            Id = homeId,
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name",
            Rooms = new List<Room>(),
            Devices = new List<HomeDevice>()
        };

        var room = new Room
        {
            Id = Guid.NewGuid(),
            Name = "Living Room",
            Home = home
        };

        var device1 = new HomeDevice
        {
            Online = true,
            Id = Guid.NewGuid(),
            Name = "Device 1",
            Device = new Device
            {
                Id = Guid.NewGuid(),
                Name = "Device 1",
                ModelNumber = "1234",
                Description = "Device 1 for home",
                Photos = new List<Photo>(),
                Business = new Business
                {
                    Id = Guid.NewGuid(),
                    Name = "Business 1",
                    Logo = "logo1.png",
                    RUT = "12345678-9",
                    BusinessOwner = new User
                    {
                        Email = "mail@mail.com",
                        Name = "Owner 1",
                        Password = "password",
                        Role = new Role { Name = "Admin" },
                        Surname = "Surname 1"
                    }
                }
            }
        };

        var device2 = new HomeDevice
        {
            Room = room,
            Online = true,
            Id = Guid.NewGuid(),
            Name = "Device 2",
            Device = new Device
            {
                Id = Guid.NewGuid(),
                Name = "Device 2",
                ModelNumber = "1234",
                Description = "Device 2 for home",
                Photos = new List<Photo>(),
                Business = new Business
                {
                    Id = Guid.NewGuid(),
                    Name = "Business 2",
                    Logo = "logo2.png",
                    RUT = "12345678-9",
                    BusinessOwner = new User
                    {
                        Email = "mail@mail.com",
                        Name = "Owner 1",
                        Password = "password",
                        Role = new Role { Name = "Admin" },
                        Surname = "Surname 1"
                    }
                }
            }
        };

        home.Devices = new List<HomeDevice>()
        {
            device1,
            device2
        };

        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);

        var result = homeService.GetAllHomeDevices(homeId, "Living Room");

        Assert.AreEqual(1, result.Count());
        Assert.AreEqual("Living Room", result.First().Room.Name);
    }

    [TestMethod]
    public void GetAllHomeDevices_WithNonExistingRoomFilter_ReturnsEmpty()
    {
        var homeId = Guid.NewGuid();
        var home = new Home
        {
            Id = homeId,
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name",
            Rooms = new List<Room>(),
            Devices = new List<HomeDevice>()
        };

        var room = new Room
        {
            Id = Guid.NewGuid(),
            Name = "Living Room",
            Home = home
        };

        var device1 = new HomeDevice
        {
            Room = room,
            Online = true,
            Id = Guid.NewGuid(),
            Name = "Device 1",
            Device = new Device
            {
                Id = Guid.NewGuid(),
                Name = "Device 1",
                ModelNumber = "1234",
                Description = "Device 1 for home",
                Photos = new List<Photo>(),
                Business = new Business
                {
                    Id = Guid.NewGuid(),
                    Name = "Business 1",
                    Logo = "logo1.png",
                    RUT = "12345678-9",
                    BusinessOwner = new User
                    {
                        Email = "mail@mail.com",
                        Name = "Owner 1",
                        Password = "password",
                        Role = new Role { Name = "Admin" },
                        Surname = "Surname 1"
                    }
                }
            }
        };

        var device2 = new HomeDevice
        {
            Room = room,
            Online = true,
            Id = Guid.NewGuid(),
            Name = "Device 2",
            Device = new Device
            {
                Id = Guid.NewGuid(),
                Name = "Device 2",
                ModelNumber = "1234",
                Description = "Device 2 for home",
                Photos = new List<Photo>(),
                Business = new Business
                {
                    Id = Guid.NewGuid(),
                    Name = "Business 2",
                    Logo = "logo2.png",
                    RUT = "12345678-9",
                    BusinessOwner = new User
                    {
                        Email = "mail@mail.com",
                        Name = "Owner 1",
                        Password = "password",
                        Role = new Role { Name = "Admin" },
                        Surname = "Surname 1"
                    }
                }
            }
        };

        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);

        var result = homeService.GetAllHomeDevices(homeId, "Kitchen");

        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public void UnRelatedHomeOwnersTest_Ok()
    {
        var member1 = new User
        {
            Email = "blankEmail1@blank.com",
            Name = "blankName1",
            Surname = "blanckSurname1",
            Password = "blankPassword",
            Id = Guid.NewGuid(),
            Role = homeOwnerRole
        };
        var member2 = new User
        {
            Email = "blankEmail2@blank.com",
            Name = "blankName2",
            Surname = "blanckSurname2",
            Password = "blankPassword",
            Id = Guid.NewGuid(),
            Role = homeOwnerRole
        };
        var homeOwner = new HomeMember(owner);
        var homeMember1 = new HomeMember(member1);

        var users = new List<User>
        {
            member2,
            member1
        };

        if (owner != null)
        {
            users.Add(owner);
        }

        var home = new Home
        {
            Id = Guid.NewGuid(),
            MainStreet = "Street",
            DoorNumber = "123",
            Latitude = "-31",
            Longitude = "31",
            MaxMembers = 6,
            Owner = owner,
            Name = "House Name"
        };
        home.Members.Add(homeOwner);
        home.Members.Add(homeMember1);

        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        userRepositoryMock.Setup(x => x.FindAll()).Returns(users);

        IEnumerable<User> returnedUsers = homeService.UnRelatedHomeOwners(home.Id);
        homeRepositoryMock.VerifyAll();

        Assert.AreEqual(returnedUsers.First(), users.First());
        Assert.IsTrue(returnedUsers.Count() == 1);
    }

    [TestMethod]
    public void TurnOnOffHomeDevice_ExistingDevice_TogglesOnlineStatus()
    {
        var Device = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Device 2",
            ModelNumber = "1234",
            Description = "Device 2 for home",
            Photos = new List<Photo>(),
            Business = new Business
            {
                Id = Guid.NewGuid(),
                Name = "Business 2",
                Logo = "logo2.png",
                RUT = "12345678-9",
                BusinessOwner = new User
                {
                    Email = "mail@mail.com",
                    Name = "Owner 1",
                    Password = "password",
                    Role = new Role { Name = "Admin" },
                    Surname = "Surname 1"
                }
            }
        };
        var homeDeviceId = Guid.NewGuid();
        var homeDevice = new HomeDevice
        {
            Device = Device,
            Id = homeDeviceId,
            Online = false,
            Name = "Device 1"
        };

        homeDeviceRepositoryMock.Setup(repo => repo.Find(It.IsAny<Func<HomeDevice, bool>>()))
            .Returns(homeDevice);

        homeDeviceRepositoryMock.Setup(repo => repo.Update(It.IsAny<HomeDevice>())).Returns(homeDevice);

        var result = homeService.TurnOnOffHomeDevice(homeDeviceId);

        homeDeviceRepositoryMock.Verify(repo => repo.Update(homeDevice), Times.Once);
        Assert.IsTrue(result);
        Assert.IsTrue(homeDevice.Online);
    }

    [TestMethod]
    public void TurnOnOffHomeDevice_DeviceAlreadyOnline_TurnsOff()
    {
        var Device = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Device 2",
            ModelNumber = "1234",
            Description = "Device 2 for home",
            Photos = new List<Photo>(),
            Business = new Business
            {
                Id = Guid.NewGuid(),
                Name = "Business 2",
                Logo = "logo2.png",
                RUT = "12345678-9",
                BusinessOwner = new User
                {
                    Email = "mail@mail.com",
                    Name = "Owner 1",
                    Password = "password",
                    Role = new Role { Name = "Admin" },
                    Surname = "Surname 1"
                }
            }
        };
        var homeDeviceId = Guid.NewGuid();
        var homeDevice = new HomeDevice
        {
            Device = Device,
            Id = homeDeviceId,
            Online = true,
            Name = "Device 1"
        };

        homeDeviceRepositoryMock.Setup(repo => repo.Find(It.IsAny<Func<HomeDevice, bool>>()))
            .Returns(homeDevice);
        homeDeviceRepositoryMock.Setup(repo => repo.Update(It.IsAny<HomeDevice>())).Returns(homeDevice);

        var result = homeService.TurnOnOffHomeDevice(homeDeviceId);

        homeDeviceRepositoryMock.Verify(repo => repo.Update(homeDevice), Times.Once);
        Assert.IsFalse(result);
        Assert.IsFalse(homeDevice.Online);
    }
}
