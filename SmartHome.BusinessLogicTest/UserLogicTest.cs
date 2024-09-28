using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Services;

namespace SmartHome.BusinessLogicTest;

[TestClass]
public class UserLogicTest
{
    private Mock<IGenericRepository<User>>? genericRepositoryMock;
    private UserService? userService;

    [TestInitialize]

    public void Initialize()
    {
        genericRepositoryMock = new Mock<IGenericRepository<User>>(MockBehavior.Strict);
        userService = new UserService(genericRepositoryMock.Object);
    }

    [TestMethod]

    public void CreateHomeOwnerTest_Ok()
    {
        var homeOwner = new User
        {
            Name = "Juan",
            Surname = "Perez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "juanperez@gmail.com",
            Role = new Role { Name = "HomeOwner" }
        };

        genericRepositoryMock.Setup(x => x.Add(homeOwner)).Returns(homeOwner);
        genericRepositoryMock.Setup(x => x.FindAll()).Returns(new List<User>());

        homeOwner.Id = Guid.NewGuid();
        var homeOwnerResult = userService.CreateHomeOwner(homeOwner);
        genericRepositoryMock.VerifyAll();
        Assert.AreEqual(homeOwner, homeOwnerResult);
    }
}
