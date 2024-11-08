using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Services;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Test;

[TestClass]
public class RoomRepositoryTest
{
    private readonly SmartHomeEFCoreContext _context;
    private readonly RoomRepository _roomRepository;

    public RoomRepositoryTest()
    {
        _context = DbContextBuilder.BuildTestDbContext();
        _roomRepository = new RoomRepository(_context);
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
    public void Add_WhenRoomIsAdded_ShouldReturnRoom()
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
            Name = "Test Name",
            Surname = "Test Surname",
            Password = "TestPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(owner);
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

        var room = new Room
        {
            Home = home,
            Name = "Living Room",
        };
        _roomRepository.Add(room);
        _context.SaveChanges();

        using var otherContext = DbContextBuilder.BuildTestDbContext();
        var roomsSaved = otherContext.Rooms.ToList();

        roomsSaved.Count.Should().Be(1);
        var roomSaved = roomsSaved[0];
        roomSaved.Id.Should().Be(roomSaved.Id);
        roomSaved.Name.Should().Be(roomSaved.Name);
    }
    #endregion

    #region Update
    #endregion

    #region Delete
    [TestMethod]
    public void Delete_WhenRoomExists_ShouldDeleteRoom()
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
            Name = "Test Name",
            Surname = "Test Surname",
            Password = "TestPassword123",
            Email = "test@example.com",
            RoleId = role.Id
        };
        _context.Users.Add(owner);
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

        var room = new Room
        {
            Home = home,
            Name = "Living Room",
        };
        _roomRepository.Add(room);
        _context.SaveChanges();

        _roomRepository.Delete(room.Id);

        _context.Rooms.FirstOrDefault(r => r.Id == room.Id).Should().BeNull();
    }
    #endregion

    #region GetAll
    #endregion

    #region Find
    #endregion
}
