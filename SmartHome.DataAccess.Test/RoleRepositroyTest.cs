using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Services;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Test;

[TestClass]
public class RoleRepositoryTest
{
    private readonly SmartHomeEFCoreContext _context;
    private readonly RoleRepository _roleRepository;

    public RoleRepositoryTest()
    {
        _context = DbContextBuilder.BuildTestDbContext();
        _roleRepository = new RoleRepository(_context);
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
    public void AddRole_WhenRoleIsAdded_ShouldReturnRole()
    {
        _context.Roles.RemoveRange(_context.Roles);
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        using var otherContext = DbContextBuilder.BuildTestDbContext();
        var rolesSaved = otherContext.Roles.ToList();

        rolesSaved.Count.Should().Be(1);
        var roleSaved = rolesSaved[0];
        roleSaved.Id.Should().Be(role.Id);
        roleSaved.Name.Should().Be(role.Name);
    }
    #endregion

    #region Delete
    [TestMethod]
    public void Delete_WhenRoleExists_ShouldRemoveFromDatabase()
    {
        _context.Roles.RemoveRange(_context.Roles);
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        _roleRepository.Delete(role.Id);

        _context.Roles.FirstOrDefault(r => r.Id == role.Id).Should().BeNull();
    }

    [TestMethod]
    public void Delete_WhenRoleDoesNotExist_ShouldThrowDatabaseException()
    {
        _context.Roles.RemoveRange(_context.Roles);
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        Action action = () => _roleRepository.Delete(Guid.NewGuid());
        action.Should().Throw<DatabaseException>()
            .WithMessage("The Role does not exist in the Data Base.");
    }
    #endregion

    #region Find
    [TestMethod]
    public void Find_WhenRoleExists_ShouldReturnRole()
    {
        _context.Roles.RemoveRange(_context.Roles);
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        var roleFound = _roleRepository.Find(r => r.Id == role.Id);

        roleFound.Should().NotBeNull();
        roleFound!.Id.Should().Be(role.Id);
        roleFound.Name.Should().Be(role.Name);
    }
    #endregion

    #region Update
    [TestMethod]
    public void Update_WhenRoleExists_ShouldUpdateRole()
    {
        _context.Roles.RemoveRange(_context.Roles);
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        var roleToUpdate = new Role
        {
            Id = role.Id,
            Name = "User"
        };

        var result = _roleRepository.Update(roleToUpdate);

        result.Should().NotBeNull();
        result.Id.Should().Be(role.Id);
        result.Name.Should().Be("User");

        var updatedEntityInDb = _context.Roles.FirstOrDefault(r => r.Id == role.Id);
        updatedEntityInDb.Should().NotBeNull();
        updatedEntityInDb.Name.Should().Be("User");
    }

    [TestMethod]
    public void Update_WhenRoleDoesNotExist_ShouldThrowDatabaseException()
    {
        _context.Roles.RemoveRange(_context.Roles);
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        var roleToUpdate = new Role
        {
            Id = Guid.NewGuid(),
            Name = "User"
        };

        Action action = () => _roleRepository.Update(roleToUpdate);
        action.Should().Throw<DatabaseException>()
            .WithMessage("The Role does not exist in the Data Base.");
    }
    #endregion

    #region GetAll
    [TestMethod]
    public void FindAll_WhenExistOnlyOne_ShouldReturnOne()
    {
        _context.Roles.RemoveRange(_context.Roles);
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin"
        };
        _roleRepository.Add(role);
        _context.SaveChanges();

        var roles = _roleRepository.FindAll();

        roles.Count.Should().Be(1);
        var roleFound = roles[0];
        roleFound.Id.Should().Be(role.Id);
        roleFound.Name.Should().Be(role.Name);
    }
    #endregion
}
