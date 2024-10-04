using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Services;

namespace SmartHome.BusinessLogicTest;

[TestClass]
public class HomeServiceTest
{
    private Mock<IGenericRepository<Home>>? homeRepositoryMock;
    private Mock<IGenericRepository<User>>? userRepositoryMock;
    private Mock<IGenericRepository<HomePermission>>? homePermissionRepositoryMock;
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
        homeService = new HomeService(homeRepositoryMock.Object, userRepositoryMock.Object, homePermissionRepositoryMock.Object);
        homeOwnerRole = new Role { Name = "HomeOwner" };
        ownerId = Guid.NewGuid();
        owner = new User { Email = "blankEmail@blank.com", Name = "blankName", Surname = "blanckSurname", Password = "blankPassword", Id = ownerId, Role = homeOwnerRole };
    }

    [TestMethod]
    public void Create_Home_OK_Test()
    {
        var home = new Home { Id = Guid.NewGuid(), MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };

        homeRepositoryMock.Setup(x => x.Add(It.IsAny<Home>())).Returns(home);
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
        var home = new Home { Devices =  new List<HomeDevice>(), Id = Guid.NewGuid(), MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner};
        var businessOwnerRole = new Role { Name = "BusinessOwner" };
        var businessOwner = new User { Email = "blankEmail@blank.com", Name = "blankName", Surname = "blanckSurname", Password = "blankPassword", Id = new Guid(), Role = businessOwnerRole };
        var business = new Business { BusinessOwner = businessOwner,Id = Guid.NewGuid(), Name = "bName", Logo = "logo", RUT = "111222333" };

        var device1 = new Device { Id = Guid.NewGuid(), Name = "Device1" , Description = "A Device", Business = business, ModelNumber = "123", Photos = "photos" };
        var device2 = new Device { Id = Guid.NewGuid(), Name = "Device2", Description = "A Device", Business = business, ModelNumber = "123", Photos = "photos" };

        var homeDevice1 = new HomeDevice { Device = device1, Id = Guid.NewGuid() , Online = true};
        var homeDevice2 = new HomeDevice { Device = device2, Id = Guid.NewGuid(), Online = false };

        home.Devices.Add(homeDevice1);
        home.Devices.Add(homeDevice2);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);

        IEnumerable<HomeDevice> homeDevices= homeService.GetAllHomeDevices(home.Id);
        homeRepositoryMock.VerifyAll();

        Assert.AreEqual(home.Devices.First() , homeDevices.First());
        Assert.AreEqual(home.Devices.Last(), homeDevices.Last());
    }
}
