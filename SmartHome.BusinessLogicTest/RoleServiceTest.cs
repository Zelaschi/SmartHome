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
    public void GetHomeOwnerRole_NotFound_ThrowsRoleException()
    {
        // Arrange
        Role role = null; // Simula que no se encuentra el rol

        // Configura el mock para que el repositorio de roles devuelva null
        roleRepositoryMock.Setup(r => r.Find(It.IsAny<Func<Role, bool>>())).Returns(role);

        // Act
        Exception exception = null;
        try
        {
            roleService.GetHomeOwnerRole();
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        // Assert
        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(RoleException));
        Assert.AreEqual("Role not found", exception.Message);
    }

    [TestMethod]
    public void GetBusinessOwnerRole_RoleExists_ReturnsRole_Test()
    {
        var expectedRole = new Role { Name = "HomeOwner", Id = Guid.Parse(SeedDataConstants.BUSINESS_OWNER_ROLE_ID) };
        roleRepositoryMock.Setup(repo => repo.Find(It.IsAny<Func<Role, bool>>()))
                           .Returns(expectedRole);

        var result = roleService.GetBusinessOwnerRole();

        Assert.IsNotNull(result);
        Assert.AreEqual(expectedRole.Id, result.Id);
    }
}
