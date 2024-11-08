using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Services;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;
using SmartHome.DataAccess.Repositories;
using ZstdSharp.Unsafe;

namespace SmartHome.DataAccess.Test;

[TestClass]
public class UserRepositoryTest
{
    private readonly SmartHomeEFCoreContext _context;
    private readonly UserRepository _userRepository;

    public UserRepositoryTest()
    {
        _context = DbContextBuilder.BuildTestDbContext();
        _userRepository = new UserRepository(_context);
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
    public void Add_WhenUserIsProvided_ShouldAddedToDatabase()
    {
        _context.Users.RemoveRange(_context.Users);
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin"
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

        using var otherContext = DbContextBuilder.BuildTestDbContext();
        var usersSaved = otherContext.Users.ToList();

        usersSaved.Count.Should().Be(1);
        var userSaved = usersSaved[0];
        userSaved.Id.Should().Be(user.Id);
        userSaved.Name.Should().Be(user.Name);
        userSaved.Surname.Should().Be(user.Surname);
        userSaved.Password.Should().Be(user.Password);
        userSaved.Email.Should().Be(user.Email);
        userSaved.RoleId.Should().Be(user.RoleId);
    }
    #endregion

    #region Delete
    [TestMethod]
    public void Delete_WhenUserIsProvided_ShouldDeleteFromDataBase()
    {
        _context.Users.RemoveRange(_context.Users);
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin"
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

        _userRepository.Delete((Guid)user.Id);

        _context.Users.FirstOrDefault(u => u.Id == user.Id).Should().BeNull();
    }

    [TestMethod]
    public void Delete_WhenUserIsNotProvided_ShouldThrowDatabaseException()
    {
        _context.Users.RemoveRange(_context.Users);
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin"
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

        Action act = () => _userRepository.Delete(Guid.NewGuid());

        act.Should().Throw<DatabaseException>()
            .WithMessage("The User does not exist in the Data Base.");
    }
    #endregion

    #region Update
    [TestMethod]
    public void Update_WhenUserIsFound_ShouldUpdateDataBase()
    {
        _context.Users.RemoveRange(_context.Users);
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin"
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

        var userUpdated = new User
        {
            Id = user.Id,
            Name = "Updated Name",
            Surname = "Updated Surname",
            Password = "UpdatedPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };
        _userRepository.Update(userUpdated);
        _context.SaveChanges();

        using var otherContext = DbContextBuilder.BuildTestDbContext();
        var usersSaved = otherContext.Users.ToList();

        usersSaved.Count.Should().Be(1);
        var userSaved = usersSaved[0];
        userSaved.Id.Should().Be(user.Id);
        userSaved.Name.Should().Be(userUpdated.Name);
        userSaved.Password.Should().Be(user.Password);
    }

    [TestMethod]
    public void Update_WhenUserIsNotFound_ShouldThrowDatabaseException()
    {
        _context.Users.RemoveRange(_context.Users);
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin"
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

        var userUpdated = new User
        {
            Id = Guid.NewGuid(),
            Name = "Updated Name",
            Surname = "Updated Surname",
            Password = "UpdatedPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };

        Action act = () => _userRepository.Update(userUpdated);

        act.Should().Throw<DatabaseException>()
            .WithMessage("The User does not exist in the Data Base.");
    }

    #endregion

    #region Find
    [TestMethod]
    public void Find_WhenUserIsFound_ShouldReturnUser()
    {
        _context.Users.RemoveRange(_context.Users);
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin"
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

        var userFound = _userRepository.Find(u => u.Id == user.Id);

        userFound.Should().NotBeNull();
        userFound!.Id.Should().Be(user.Id);
        userFound.Name.Should().Be(user.Name);
        userFound.Surname.Should().Be(user.Surname);
        userFound.Password.Should().Be(user.Password);
        userFound.Email.Should().Be(user.Email);
        userFound.RoleId.Should().Be(user.RoleId);
    }
    #endregion

    #region GetAll
    [TestMethod]
    public void GetAll_WhenUsersAreFound_ShouldReturnUsers()
    {
        _context.Users.RemoveRange(_context.Users);
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        var user1 = new User
        {
            Name = "Test Name1",
            Surname = "Test Surname1",
            Password = "Test1Password123",
            Email = "test1@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(user1);
        _context.SaveChanges();

        var user2 = new User
        {
            Name = "Test Name2",
            Surname = "Test Surname2",
            Password = "Test2Password123",
            Email = "test2@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(user2);
        _context.SaveChanges();

        var usersFound = _userRepository.FindAll();

        usersFound.Should().HaveCount(2);
        usersFound[0].Id.Should().Be(user1.Id);
        usersFound[0].Name.Should().Be(user1.Name);
        usersFound[0].Surname.Should().Be(user1.Surname);
        usersFound[0].Password.Should().Be(user1.Password);
        usersFound[0].Email.Should().Be(user1.Email);
        usersFound[0].RoleId.Should().Be(user1.RoleId);
        usersFound[1].Id.Should().Be(user2.Id);
        usersFound[1].Name.Should().Be(user2.Name);
        usersFound[1].Surname.Should().Be(user2.Surname);
        usersFound[1].Password.Should().Be(user2.Password);
        usersFound[1].Email.Should().Be(user2.Email);
        usersFound[1].RoleId.Should().Be(user2.RoleId);
    }

    #endregion
}
