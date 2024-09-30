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
    private Mock<IGenericRepository<HomeMember>>? homeMemberRepositoryMock;
    private HomeService? homeService;
    private Role? homeOwnerRole;

    [TestInitialize]
    public void Initialize()
    {
        homeMemberRepositoryMock = new Mock<IGenericRepository<HomeMember>>(MockBehavior.Strict);
        homeRepositoryMock = new Mock<IGenericRepository<Home>>(MockBehavior.Strict);
        homeService = new HomeService(homeRepositoryMock.Object, homeMemberRepositoryMock.Object);
        homeOwnerRole = new Role { Name = "HomeOwner" };
    }

    [TestMethod]
    public void Create_Home_OK_Test()
    {
        var owner = new User { Email = "blankEmail@blank.com", Name = "blankName", Surname = "blanckSurname", Password = "blankPassword", Id = new Guid(), Role = homeOwnerRole };
        var home = new Home { Id = Guid.NewGuid(), MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };

        homeRepositoryMock.Setup(x => x.Add(It.IsAny<Home>())).Returns(home);

        var result = homeService.CreateHome(home);

        homeRepositoryMock.VerifyAll();
        Assert.AreEqual(home, result);
    }
}
