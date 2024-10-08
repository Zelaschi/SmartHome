using Moq;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Services;

namespace SmartHome.BusinessLogicTest;

[TestClass]
public class BusinessServiceTest
{
    private Mock<IGenericRepository<Business>>? businessRepositoryMock;
    private Mock<IGenericRepository<User>>? userRepositoryMock;
    private BusinessService? businessService;

    [TestInitialize]

    public void Initialize()
    {
        businessRepositoryMock = new Mock<IGenericRepository<Business>>(MockBehavior.Strict);
        userRepositoryMock = new Mock<IGenericRepository<User>>(MockBehavior.Strict);
        businessService = new BusinessService(businessRepositoryMock.Object, userRepositoryMock.Object);
    }

    [TestMethod]

    public void GetAll_Businesses_Test()
    {
        var businessOwner = new Role { Name = "BusinessOwner" };
        var businesses = new List<Business>
        {
            new Business
            {
                Id = Guid.NewGuid(),
                Name = "HikVision",
                Logo = "Logo1",
                RUT = "1234",
                BusinessOwner = new User
                {
                    Name = "Juan",
                    Surname = "Perez",
                    Password = "Password@1234",
                    CreationDate = DateTime.Today,
                    Email = "juanperez@gmail.com",
                    Role = businessOwner
                }
            },
            new Business
            {
                Id = Guid.NewGuid(),
                Name = "Kolke",
                Logo = "Logo2",
                RUT = "5678",
                BusinessOwner = new User
                {
                    Name = "Pedro",
                    Surname = "Rodriguez",
                    Password = "Password@1234",
                    CreationDate = DateTime.Today,
                    Email = "pedrorodriguez@gmail.com",
                    Role = businessOwner
                }
            }
        };

        businessRepositoryMock.Setup(x => x.FindAll()).Returns(businesses);

        var businessesResult = businessService.GetAllBusinesses();

        businessRepositoryMock.VerifyAll();
        Assert.AreEqual(businesses, businessesResult);
    }

    [TestMethod]

    public void Create_Business_Test()
    {
        var businessOwner = new Role { Name = "BusinessOwner" };
        var business = new Business
        {
            Id = Guid.NewGuid(),
            Name = "HikVision",
            Logo = "Logo1",
            RUT = "1234",
        };
        var owner = new User
        {
            Id = Guid.NewGuid(),
            Name = "Pedro",
            Surname = "Rodriguez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "pedrorodriguez@gmail.com",
            Role = businessOwner,
            Complete = false
        };

        businessRepositoryMock.Setup(x => x.Add(business)).Returns(business);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(owner);

        userRepositoryMock.Setup(x => x.Update(It.Is<User>(u => u.Id == owner.Id))).Returns(owner);

        var businessResult = businessService.CreateBusiness(business, owner);

        businessRepositoryMock.Verify(x => x.Add(business), Times.Once);
        userRepositoryMock.Verify(x => x.Update(It.Is<User>(u => u.Id == owner.Id && u.Complete == true)), Times.Once);

        Assert.AreEqual(business, businessResult);
        Assert.IsTrue(owner.Complete);
        Assert.AreEqual(business.BusinessOwner, owner);
    }

    [TestMethod]

    public void Create_Business_With_Complete_Account_Throws_Exception_Test()
    {
        var businessOwner = new Role { Name = "BusinessOwner" };
        var business = new Business
        {
            Id = Guid.NewGuid(),
            Name = "HikVision",
            Logo = "Logo1",
            RUT = "1234",
        };
        var owner = new User
        {
            Id = Guid.NewGuid(),
            Name = "Pedro",
            Surname = "Rodriguez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "pedrorodriguez@gmail.com",
            Role = businessOwner,
            Complete = true,
        };

        try
        {
            businessService.CreateBusiness(business, owner);
        }
        catch (Exception e)
        {
            businessRepositoryMock.VerifyAll();
            Assert.IsInstanceOfType(e, typeof(UserException));
            Assert.AreEqual("User is already owner of a business", e.Message);
        }
    }
}
