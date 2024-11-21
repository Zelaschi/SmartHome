using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
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
    [TestMethod]
    public void Update_WhenRoomExists_ShouldUpdateRoom()
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

        var updatedRoom = new Room
        {
            Id = room.Id,
            Home = home,
            Name = "Kitchen"
        };

        var result = _roomRepository.Update(updatedRoom);

        result.Should().NotBeNull();
        result.Id.Should().Be(room.Id);
        result.Name.Should().Be("Kitchen");

        var updatedEntityInDb = _context.Rooms.FirstOrDefault(r => r.Id == room.Id);
        updatedEntityInDb.Should().NotBeNull();
        updatedEntityInDb.Name.Should().Be("Kitchen");
    }

    [TestMethod]
    public void Update_WhenRoomDoesNotExist_ShouldThrowDatabaseException()
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

        var nonExistingRoom = new Room
        {
            Home = home,
            Name = "Kitchen"
        };

        Action action = () => _roomRepository.Update(nonExistingRoom);

        action.Should().Throw<DatabaseException>()
            .WithMessage("The Room does not exist in the Data Base.");
    }
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

    [TestMethod]
    public void Delete_WhenRoomDoesNotExist_ShouldThrowDatabaseException()
    {
        Action action = () => _roomRepository.Delete(Guid.NewGuid());
        action.Should().Throw<DatabaseException>()
            .WithMessage("The Room does not exist in the Data Base.");
    }
    #endregion

    #region GetAll
    [TestMethod]
    public void FindAll_WhenRoomsExist_ShouldReturnAllRooms()
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

        var rooms = _roomRepository.FindAll();

        rooms.Count.Should().Be(1);
        rooms[0].Id.Should().Be(room.Id);
        rooms[0].Name.Should().Be(room.Name);
    }
    #endregion

    #region Find
    [TestMethod]
    public void Find_WhenRoomExists_ShouldReturnRoom()
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

        var roomFound = _roomRepository.Find(r => r.Id == room.Id);

        roomFound.Should().NotBeNull();
        roomFound!.Id.Should().Be(room.Id);
        roomFound.Name.Should().Be(room.Name);
    }
    #endregion
}
