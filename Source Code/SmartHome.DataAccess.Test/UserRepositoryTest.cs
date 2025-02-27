﻿using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
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
        _userRepository.Add(user);
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

        var user = new User
        {
            Name = "Test Name",
            Surname = "Test Surname",
            Password = "Test1Password123",
            Email = "test@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(user);
        _context.SaveChanges();

        var usersFound = _userRepository.FindAll();

        usersFound.Should().HaveCount(1);
        usersFound[0].Id.Should().Be(user.Id);
        usersFound[0].Name.Should().Be(user.Name);
        usersFound[0].Surname.Should().Be(user.Surname);
        usersFound[0].Password.Should().Be(user.Password);
        usersFound[0].Email.Should().Be(user.Email);
        usersFound[0].RoleId.Should().Be(user.RoleId);
    }
    #endregion

    #region Filtered
    [TestMethod]
    public void FindAllFiltered_ShouldReturnAllUsers_WhenNoFilterIsProvided()
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
        var user2 = new User
        {
            Name = "Test2 Name",
            Surname = "Test2 Surname",
            Password = "Test2Password123",
            Email = "test2@example.com",
            RoleId = role.Id
        };
        _userRepository.Add(user2);
        _userRepository.Add(user);
        _context.SaveChanges();

        var result = _userRepository.FindAllFiltered(null, 1, 10);
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Test2 Name");
    }

    [TestMethod]
    public void FindAllFiltered_ShouldReturnFilteredUsers_WhenFilterIsProvided()
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
        var user2 = new User
        {
            Name = "Test2 Name",
            Surname = "Test2 Surname",
            Password = "Test2Password123",
            Email = "test2@example.com",
            RoleId = role.Id
        };
        _userRepository.Add(user2);
        _userRepository.Add(user);
        _context.SaveChanges();

        var result = _userRepository.FindAllFiltered(u => u.Name == "Test Name", 1, 10);
        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Test Name");
    }

    [TestMethod]
    public void FindAllFiltered_OneParam_ShouldReturnAllUsers_WhenFilterIsProvided()
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

        var user2 = new User
        {
            Name = "Test Name 2",
            Surname = "Test Surname 2",
            Password = "TestPassword123",
            Email = "test2@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(user2);
        _context.Users.Add(user);
        _context.SaveChanges();

        var result = _userRepository.FindAllFiltered(u => u.Name == "Test Name");

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Test Name");
    }
    #endregion
}
