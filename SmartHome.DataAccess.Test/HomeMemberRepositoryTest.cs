using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Test;
[TestClass]
public class HomeMemberRepositoryTest
{
    private readonly SmartHomeEFCoreContext _context;
    private readonly HomeMemberRepository _homeMemberRepository;

    public HomeMemberRepositoryTest()
    {
        _context = DbContextBuilder.BuildTestDbContext();
        _homeMemberRepository = new HomeMemberRepository(_context);
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
    public void Add_WhenInfoIsProvided_ShouldAddedToDatabase()
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "User Role"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        var owner = new User
        {
            Name = "Owner Name",
            Surname = "Owner Surname",
            Password = "TestPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(owner);
        _context.SaveChanges();

        var user = new User
        {
            Name = "User Name",
            Surname = "User Surname",
            Password = "TestPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(user);
        _context.SaveChanges();

        var home = new Home
        {
            Id = Guid.NewGuid(),
            Name = "Test Home",
            MainStreet = "Test Street",
            DoorNumber = "123",
            Latitude = "0.0000",
            Longitude = "0.0000",
            MaxMembers = 4,
            Owner = owner
        };
        _context.Homes.Add(home);
        _context.SaveChanges();

        var homeMember = new HomeMember(user)
        {
            HomeId = home.Id
        };
        _homeMemberRepository.Add(homeMember);

        using var otherContext = DbContextBuilder.BuildTestDbContext();
        var homeMembersSaved = otherContext.HomeMembers
            .Include(hm => hm.User)
            .ThenInclude(u => u.Role)
            .ToList();

        homeMembersSaved.Count.Should().Be(1);
        var homeMemberSaved = homeMembersSaved[0];
        homeMemberSaved.User.Id.Should().Be(homeMember.User.Id);
        homeMemberSaved.HomeMemberId.Should().Be(homeMember.HomeMemberId);
    }
    #endregion

    #region Delete
    [TestMethod]
    public void Delete_WhenHomeMemberExists_ShouldRemoveFromDatabase()
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "User Role"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        var owner = new User
        {
            Name = "Owner Name",
            Surname = "Owner Surname",
            Password = "TestPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(owner);
        _context.SaveChanges();

        var user = new User
        {
            Name = "User Name",
            Surname = "User Surname",
            Password = "TestPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(user);
        _context.SaveChanges();

        var home = new Home
        {
            Id = Guid.NewGuid(),
            Name = "Test Home",
            MainStreet = "Test Street",
            DoorNumber = "123",
            Latitude = "0.0000",
            Longitude = "0.0000",
            MaxMembers = 4,
            Owner = owner
        };
        _context.Homes.Add(home);
        _context.SaveChanges();

        var homeMember = new HomeMember(user)
        {
            HomeId = home.Id
        };
        _homeMemberRepository.Add(homeMember);
        _context.SaveChanges();

        _homeMemberRepository.Delete(homeMember.HomeMemberId);

        _context.HomeMembers.FirstOrDefault(hm => hm.HomeMemberId == homeMember.HomeMemberId).Should().BeNull();
    }

    [TestMethod]
    public void Delete_WhenHomeMemberDoesNotExists_ShouldThrowException()
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "User Role"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        var owner = new User
        {
            Name = "Owner Name",
            Surname = "Owner Surname",
            Password = "TestPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(owner);
        _context.SaveChanges();

        var user = new User
        {
            Name = "User Name",
            Surname = "User Surname",
            Password = "TestPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(user);
        _context.SaveChanges();

        var home = new Home
        {
            Id = Guid.NewGuid(),
            Name = "Test Home",
            MainStreet = "Test Street",
            DoorNumber = "123",
            Latitude = "0.0000",
            Longitude = "0.0000",
            MaxMembers = 4,
            Owner = owner
        };
        _context.Homes.Add(home);
        _context.SaveChanges();

        var homeMember = new HomeMember(user)
        {
            HomeId = home.Id
        };

        Action action = () => _homeMemberRepository.Delete(homeMember.HomeMemberId);

        action.Should().Throw<DatabaseException>()
            .WithMessage("The HomeMember does not exist in the Data Base.");
    }

    #endregion

    #region Find
    [TestMethod]
    public void Find_WhenHomeMemberExists_ShouldReturnHomeMember()
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "User Role"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        var owner = new User
        {
            Name = "Owner Name",
            Surname = "Owner Surname",
            Password = "TestPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(owner);
        _context.SaveChanges();

        var user = new User
        {
            Name = "User Name",
            Surname = "User Surname",
            Password = "TestPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(user);
        _context.SaveChanges();

        var home = new Home
        {
            Id = Guid.NewGuid(),
            Name = "Test Home",
            MainStreet = "Test Street",
            DoorNumber = "123",
            Latitude = "0.0000",
            Longitude = "0.0000",
            MaxMembers = 4,
            Owner = owner
        };
        _context.Homes.Add(home);
        _context.SaveChanges();

        var homeMember = new HomeMember(user)
        {
            HomeId = home.Id
        };
        _homeMemberRepository.Add(homeMember);
        _context.SaveChanges();

        var result = _homeMemberRepository.Find(hm => hm.HomeMemberId == homeMember.HomeMemberId);

        result.Should().NotBeNull();
        result.HomeMemberId.Should().Be(homeMember.HomeMemberId);
        result.User.Name.Should().Be(homeMember.User.Name);
    }
    #endregion

    #region Update
    [TestMethod]
    public void Update_WhenHomeMemberExists_ShouldUpdateInDataBase()
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "User Role"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        var owner = new User
        {
            Name = "Owner Name",
            Surname = "Owner Surname",
            Password = "TestPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(owner);
        _context.SaveChanges();

        var user = new User
        {
            Name = "User Name",
            Surname = "User Surname",
            Password = "TestPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(user);
        _context.SaveChanges();

        var home = new Home
        {
            Id = Guid.NewGuid(),
            Name = "Test Home",
            MainStreet = "Test Street",
            DoorNumber = "123",
            Latitude = "0.0000",
            Longitude = "0.0000",
            MaxMembers = 4,
            Owner = owner
        };
        _context.Homes.Add(home);
        _context.SaveChanges();

        var homeMember = new HomeMember(user)
        {
            HomeId = home.Id
        };
        _homeMemberRepository.Add(homeMember);
        _context.SaveChanges();

        user.Name = "Updated User Name";
        user.Surname = "Updated User Surname";
        user.Password = "Updated TestPassword123";
        user.Email = "Updated test@example.com";
        _context.Users.Update(user);
        _context.SaveChanges();

        var updatedHomeMember = new HomeMember
        {
            HomeMemberId = homeMember.HomeMemberId,
            HomeId = home.Id,
            User = user
        };

        var result = _homeMemberRepository.Update(updatedHomeMember);

        result.Should().NotBeNull();
        result.HomeMemberId.Should().Be(updatedHomeMember.HomeMemberId);
        result.User.Name.Should().Be("Updated User Name");

        var updatedEntityInDb = _context.HomeMembers
            .Include(hm => hm.User)
            .FirstOrDefault(hm => hm.HomeMemberId == homeMember.HomeMemberId);

        updatedEntityInDb.Should().NotBeNull();
        updatedEntityInDb.User.Name.Should().Be("Updated User Name");
    }

    [TestMethod]
    public void Update_WhenHomeMemberDoesNotExist_ShouldThrowDatabaseException()
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "User Role"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        var owner = new User
        {
            Name = "Owner Name",
            Surname = "Owner Surname",
            Password = "TestPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(owner);
        _context.SaveChanges();

        var user = new User
        {
            Name = "User Name",
            Surname = "User Surname",
            Password = "TestPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(user);
        _context.SaveChanges();

        var home = new Home
        {
            Id = Guid.NewGuid(),
            Name = "Test Home",
            MainStreet = "Test Street",
            DoorNumber = "123",
            Latitude = "0.0000",
            Longitude = "0.0000",
            MaxMembers = 4,
            Owner = owner
        };
        _context.Homes.Add(home);
        _context.SaveChanges();

        var homeMember = new HomeMember(user)
        {
            HomeId = home.Id
        };
        _homeMemberRepository.Add(homeMember);
        _context.SaveChanges();

        user.Name = "Updated User Name";
        user.Surname = "Updated User Surname";
        user.Password = "Updated TestPassword123";
        user.Email = "Updated test@example.com";
        _context.Users.Update(user);
        _context.SaveChanges();

        var nonExistingHomeMember = new HomeMember
        {
            HomeMemberId = Guid.NewGuid(),
            HomeId = home.Id,
            User = user
        };

        Action action = () => _homeMemberRepository.Update(nonExistingHomeMember);

        action.Should().Throw<DatabaseException>()
            .WithMessage("The HomeMember does not exist in the Data Base.");
    }
    #endregion

    #region FindAll
    [TestMethod]
    public void FindAll_WhenExistOnlyOne_ShouldReturnOneHomeMember()
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "User Role"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        var owner = new User
        {
            Name = "Owner Name",
            Surname = "Owner Surname",
            Password = "TestPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(owner);
        _context.SaveChanges();

        var user = new User
        {
            Name = "User Name",
            Surname = "User Surname",
            Password = "TestPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(user);
        _context.SaveChanges();

        var home = new Home
        {
            Id = Guid.NewGuid(),
            Name = "Test Home",
            MainStreet = "Test Street",
            DoorNumber = "123",
            Latitude = "0.0000",
            Longitude = "0.0000",
            MaxMembers = 4,
            Owner = owner
        };
        _context.Homes.Add(home);
        _context.SaveChanges();

        var homeMember = new HomeMember(user)
        {
            HomeId = home.Id
        };
        _homeMemberRepository.Add(homeMember);
        _context.SaveChanges();

        var homeMembers = _homeMemberRepository.FindAll();

        homeMembers.Count.Should().Be(1);
        homeMembers[0].HomeMemberId.Should().Be(homeMember.HomeMemberId);
        homeMembers[0].User.Name.Should().Be(homeMember.User.Name);
    }
    #endregion

    #region UpdateMultipleElements

    [TestMethod]
    public void UpdateMultiplElements_WhenAllHomeMembersExist_ShouldUpdateInDatabase()
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "User Role"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        var owner = new User
        {
            Name = "Owner Name",
            Surname = "Owner Surname",
            Password = "TestPassword123",
            Email = "owner@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(owner);
        _context.SaveChanges();

        var user1 = new User
        {
            Name = "User1",
            Surname = "Surname1",
            Password = "Pass1",
            Email = "user1@example.com",
            RoleId = role.Id
        };
        var user2 = new User
        {
            Name = "User2",
            Surname = "Surname2",
            Password = "Pass2",
            Email = "user2@example.com",
            RoleId = role.Id
        };
        _context.Users.AddRange(user1, user2);
        _context.SaveChanges();

        var home = new Home
        {
            Id = Guid.NewGuid(),
            Name = "Test Home",
            MainStreet = "Test Street",
            DoorNumber = "123",
            Latitude = "0.0000",
            Longitude = "0.0000",
            MaxMembers = 4,
            Owner = owner
        };
        _context.Homes.Add(home);
        _context.SaveChanges();

        var homeMember1 = new HomeMember(user1)
        {
            HomeId = home.Id
        };
        var homeMember2 = new HomeMember(user2)
        {
            HomeId = home.Id
        };
        _homeMemberRepository.Add(homeMember1);
        _homeMemberRepository.Add(homeMember2);
        _context.SaveChanges();

        homeMember1.User.Name = "Updated User1";
        homeMember2.User.Name = "Updated User2";

        var updatedEntities = new List<HomeMember>
        {
            homeMember1,
            homeMember2
        };

        var result = _homeMemberRepository.UpdateMultiplElements(updatedEntities);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].User.Name.Should().Be("Updated User1");
        result[1].User.Name.Should().Be("Updated User2");

        var updatedMember1 = _context.HomeMembers
            .Include(hm => hm.User)
            .FirstOrDefault(hm => hm.HomeMemberId == homeMember1.HomeMemberId);
        var updatedMember2 = _context.HomeMembers
            .Include(hm => hm.User)
            .FirstOrDefault(hm => hm.HomeMemberId == homeMember2.HomeMemberId);

        updatedMember1.Should().NotBeNull();
        updatedMember1.User.Name.Should().Be("Updated User1");

        updatedMember2.Should().NotBeNull();
        updatedMember2.User.Name.Should().Be("Updated User2");
    }

    [TestMethod]
    public void UpdateMultiplElements_WhenAnyHomeMemberDoesNotExist_ShouldThrowDatabaseException()
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "User Role"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        var user = new User
        {
            Name = "User",
            Surname = "Surname",
            Password = "Password",
            Email = "user@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(user);
        _context.SaveChanges();

        var home = new Home
        {
            Id = Guid.NewGuid(),
            Name = "Test Home",
            MainStreet = "Test Street",
            DoorNumber = "123",
            Latitude = "0.0000",
            Longitude = "0.0000",
            MaxMembers = 4,
            Owner = user
        };
        _context.Homes.Add(home);
        _context.SaveChanges();

        var homeMember = new HomeMember(user)
        {
            HomeId = home.Id
        };
        _homeMemberRepository.Add(homeMember);
        _context.SaveChanges();

        var nonExistingHomeMember = new HomeMember
        {
            HomeMemberId = Guid.NewGuid(),
            HomeId = home.Id,
            User = user
        };

        var updatedEntities = new List<HomeMember>
        {
            homeMember,
            nonExistingHomeMember
        };

        Action action = () => _homeMemberRepository.UpdateMultiplElements(updatedEntities);

        action.Should().Throw<DatabaseException>()
            .WithMessage("The HomeMember does not exist in the Data Base.");
    }
    #endregion
}
