﻿using Moq;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Services;

namespace SmartHome.BusinessLogic.Test;

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
        var sp = new SystemPermission
        {
            Name = "READ",
            Description = "WRITE"
        };
        var role = new Role
        {
            Name = "Admin"
        };
        role.SystemPermissions.Add(sp);

        var existingUser = new User
        {
            Name = "Juan",
            Surname = "Perez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "juanperez@gmail.com",
            Id = userId,
            Role = role
        };

        var list = new List<User> { existingUser };

        userRepositoryMock.Setup(repo => repo.FindAll())
            .Returns(list);

        Session? sessionAdded = null;
        sessionRepositoryMock.Setup(repo => repo.Add(It.IsAny<Session>()))
            .Callback<Session>(s => sessionAdded = s)
            .Returns((Session s) => s);

        var result = sessionService.LogIn("juanperez@gmail.com", "Password@1234");

        Assert.IsNotNull(result);
        Assert.IsNotNull(sessionAdded);
        Assert.AreEqual(userId, sessionAdded.UserId);
        Assert.AreEqual(result.SessionId, sessionAdded.SessionId);
        sessionRepositoryMock.Verify(repo => repo.Add(It.IsAny<Session>()), Times.Once);
    }

    [TestMethod]
    public void Get_UserOfSession_Test()
    {
        var token = Guid.NewGuid();
        var session = new Session
        {
            SessionId = token,
            UserId = Guid.NewGuid()
        };
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

        sessionRepositoryMock.Setup(repo => repo.Find(It.IsAny<Func<Session, bool>>()))
                              .Returns(session);
        userRepositoryMock.Setup(repo => repo.Find(It.IsAny<Func<User, bool>>()))
                           .Returns(existingUser);

        var result = sessionService.GetUserOfSession(token);

        Assert.IsNotNull(result);
        Assert.AreEqual(existingUser.Id, result.Id);
    }

    [TestMethod]
    public void Get_UserOfSession_SessionNotFound_Throws_Exception()
    {
        var token = Guid.NewGuid();
        sessionRepositoryMock.Setup(repo => repo.Find(It.IsAny<Func<Session, bool>>()))
                              .Returns((Session)null);

        Exception exception = null;

        try
        {
            var user = sessionService.GetUserOfSession(token);
        }
        catch (Exception e)
        {
            exception = e;
        }

        sessionRepositoryMock.VerifyAll();
        userRepositoryMock.VerifyAll();
        Assert.IsInstanceOfType(exception, typeof(SessionException));
        Assert.AreEqual("The session with token: " + token + " was not found", exception.Message);
    }

    [TestMethod]
    public void Get_UserOfSession_UserNotFound_Throws_Exception()
    {
        var token = Guid.NewGuid();
        var session = new Session
        {
            SessionId = token,
            UserId = Guid.NewGuid()
        };

        sessionRepositoryMock.Setup(repo => repo.Find(It.IsAny<Func<Session, bool>>()))
                              .Returns(session);
        userRepositoryMock.Setup(repo => repo.Find(It.IsAny<Func<User, bool>>()))
                           .Returns((User)null);

        Exception exception = null;

        try
        {
            var user = sessionService.GetUserOfSession(token);
        }
        catch (Exception e)
        {
            exception = e;
        }

        sessionRepositoryMock.VerifyAll();
        userRepositoryMock.VerifyAll();
        Assert.IsInstanceOfType(exception, typeof(SessionException));
        Assert.AreEqual("The user of the session with token: " + token + " was not found", exception.Message);
    }

    [TestMethod]
    public void IsSessionValid_Session_Not_Valid()
    {
        var token = Guid.NewGuid();
        sessionRepositoryMock.Setup(repo => repo.Find(It.IsAny<Func<Session, bool>>()))
                              .Returns((Session)null);

        var result = sessionService.IsSessionValid(token);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsSessionValid_Session_Valid()
    {
        var token = Guid.NewGuid();
        var session = new Session
        {
            SessionId = token,
            UserId = Guid.NewGuid()
        };

        sessionRepositoryMock.Setup(repo => repo.Find(It.IsAny<Func<Session, bool>>()))
                              .Returns(session);

        var result = sessionService.IsSessionValid(token);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void LogIn_User_Not_Found_Throws_Exception()
    {
        userRepositoryMock.Setup(repo => repo.FindAll())
                          .Returns(new List<User>());

        Exception exception = null;

        try
        {
            var result = sessionService.LogIn("mail@gmail.com", "password");
        }
        catch (Exception e)
        {
            exception = e;
        }

        userRepositoryMock.VerifyAll();
        Assert.IsInstanceOfType(exception, typeof(UserException));
        Assert.AreEqual("User with that email and password does not exist", exception.Message);
    }
}
