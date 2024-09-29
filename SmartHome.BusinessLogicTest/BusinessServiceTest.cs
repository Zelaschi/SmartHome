using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Services;

namespace SmartHome.BusinessLogicTest;

[TestClass]
public class BusinessServiceTest
{
    private Mock<IGenericRepository<Business>>? businessRepositoryMock;
    private BusinessService? businessService;

    [TestInitialize]

    public void Initialize()
    {
        businessRepositoryMock = new Mock<IGenericRepository<Business>>(MockBehavior.Strict);
        businessService = new BusinessService(businessRepositoryMock.Object);
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
}
