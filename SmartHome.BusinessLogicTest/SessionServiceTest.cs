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
public class SessionServiceTest
{
    private Mock<IGenericRepository<User>>? userRepositoryMock;
    private Mock<IGenericRepository<Session>>? sessionRepositoryMock;
    private SessionService? sessionService;

    [TestInitialize]
    public void Initialize()
    {
        userRepositoryMock = new Mock<IGenericRepository<User>>(MockBehavior.Strict);
        sessionRepositoryMock = new Mock<IGenericRepository<Session>>(MockBehavior.Strict);
        sessionService = new SessionService(userRepositoryMock.Object, sessionRepositoryMock.Object);
    }

    [TestMethod]
    public void GetLogIn_WithValidCredentials_Test()
    {
        var userId = Guid.NewGuid();
        var existingUser = new User
        {
            Name = "Juan",
            Surname = "Perez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "juanperez@gmail.com",
            Id = userId
        };

        userRepositoryMock.Setup(repo => repo.FindAll())
            .Returns(new List<User> { existingUser });

        Session? sessionAdded = null;
        sessionRepositoryMock.Setup(repo => repo.Add(It.IsAny<Session>()))
            .Callback<Session>(s => sessionAdded = s)
            .Returns((Session s) => s);

        var result = sessionService.LogIn("juanperez@gmail.com", "Password@1234");

        Assert.IsNotNull(result);
        Assert.IsNotNull(sessionAdded);
        Assert.AreEqual(userId, sessionAdded.UserId);
        Assert.AreEqual(result, sessionAdded.SessionId);
        sessionRepositoryMock.Verify(repo => repo.Add(It.IsAny<Session>()), Times.Once);
    }
}
