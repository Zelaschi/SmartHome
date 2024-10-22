using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Moq;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.ExtraRepositoryInterfaces;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Services;

namespace SmartHome.BusinessLogicTest;

[TestClass]
public class RoleServiceTest
{
    private Mock<IGenericRepository<Home>>? homeRepositoryMock;
    private Mock<IGenericRepository<User>>? userRepositoryMock;
    private Mock<IGenericRepository<HomePermission>>? homePermissionRepositoryMock;
    private Mock<IGenericRepository<HomeDevice>>? homeDeviceRepositoryMock;
    private Mock<IGenericRepository<HomeMember>>? homeMemberRepositoryMock;
    private Mock<IGenericRepository<Device>>? deviceRepositoryMock;
    private Mock<IGenericRepository<Role>>? roleRepositoryMock;
    private Mock<IHomesFromUserRepository>? homesFromUserRepositoryMock;
    private Mock<IGenericRepository<SystemPermission>>? systemPermissionRepositoryMock;
    private HomeService? homeService;
    private RoleService? roleService;
    private Role? homeOwnerRole;
    private Guid ownerId;
    private User? owner;

    [TestInitialize]
    public void Initialize()
    {
        systemPermissionRepositoryMock = new Mock<IGenericRepository<SystemPermission>>(MockBehavior.Strict);
        homeRepositoryMock = new Mock<IGenericRepository<Home>>(MockBehavior.Strict);
        roleRepositoryMock = new Mock<IGenericRepository<Role>>(MockBehavior.Strict);
        userRepositoryMock = new Mock<IGenericRepository<User>>(MockBehavior.Strict);
        homePermissionRepositoryMock = new Mock<IGenericRepository<HomePermission>>(MockBehavior.Strict);
        homeDeviceRepositoryMock = new Mock<IGenericRepository<HomeDevice>>(MockBehavior.Strict);
        homeMemberRepositoryMock = new Mock<IGenericRepository<HomeMember>>(MockBehavior.Strict);
        deviceRepositoryMock = new Mock<IGenericRepository<Device>>(MockBehavior.Strict);
        homesFromUserRepositoryMock = new Mock<IHomesFromUserRepository>(MockBehavior.Strict);
        homeService = new HomeService(homeMemberRepositoryMock.Object, homeDeviceRepositoryMock.Object, homeRepositoryMock.Object,
                                      userRepositoryMock.Object, homePermissionRepositoryMock.Object, deviceRepositoryMock.Object,
                                      homesFromUserRepositoryMock.Object);
        roleService = new RoleService(roleRepositoryMock.Object, systemPermissionRepositoryMock.Object);
        homeOwnerRole = new Role { Name = "HomeOwner" };
        ownerId = Guid.NewGuid();
        owner = new User { Email = "owner@blank.com", Name = "ownerName", Surname = "ownerSurname", Password = "ownerPassword", Id = ownerId, Role = homeOwnerRole };
    }

    [TestMethod]
    public void Get_HomeOwnerRole_NotFound_ThrowsRoleException()
    {
        Role role = null;

        roleRepositoryMock.Setup(r => r.Find(It.IsAny<Func<Role, bool>>())).Returns(role);

        Exception exception = null;
        try
        {
            roleService.GetHomeOwnerRole();
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(RoleException));
        Assert.AreEqual("Role not found", exception.Message);
    }

    [TestMethod]
    public void Get_HomeOwnerRole_RoleExists_ReturnsRole_Test()
    {
        var expectedRole = new Role { Name = "HomeOwner", Id = Guid.Parse(SeedDataConstants.HOME_OWNER_ROLE_ID) };
        roleRepositoryMock.Setup(repo => repo.Find(It.IsAny<Func<Role, bool>>()))
                           .Returns(expectedRole);

        var result = roleService.GetHomeOwnerRole();

        Assert.IsNotNull(result);
        Assert.AreEqual(expectedRole.Id, result.Id);
    }

    [TestMethod]
    public void Get_BusinessOwnerRole_RoleExists_ReturnsRole_Test()
    {
        var expectedRole = new Role { Name = "BusinessOwner", Id = Guid.Parse(SeedDataConstants.BUSINESS_OWNER_ROLE_ID) };
        roleRepositoryMock.Setup(repo => repo.Find(It.IsAny<Func<Role, bool>>()))
                           .Returns(expectedRole);

        var result = roleService.GetBusinessOwnerRole();

        Assert.IsNotNull(result);
        Assert.AreEqual(expectedRole.Id, result.Id);
    }

    [TestMethod]
    public void Get_BusinessOwnerRole_NotFound_ThrowsRoleException()
    {
        Role role = null;

        roleRepositoryMock.Setup(r => r.Find(It.IsAny<Func<Role, bool>>())).Returns(role);

        Exception exception = null;
        try
        {
            roleService.GetBusinessOwnerRole();
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(RoleException));
        Assert.AreEqual("Role not found", exception.Message);
    }

    [TestMethod]
    public void Get_AdminRole_RoleExists_ReturnsRole_Test()
    {
        var expectedRole = new Role { Name = "Admin", Id = Guid.Parse(SeedDataConstants.ADMIN_ROLE_ID) };
        roleRepositoryMock.Setup(repo => repo.Find(It.IsAny<Func<Role, bool>>()))
                           .Returns(expectedRole);

        var result = roleService.GetAdminRole();

        Assert.IsNotNull(result);
        Assert.AreEqual(expectedRole.Id, result.Id);
    }

    [TestMethod]
    public void Get_AdminRole_NotFound_ThrowsRoleException()
    {
        Role role = null;

        roleRepositoryMock.Setup(r => r.Find(It.IsAny<Func<Role, bool>>())).Returns(role);

        Exception exception = null;
        try
        {
            roleService.GetAdminRole();
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(RoleException));
        Assert.AreEqual("Role not found", exception.Message);
    }
    [TestMethod]
    public void Get_BusinessOwnerHomeOwnerRole_RoleExists_ReturnsRole_Test()
    {
        var expectedRole = new Role { Name = "BusinessOwnerHomeOwner", Id = Guid.Parse(SeedDataConstants.BUSINESS_OWNER_HOME_OWNER_ROLE_ID) };
        roleRepositoryMock.Setup(repo => repo.Find(It.IsAny<Func<Role, bool>>()))
                           .Returns(expectedRole);

        var result = roleService.GetBusinessOwnerHomeOwnerRole();

        Assert.IsNotNull(result);
        Assert.AreEqual(expectedRole.Id, result.Id);
    }

    [TestMethod]
    public void Get_BusinessOwnerHomeOwnerRole_NotFound_ThrowsRoleException()
    {
        Role role = null;

        roleRepositoryMock.Setup(r => r.Find(It.IsAny<Func<Role, bool>>())).Returns(role);

        Exception exception = null;
        try
        {
            roleService.GetBusinessOwnerHomeOwnerRole();
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(RoleException));
        Assert.AreEqual("Role not found", exception.Message);
    }

    [TestMethod]
    public void Get_AdminHomeOwnerRole_RoleExists_ReturnsRole_Test()
    {
        var expectedRole = new Role { Name = "AdminHomeOwner", Id = Guid.Parse(SeedDataConstants.ADMIN_HOME_OWNER_ROLE_ID) };
        roleRepositoryMock.Setup(repo => repo.Find(It.IsAny<Func<Role, bool>>()))
                           .Returns(expectedRole);

        var result = roleService.GetAdminHomeOwnerRole();

        Assert.IsNotNull(result);
        Assert.AreEqual(expectedRole.Id, result.Id);
    }

    [TestMethod]
    public void Get_AdminHomeOwnerRole_NotFound_ThrowsRoleException()
    {
        Role role = null;

        roleRepositoryMock.Setup(r => r.Find(It.IsAny<Func<Role, bool>>())).Returns(role);

        Exception exception = null;
        try
        {
            roleService.GetAdminHomeOwnerRole();
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(RoleException));
        Assert.AreEqual("Role not found", exception.Message);
    }

    [TestMethod]
    public void HasPermission_Role_NotFound_ThrowsRoleException()
    {
        Role role = null;

        roleRepositoryMock.Setup(r => r.Find(It.IsAny<Func<Role, bool>>())).Returns(role);

        Exception exception = null;
        try
        {
            roleService.HasPermission(Guid.NewGuid(), Guid.NewGuid());
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(RoleException));
        Assert.AreEqual("Role not found", exception.Message);
    }

    [TestMethod]
    public void HasPermission_Permission_NotFound_ThrowsRoleException()
    {
        var expectedRole = new Role { Name = "AdminHomeOwner", Id = Guid.Parse(SeedDataConstants.ADMIN_HOME_OWNER_ROLE_ID) };
        roleRepositoryMock.Setup(repo => repo.Find(It.IsAny<Func<Role, bool>>()))
                           .Returns(expectedRole);

        var result = roleService.HasPermission(expectedRole.Id, Guid.NewGuid());

        Assert.IsNotNull(result);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void HasPermission_Permission_Found_ReturnsTrue_Test()
    {
        var expectedRole = new Role { Name = "AdminHomeOwner", Id = Guid.Parse(SeedDataConstants.ADMIN_HOME_OWNER_ROLE_ID) };
        var expectedRolePermission = new SystemPermission { Id = Guid.NewGuid(), Name = "Permission", Description = "Test" };
        expectedRole.SystemPermissions.Add(expectedRolePermission);

        roleRepositoryMock.Setup(repo => repo.Find(It.IsAny<Func<Role, bool>>()))
                           .Returns(expectedRole);
        systemPermissionRepositoryMock.Setup(p => p.Find(It.IsAny<Func<SystemPermission, bool>>()))
                                      .Returns(expectedRolePermission);

        var result = roleService.HasPermission(expectedRole.Id, expectedRolePermission.Id);

        Assert.IsNotNull(result);
        Assert.IsTrue(result);
    }
}
