using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Services;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Test;

[TestClass]
public class SessionRepositoryTest
{
    private readonly SmartHomeEFCoreContext _context;
    private readonly SessionRepository _sessionRepository;

    public SessionRepositoryTest()
    {
        _context = DbContextBuilder.BuildTestDbContext();
        _sessionRepository = new SessionRepository(_context);
    }

    [TestInitialize]
    public void Setup()
    {
        _context.Database.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    #region Add
    [TestMethod]
    public void Add_SessionEntity_ShouldReturnSessionEntity()
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "User"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        var user = new User
        {
            Name = "Test Name",
            Surname = "Test Surname",
            Password = "TestPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(user);
        _context.SaveChanges();

        var session = new Session
        {
            SessionId = Guid.NewGuid(),
            UserId = (Guid)user.Id
        };
        _sessionRepository.Add(session);
        _context.SaveChanges();

        var sessionsSaved = _context.Sessions.ToList();
        sessionsSaved.Count.Should().Be(1);

        var sessionSaved = sessionsSaved[0];
        sessionSaved.SessionId.Should().Be(session.SessionId);
        sessionSaved.UserId.Should().Be(session.UserId);
    }
    #endregion

    #region Delete
    [TestMethod]
    public void Delete_SessionEntity_ShouldDeleteCorrectly()
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "User"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        var user = new User
        {
            Name = "Test Name",
            Surname = "Test Surname",
            Password = "TestPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(user);
        _context.SaveChanges();

        var session = new Session
        {
            SessionId = Guid.NewGuid(),
            UserId = (Guid)user.Id
        };
        _sessionRepository.Add(session);
        _context.SaveChanges();

        _sessionRepository.Delete(session.SessionId);

        _context.Sessions.FirstOrDefault(s => s.SessionId == session.SessionId).Should().BeNull();
    }

    [TestMethod]
    public void Delete_SessionEntityNotFound_ShouldThrowException()
    {
        Action action = () => _sessionRepository.Delete(Guid.NewGuid());

        action.Should().Throw<DatabaseException>()
            .WithMessage("The Session does not exist in the Data Base.");
    }
    #endregion

    #region Update
    [TestMethod]
    public void Update_SessionEntity_ShouldUpdateCorrectly()
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "User"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        var user = new User
        {
            Name = "Test Name",
            Surname = "Test Surname",
            Password = "TestPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(user);
        _context.SaveChanges();

        var user2 = new User
        {
            Name = "Test Name",
            Surname = "Test Surname",
            Password = "TestPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(user2);
        _context.SaveChanges();

        var session = new Session
        {
            SessionId = Guid.NewGuid(),
            UserId = (Guid)user.Id
        };
        _sessionRepository.Add(session);
        _context.SaveChanges();

        var sessionToUpdate = _context.Sessions.FirstOrDefault(s => s.SessionId == session.SessionId);
        sessionToUpdate.UserId = (Guid)user2.Id;

        var result = _sessionRepository.Update(sessionToUpdate);

        result.Should().NotBeNull();
        result.UserId.Should().Be(session.UserId);
        result.SessionId.Should().Be(sessionToUpdate.SessionId);

        var updatedEntityInDb = _context.Sessions.FirstOrDefault(s => s.SessionId == session.SessionId);
        updatedEntityInDb.Should().NotBeNull();
        updatedEntityInDb.UserId.Should().Be(session.UserId);
        updatedEntityInDb.SessionId.Should().Be(sessionToUpdate.SessionId);
    }

    [TestMethod]
    public void Update_SessionDoesNotExist_ShouldThrowDatabaseException()
    {
        Action act = () => _sessionRepository.Update(new Session { SessionId = Guid.NewGuid(), UserId = Guid.NewGuid() });

        act.Should().Throw<DatabaseException>()
            .WithMessage("The Session does not exist in the Data Base.");
    }
    #endregion

    #region Find
    [TestMethod]
    public void Find_SessionEntity_ShouldReturnSessionEntity()
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "User"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        var user = new User
        {
            Name = "Test Name",
            Surname = "Test Surname",
            Password = "TestPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(user);
        _context.SaveChanges();

        var session = new Session
        {
            SessionId = Guid.NewGuid(),
            UserId = (Guid)user.Id
        };
        _sessionRepository.Add(session);
        _context.SaveChanges();

        var result = _sessionRepository.Find(s => s.SessionId == session.SessionId);

        result.Should().NotBeNull();
        result!.SessionId.Should().Be(session.SessionId);
        result.UserId.Should().Be(session.UserId);
    }
    #endregion

    #region GetAll
    [TestMethod]
    public void GetAll_ShouldReturnAllSessionEntities()
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "User"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        var user = new User
        {
            Name = "Test Name",
            Surname = "Test Surname",
            Password = "TestPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(user);
        _context.SaveChanges();

        var session = new Session
        {
            SessionId = Guid.NewGuid(),
            UserId = (Guid)user.Id
        };
        _sessionRepository.Add(session);
        _context.SaveChanges();

        var user2 = new User
        {
            Name = "Test Name2",
            Surname = "Test Surname2",
            Password = "TestPassword123",
            Email = "test2@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(user2);
        _context.SaveChanges();

        var session2 = new Session
        {
            SessionId = Guid.NewGuid(),
            UserId = (Guid)user2.Id
        };
        _sessionRepository.Add(session2);
        _context.SaveChanges();

        var sessions = _sessionRepository.FindAll().ToList();

        sessions.Count.Should().Be(2);
        sessions.Should().Contain(session);
        sessions.Should().Contain(session2);
    }
    #endregion
}
