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
    #endregion

    #region Find
    #endregion

    #region Update
    #endregion

    #region GetAll
    #endregion

}
