﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Moq;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.ExtraRepositoryInterfaces;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Services;

namespace SmartHome.BusinessLogicTest;

[TestClass]
public class HomeServiceTest
{
    private Mock<IGenericRepository<Home>>? homeRepositoryMock;
    private Mock<IGenericRepository<User>>? userRepositoryMock;
    private Mock<IGenericRepository<HomePermission>>? homePermissionRepositoryMock;
    private Mock<IGenericRepository<HomeDevice>>? homeDeviceRepositoryMock;
    private Mock<IGenericRepository<HomeMember>>? homeMemberRepositoryMock;
    private Mock<IGenericRepository<Device>>? deviceRepositoryMock;
    private Mock<IHomesFromUserRepository>? homesFromUserRepositoryMock;
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
        homesFromUserRepositoryMock = new Mock<IHomesFromUserRepository>(MockBehavior.Strict);
        homeService = new HomeService(homeMemberRepositoryMock.Object, homeDeviceRepositoryMock.Object, homeRepositoryMock.Object,
                                      userRepositoryMock.Object, homePermissionRepositoryMock.Object, deviceRepositoryMock.Object,
                                      homesFromUserRepositoryMock.Object);
        homeOwnerRole = new Role { Name = "HomeOwner" };
        ownerId = Guid.NewGuid();
        owner = new User { Email = "owner@blank.com", Name = "ownerName", Surname = "ownerSurname", Password = "ownerPassword", Id = ownerId, Role = homeOwnerRole };
    }

    [TestMethod]
    public void Create_Home_OK_Test()
    {
        var home = new Home { Id = Guid.NewGuid(), MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };

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
        var home = new Home { Id = Guid.NewGuid(), MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };
        var memberId = Guid.NewGuid();
        var member = new User { Email = "blankEmail1@blank.com", Name = "blankName1", Surname = "blanckSurname1", Password = "blankPassword", Id = memberId, Role = homeOwnerRole };

        homeRepositoryMock.Setup(x => x.Update(It.IsAny<Home>())).Returns(home);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(owner);

        homeService.AddHomeMemberToHome(home.Id, memberId);
        homeRepositoryMock.VerifyAll();
        userRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void GetAll_HomeMembers_Test()
    {
        var member1 = new User { Email = "blankEmail1@blank.com", Name = "blankName1", Surname = "blanckSurname1", Password = "blankPassword", Id = new Guid(), Role = homeOwnerRole };
        var member2 = new User { Email = "blankEmail2@blank.com", Name = "blankName2", Surname = "blanckSurname2", Password = "blankPassword", Id = new Guid(), Role = homeOwnerRole };
        var homeOwner = new HomeMember(owner);
        var homeMember1 = new HomeMember(member1);
        var homeMember2 = new HomeMember(member2);

        var home = new Home { Id = Guid.NewGuid(), MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };
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
        var home = new Home { Devices = new List<HomeDevice>(), Id = Guid.NewGuid(), MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };
        var businessOwnerRole = new Role { Name = "BusinessOwner" };
        var businessOwner = new User { Email = "blankEmail@blank.com", Name = "blankName", Surname = "blanckSurname", Password = "blankPassword", Id = new Guid(), Role = businessOwnerRole };
        var business = new Business { BusinessOwner = businessOwner, Id = Guid.NewGuid(), Name = "bName", Logo = "logo", RUT = "111222333" };

        var device1 = new Device { Id = Guid.NewGuid(), Name = "Device1", Description = "A Device", Business = business, ModelNumber = "123", Photos = "photos" };
        var device2 = new Device { Id = Guid.NewGuid(), Name = "Device2", Description = "A Device", Business = business, ModelNumber = "123", Photos = "photos" };

        var homeDevice1 = new HomeDevice { Device = device1, Id = Guid.NewGuid(), Online = true };
        var homeDevice2 = new HomeDevice { Device = device2, Id = Guid.NewGuid(), Online = false };

        home.Devices.Add(homeDevice1);
        home.Devices.Add(homeDevice2);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);

        IEnumerable<HomeDevice> homeDevices = homeService.GetAllHomeDevices(home.Id);
        homeRepositoryMock.VerifyAll();

        Assert.AreEqual(home.Devices.First(), homeDevices.First());
        Assert.AreEqual(home.Devices.Last(), homeDevices.Last());
    }

    [TestMethod]
    public void Create_Person_Detecion_Notification()
    {
        var home = new Home { Devices = new List<HomeDevice>(), Id = Guid.NewGuid(), MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };
        var homeMember = new HomeMember(owner);
        var businessOwnerRole = new Role { Name = "BusinessOwner" };
        var businessOwner = new User { Email = "blankEmail@blank.com", Name = "blankName", Surname = "blanckSurname", Password = "blankPassword", Id = new Guid(), Role = businessOwnerRole };
        var business = new Business { BusinessOwner = businessOwner, Id = Guid.NewGuid(), Name = "bName", Logo = "logo", RUT = "111222333" };
        var device = new Device { Name = "DeviceName", Business = business, Description = "DeviceDescription", Photos = "photo", ModelNumber = "a" };
        var homeDevice = new HomeDevice { Device = device, Id = Guid.NewGuid(), Online = true };
        var notificationPermission = new HomePermission { Id = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID), Name = "NotificationPermission" };

        var notification = new Notification { Date = DateTime.Today, Event = "Test", HomeDevice = homeDevice, Time = DateTime.Now, DetectedPerson = owner };
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
        var home = new Home { Devices = new List<HomeDevice>(), Id = Guid.NewGuid(), MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };
        var homeMember = new HomeMember(owner);
        var businessOwnerRole = new Role { Name = "BusinessOwner" };
        var businessOwner = new User { Email = "blankEmail@blank.com", Name = "blankName", Surname = "blanckSurname", Password = "blankPassword", Id = new Guid(), Role = businessOwnerRole };
        var business = new Business { BusinessOwner = businessOwner, Id = Guid.NewGuid(), Name = "bName", Logo = "logo", RUT = "111222333" };
        var device = new Device { Name = "DeviceName", Business = business, Description = "DeviceDescription", Photos = "photo", ModelNumber = "a" };
        var homeDevice = new HomeDevice { Device = device, Id = Guid.NewGuid(), Online = true };
        var notificationPermission = new HomePermission { Id = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID), Name = "NotificationPermission" };

        var notification = new Notification { Date = DateTime.Today, Event = "Test", HomeDevice = homeDevice, Time = DateTime.Now };
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
        var home = new Home { Devices = new List<HomeDevice>(), Id = Guid.NewGuid(), MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };
        var homeMember = new HomeMember(owner);
        var businessOwnerRole = new Role { Name = "BusinessOwner" };
        var businessOwner = new User { Email = "blankEmail@blank.com", Name = "blankName", Surname = "blanckSurname", Password = "blankPassword", Id = new Guid(), Role = businessOwnerRole };
        var business = new Business { BusinessOwner = businessOwner, Id = Guid.NewGuid(), Name = "bName", Logo = "logo", RUT = "111222333" };
        var device = new Device { Name = "DeviceName", Business = business, Description = "DeviceDescription", Photos = "photo", ModelNumber = "a" };
        var homeDevice = new HomeDevice { Device = device, Id = Guid.NewGuid(), Online = false };

        var notification = new Notification { Date = DateTime.Today, Event = "Test", HomeDevice = homeDevice, Time = DateTime.Now };
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
        // ASSERT

        var home = new Home { Devices = new List<HomeDevice>(), Id = Guid.NewGuid(), MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };
        var notificationPermission = new HomePermission { Id = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID), Name = "NotificationPermission" };

        homeRepositoryMock.Setup(x => x.Add(It.IsAny<Home>())).Returns(home);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns((Home)null);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(owner);
        homePermissionRepositoryMock.Setup(x => x.FindAll()).Returns(new List<HomePermission> { notificationPermission });

        var result = homeService.CreateHome(home, ownerId);

        var memberId = Guid.NewGuid();
        var member = new User { Email = "blankEmail1@blank.com", Name = "blankName1", Surname = "blanckSurname1", Password = "blankPassword", Id = memberId, Role = homeOwnerRole };

        homeRepositoryMock.Setup(x => x.Update(It.IsAny<Home>())).Returns(home);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(member);

        HomeMember homeMember = homeService.AddHomeMemberToHome(home.Id, memberId);

        var businessOwnerRole = new Role { Name = "BusinessOwner" };
        var businessOwner = new User { Email = "blankEmail@blank.com", Name = "blankName", Surname = "blanckSurname", Password = "blankPassword", Id = new Guid(), Role = businessOwnerRole };
        var business = new Business { BusinessOwner = businessOwner, Id = Guid.NewGuid(), Name = "bName", Logo = "logo", RUT = "111222333" };
        var device = new Device { Name = "DeviceName", Business = business, Description = "DeviceDescription", Photos = "photo", ModelNumber = "a" };
        var homeDevice = new HomeDevice { Device = device, Id = Guid.NewGuid(), Online = true };

        home.Devices.Add(homeDevice);

        var homeOwner = result.Members.Find(x => x.User == owner);

        homeDeviceRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeDevice, bool>>())).Returns(homeDevice);
        homeMemberRepositoryMock
            .SetupSequence(x => x.Find(It.IsAny<Func<HomeMember, bool>>()))
            .Returns(homeOwner)
            .Returns(homeMember);
        homePermissionRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomePermission, bool>>())).Returns(notificationPermission);

        // ACCTION

        homeService.CreateMovementDetectionNotification(homeDevice.Id);

        IEnumerable<HomeMember> homeMembers = (List<HomeMember>)homeService.GetAllHomeMembers(home.Id);
        var ownerFound = homeMembers.First(x => x.User == owner);
        var memberFound = homeMembers.First(x => x.User == member);

        // ASSERT

        Assert.IsTrue(ownerFound.Notifications.Count > 0);
        Assert.IsTrue(memberFound.Notifications.Count == 0);

        homeRepositoryMock.VerifyAll();
        userRepositoryMock.VerifyAll();
        homeRepositoryMock.VerifyAll();
    }

    [TestMethod]

    public void Register_Device_To_Home_Test()
    {
        var homeId = Guid.NewGuid();
        var home = new Home { Id = homeId, MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };
        var businessOwnerRole = new Role { Name = "BusinessOwner" };
        var businessOwner = new User { Email = "blankEmail@blank.com", Name = "blankName", Surname = "blanckSurname", Password = "blankPassword", Id = Guid.NewGuid(), Role = businessOwnerRole };
        var business = new Business { BusinessOwner = businessOwner, Id = Guid.NewGuid(), Name = "bName", Logo = "logo", RUT = "111222333" };
        var deviceId = Guid.NewGuid();
        var device = new Device { Id = deviceId, Name = "Window sensor", ModelNumber = "1234", Description = "Window sensor for home", Photos = "photo", Business = business };
        var homeDeviceId = Guid.NewGuid();
        var homeDevice = new HomeDevice { Id = homeDeviceId, Online = true, Device = device, HomeId = homeId };

        homeRepositoryMock.Setup(x => x.Update(It.IsAny<Home>())).Returns(home);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        deviceRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Device, bool>>())).Returns(device);

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
        var home = new Home { Id = homeId, MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };
        var deviceId = Guid.NewGuid();

        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
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

        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns((Home)null);

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
    public void Add_Permissions_To_HomeMember_Test()
    {
        var homeId = Guid.NewGuid();
        var home = new Home { Id = homeId, MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };

        var userId = Guid.NewGuid();
        var user = new User { Email = "blankEmail1@blank.com", Name = "blankName1", Surname = "blanckSurname1", Password = "blankPassword", Id = userId, Role = homeOwnerRole };

        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        homeRepositoryMock.Setup(x => x.Update(It.IsAny<Home>())).Returns(home);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(user);

        var returnedMember = homeService.AddHomeMemberToHome(homeId, userId);

        homeMemberRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeMember, bool>>())).Returns(returnedMember);
        homeMemberRepositoryMock.Setup(x => x.Update(It.IsAny<HomeMember>())).Returns(returnedMember);

        var addMemberPermission = new HomePermission { Id = Guid.Parse(SeedDataConstants.ADD_MEMBER_TO_HOME_PERMISSION_ID), Name = "AddMemberPermission" };
        var addDevicesPermission = new HomePermission { Id = Guid.Parse(SeedDataConstants.ADD_DEVICES_TO_HOME_HOMEPERMISSION_ID), Name = "AddDevicesPermission" };
        var listDevicesPermission = new HomePermission { Id = Guid.Parse(SeedDataConstants.LIST_DEVICES_HOMEPERMISSION_ID), Name = "ListDevicesPermission" };
        var notificationsPermission = new HomePermission { Id = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID), Name = "NotificationsPermission" };

        var permissions = new List<HomePermission> { addMemberPermission, addDevicesPermission, listDevicesPermission, notificationsPermission };

        homePermissionRepositoryMock.Setup(x => x.FindAll()).Returns(permissions);
        homePermissionRepositoryMock
            .SetupSequence(x => x.Find(It.IsAny<Func<HomePermission, bool>>()))
            .Returns(addMemberPermission)
            .Returns(addDevicesPermission)
            .Returns(listDevicesPermission)
            .Returns(notificationsPermission);

        homeService.UpdateHomePermissionsOfHomeMember(returnedMember.HomeMemberId, permissions);

        IEnumerable<HomeMember> members = homeService.GetAllHomeMembers(homeId);
        var foundMember = members.First(x => x.User == user);

        Assert.IsTrue(homeService.HasPermission(returnedMember.HomeMemberId, addMemberPermission.Id));
        Assert.IsTrue(homeService.HasPermission(returnedMember.HomeMemberId, addDevicesPermission.Id));
        Assert.IsTrue(homeService.HasPermission(returnedMember.HomeMemberId, listDevicesPermission.Id));
        Assert.IsTrue(homeService.HasPermission(returnedMember.HomeMemberId, notificationsPermission.Id));
    }

    [TestMethod]
    public void Update_Permissions_To_HomeMember_Test()
    {
        var homeId = Guid.NewGuid();
        var home = new Home { Id = homeId, MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };

        var userId = Guid.NewGuid();
        var user = new User { Email = "blankEmail1@blank.com", Name = "blankName1", Surname = "blanckSurname1", Password = "blankPassword", Id = userId, Role = homeOwnerRole };

        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        homeRepositoryMock.Setup(x => x.Update(It.IsAny<Home>())).Returns(home);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(user);

        var returnedMember = homeService.AddHomeMemberToHome(homeId, userId);

        homeMemberRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeMember, bool>>())).Returns(returnedMember);
        homeMemberRepositoryMock.Setup(x => x.Update(It.IsAny<HomeMember>())).Returns(returnedMember);

        var addMemberPermission = new HomePermission { Id = Guid.Parse(SeedDataConstants.ADD_MEMBER_TO_HOME_PERMISSION_ID), Name = "AddMemberPermission" };
        var addDevicesPermission = new HomePermission { Id = Guid.Parse(SeedDataConstants.ADD_DEVICES_TO_HOME_HOMEPERMISSION_ID), Name = "AddDevicesPermission" };
        var listDevicesPermission = new HomePermission { Id = Guid.Parse(SeedDataConstants.LIST_DEVICES_HOMEPERMISSION_ID), Name = "ListDevicesPermission" };
        var notificationsPermission = new HomePermission { Id = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID), Name = "NotificationsPermission" };

        var permissions = new List<HomePermission> { addMemberPermission, addDevicesPermission, listDevicesPermission, notificationsPermission };

        homePermissionRepositoryMock.Setup(x => x.FindAll()).Returns(permissions);
        homePermissionRepositoryMock
            .SetupSequence(x => x.Find(It.IsAny<Func<HomePermission, bool>>()))
            .Returns(addMemberPermission)
            .Returns(addDevicesPermission)
            .Returns(listDevicesPermission)
            .Returns(notificationsPermission);

        homeService.UpdateHomePermissionsOfHomeMember(returnedMember.HomeMemberId, permissions);

        var updatedPermissions = new List<HomePermission> { addMemberPermission, addDevicesPermission, listDevicesPermission };

        homeService.UpdateHomePermissionsOfHomeMember(returnedMember.HomeMemberId, updatedPermissions);
        IEnumerable<HomeMember> members = homeService.GetAllHomeMembers(homeId);
        var foundMember = members.First(x => x.User == user);

        Assert.IsTrue(homeService.HasPermission(returnedMember.HomeMemberId, addMemberPermission.Id));
        Assert.IsTrue(homeService.HasPermission(returnedMember.HomeMemberId, addDevicesPermission.Id));
        Assert.IsTrue(homeService.HasPermission(returnedMember.HomeMemberId, listDevicesPermission.Id));
        Assert.IsFalse(homeService.HasPermission(returnedMember.HomeMemberId, notificationsPermission.Id));
    }

    [TestMethod]
    public void Get_Users_Notifications()
    {
        var owner = new User { Email = "owner@blank.com", Name = "ownerName", Surname = "ownerSurname", Password = "ownerPassword", Id = ownerId, Role = homeOwnerRole };
        var home1Id = Guid.NewGuid();
        var home1 = new Home { Id = home1Id, MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };
        var home2Id = Guid.NewGuid();
        var home2 = new Home { Id = home2Id, MainStreet = "Street2", DoorNumber = "1235", Latitude = "-32", Longitude = "11", MaxMembers = 6, Owner = owner };
        var notificationPermission = new HomePermission { Id = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID), Name = "NotificationPermission" };

        var businessOwnerRole = new Role { Name = "BusinessOwner" };
        var businessOwner = new User { Email = "blankEmail@blank.com", Name = "blankName", Surname = "blanckSurname", Password = "blankPassword", Id = Guid.NewGuid(), Role = businessOwnerRole };
        var business = new Business { BusinessOwner = businessOwner, Id = Guid.NewGuid(), Name = "bName", Logo = "logo", RUT = "111222333" };
        var deviceId = Guid.NewGuid();
        var device = new Device { Id = deviceId, Name = "Window sensor", ModelNumber = "1234", Description = "Window sensor for home", Photos = "photo", Business = business };
        var homeDeviceId = Guid.NewGuid();

        homeRepositoryMock.Setup(x => x.Add(It.IsAny<Home>())).Returns(home1);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns((Home)null);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(owner);
        homePermissionRepositoryMock.Setup(x => x.FindAll()).Returns(new List<HomePermission> { notificationPermission });
        homePermissionRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomePermission,bool>>())).Returns(notificationPermission);

        homeService.CreateHome(home1, ownerId);
        homeService.CreateHome(home2, ownerId);

        homeRepositoryMock.SetupSequence(x => x.Update(It.IsAny<Home>())).Returns(home1).Returns(home2);
        homeRepositoryMock.SetupSequence(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home1).Returns(home2);
        deviceRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Device, bool>>())).Returns(device);

        var homeDevice1 = homeService.AddDeviceToHome(home1Id, deviceId);
        var homeDevice2 = homeService.AddDeviceToHome(home2Id, deviceId);

        homeDeviceRepositoryMock.SetupSequence(x => x.Find(It.IsAny<Func<HomeDevice, bool>>())).Returns(homeDevice1).Returns(homeDevice2);
        homeRepositoryMock.SetupSequence(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home1).Returns(home2);
        homeMemberRepositoryMock.SetupSequence(x => x.Find(It.IsAny<Func<HomeMember, bool>>())).Returns(new HomeMember(owner)).Returns(new HomeMember(owner));

        var noti1 = homeService.CreatePersonDetectionNotification(home1.Id, ownerId);
        var noti2 = homeService.CreateMovementDetectionNotification(home2.Id);
        var homeMember1 = home1.Members.FirstOrDefault(x => x.User.Id == ownerId);
        homeMember1.Notifications.Add(noti1);
        var homeMember2 = home2.Members.FirstOrDefault(x => x.User.Id == ownerId);
        homeMember2.Notifications.Add(noti2);
        home1.Members.Clear();
        home1.Members.Add(homeMember1);
        home2.Members.Clear();
        home2.Members.Add(homeMember2);

        homeRepositoryMock.Setup(x => x.FindAll()).Returns(new List<Home> { home1, home2 });

        var notifications = homeService.GetUsersNotifications(owner);
        Assert.IsTrue(notifications.Count == 2);
    }

    [TestMethod]

    public void GetAll_Users_Homes_Test()
    {
        var member1Id = Guid.NewGuid();
        var member1 = new User { Email = "blankEmail1@blank.com", Name = "blankName1", Surname = "blanckSurname1", Password = "blankPassword", Id = member1Id, Role = homeOwnerRole };
        var member2 = new User { Email = "blankEmail2@blank.com", Name = "blankName2", Surname = "blanckSurname2", Password = "blankPassword", Id = new Guid(), Role = homeOwnerRole };
        var homeOwner = new HomeMember(owner);
        var homeMember1 = new HomeMember(member1);
        var homeMember2 = new HomeMember(member2);

        var home1 = new Home { Id = Guid.NewGuid(), MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };
        var home2 = new Home { Id = Guid.NewGuid(), MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };
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
        var home = new Home { Devices = new List<HomeDevice>(), Id = Guid.NewGuid(), MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };
        var homeMember = new HomeMember(owner);
        var businessOwnerRole = new Role { Name = "BusinessOwner" };
        var businessOwner = new User { Email = "blankEmail@blank.com", Name = "blankName", Surname = "blanckSurname", Password = "blankPassword", Id = new Guid(), Role = businessOwnerRole };
        var business = new Business { BusinessOwner = businessOwner, Id = Guid.NewGuid(), Name = "bName", Logo = "logo", RUT = "111222333" };
        var device = new Device { Name = "DeviceName", Business = business, Description = "DeviceDescription", Photos = "photo", ModelNumber = "a" };
        var homeDevice = new HomeDevice { Device = device, Id = Guid.NewGuid(), Online = true };
        var notificationPermission = new HomePermission { Id = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID), Name = "NotificationPermission" };

        var notification = new Notification { Date = DateTime.Today, Event = "Test", HomeDevice = homeDevice, Time = DateTime.Now };
        home.Devices.Add(homeDevice);
        home.Members.Add(homeMember);

        homeDeviceRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeDevice, bool>>())).Returns(homeDevice);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        homeRepositoryMock.Setup(x => x.Update(It.IsAny<Home>())).Returns(home);
        homeMemberRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeMember, bool>>())).Returns(homeMember);
        homePermissionRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomePermission, bool>>())).Returns(notificationPermission);

        homeService.CreateOpenCloseWindowNotification(homeDevice.Id, true);

        homeMember.Notifications.Add(notification);
        homeRepositoryMock.VerifyAll();

        IEnumerable<HomeMember> homeMembers = (List<HomeMember>)homeService.GetAllHomeMembers(home.Id);

        Assert.AreEqual(homeMember.Notifications.First(), homeMembers.First().Notifications.First());
    }

    [TestMethod]
    public void Create_Closed_Window_Notification_Test()
    {
        var home = new Home { Devices = new List<HomeDevice>(), Id = Guid.NewGuid(), MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };
        var homeMember = new HomeMember(owner);
        var businessOwnerRole = new Role { Name = "BusinessOwner" };
        var businessOwner = new User { Email = "blankEmail@blank.com", Name = "blankName", Surname = "blanckSurname", Password = "blankPassword", Id = new Guid(), Role = businessOwnerRole };
        var business = new Business { BusinessOwner = businessOwner, Id = Guid.NewGuid(), Name = "bName", Logo = "logo", RUT = "111222333" };
        var device = new Device { Name = "DeviceName", Business = business, Description = "DeviceDescription", Photos = "photo", ModelNumber = "a" };
        var homeDevice = new HomeDevice { Device = device, Id = Guid.NewGuid(), Online = true };
        var notificationPermission = new HomePermission { Id = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID), Name = "NotificationPermission" };

        var notification = new Notification { Date = DateTime.Today, Event = "Test", HomeDevice = homeDevice, Time = DateTime.Now };
        home.Devices.Add(homeDevice);
        home.Members.Add(homeMember);

        homeDeviceRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeDevice, bool>>())).Returns(homeDevice);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        homeRepositoryMock.Setup(x => x.Update(It.IsAny<Home>())).Returns(home);
        homeMemberRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeMember, bool>>())).Returns(homeMember);
        homePermissionRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomePermission, bool>>())).Returns(notificationPermission);

        homeService.CreateOpenCloseWindowNotification(homeDevice.Id, false);

        homeMember.Notifications.Add(notification);
        homeRepositoryMock.VerifyAll();

        IEnumerable<HomeMember> homeMembers = (List<HomeMember>)homeService.GetAllHomeMembers(home.Id);

        Assert.AreEqual(homeMember.Notifications.First(), homeMembers.First().Notifications.First());
    }

    [TestMethod]
    public void Create_Window_Sensor_Notification_On_Off_Device_Throws_Exception()
    {
        var home = new Home { Devices = new List<HomeDevice>(), Id = Guid.NewGuid(), MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };
        var homeMember = new HomeMember(owner);
        var businessOwnerRole = new Role { Name = "BusinessOwner" };
        var businessOwner = new User { Email = "blankEmail@blank.com", Name = "blankName", Surname = "blanckSurname", Password = "blankPassword", Id = new Guid(), Role = businessOwnerRole };
        var business = new Business { BusinessOwner = businessOwner, Id = Guid.NewGuid(), Name = "bName", Logo = "logo", RUT = "111222333" };
        var device = new Device { Name = "DeviceName", Business = business, Description = "DeviceDescription", Photos = "photo", ModelNumber = "a" };
        var homeDevice = new HomeDevice { Device = device, Id = Guid.NewGuid(), Online = false };

        var notification = new Notification { Date = DateTime.Today, Event = "Test", HomeDevice = homeDevice, Time = DateTime.Now };
        home.Devices.Add(homeDevice);
        home.Members.Add(homeMember);

        homeDeviceRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeDevice, bool>>())).Returns(homeDevice);

        var ex = new HomeDeviceException("PlaceHolder");
        try
        {
            homeService.CreateOpenCloseWindowNotification(homeDevice.Id, false);
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
        // ASSERT

        var home = new Home { Devices = new List<HomeDevice>(), Id = Guid.NewGuid(), MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };
        var notificationPermission = new HomePermission { Id = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID), Name = "NotificationPermission" };

        homeRepositoryMock.Setup(x => x.Add(It.IsAny<Home>())).Returns(home);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns((Home)null);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(owner);
        homePermissionRepositoryMock.Setup(x => x.FindAll()).Returns(new List<HomePermission> { notificationPermission });

        var result = homeService.CreateHome(home, ownerId);

        var memberId = Guid.NewGuid();
        var member = new User { Email = "blankEmail1@blank.com", Name = "blankName1", Surname = "blanckSurname1", Password = "blankPassword", Id = memberId, Role = homeOwnerRole };

        homeRepositoryMock.Setup(x => x.Update(It.IsAny<Home>())).Returns(home);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(member);

        HomeMember homeMember = homeService.AddHomeMemberToHome(home.Id, memberId);

        var businessOwnerRole = new Role { Name = "BusinessOwner" };
        var businessOwner = new User { Email = "blankEmail@blank.com", Name = "blankName", Surname = "blanckSurname", Password = "blankPassword", Id = new Guid(), Role = businessOwnerRole };
        var business = new Business { BusinessOwner = businessOwner, Id = Guid.NewGuid(), Name = "bName", Logo = "logo", RUT = "111222333" };
        var device = new Device { Name = "DeviceName", Business = business, Description = "DeviceDescription", Photos = "photo", ModelNumber = "a" };
        var homeDevice = new HomeDevice { Device = device, Id = Guid.NewGuid(), Online = true };

        home.Devices.Add(homeDevice);

        var homeOwner = result.Members.Find(x => x.User == owner);

        homeDeviceRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomeDevice, bool>>())).Returns(homeDevice);
        homeMemberRepositoryMock
            .SetupSequence(x => x.Find(It.IsAny<Func<HomeMember, bool>>()))
            .Returns(homeOwner)
            .Returns(homeMember);
        homePermissionRepositoryMock.Setup(x => x.Find(It.IsAny<Func<HomePermission, bool>>())).Returns(notificationPermission);

        // ACCTION

        homeService.CreateOpenCloseWindowNotification(homeDevice.Id, true);

        IEnumerable<HomeMember> homeMembers = (List<HomeMember>)homeService.GetAllHomeMembers(home.Id);
        var ownerFound = homeMembers.First(x => x.User == owner);
        var memberFound = homeMembers.First(x => x.User == member);

        // ASSERT

        Assert.IsTrue(ownerFound.Notifications.Count > 0);
        Assert.IsTrue(memberFound.Notifications.Count == 0);

        homeRepositoryMock.VerifyAll();
        userRepositoryMock.VerifyAll();
        homeRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void Create_Repeated_Adress_Home_Throws_Exception_Test()
    {
        var home = new Home { Id = Guid.NewGuid(), MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };
        var home2 = new Home { Id = Guid.NewGuid(), MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 3, Owner = owner };
        homeRepositoryMock.Setup(u => u.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        userRepositoryMock.Setup(u => u.Find(It.IsAny<Func<User, bool>>())).Returns(owner);
        Exception exception = null;

        try
        {
            homeService.CreateHome(home, ownerId);
            homeService.CreateHome(home2, ownerId);
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
        var home = new Home { Id = Guid.NewGuid(), MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };
        var memberId = Guid.NewGuid();
        var member = new User { Email = "blankEmail1@blank.com", Name = "blankName1", Surname = "blanckSurname1", Password = "blankPassword", Id = memberId, Role = homeOwnerRole };

        homeRepositoryMock.Setup(x => x.Update(It.IsAny<Home>())).Returns(home);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(owner);
        Exception exception = null;

        try
        {
            homeService.AddHomeMemberToHome(home.Id, memberId);
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
}
