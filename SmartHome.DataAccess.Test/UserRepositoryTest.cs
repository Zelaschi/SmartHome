using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Services;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;
using SmartHome.DataAccess.Repositories;

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
    #endregion

    #region Update
    #endregion

    #region Find
    #endregion

    #region GetAll
    #endregion
}
