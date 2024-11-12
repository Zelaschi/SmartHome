using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Services;
using SmartHome.BusinessLogic.InitialSeedData;
using System.Linq.Expressions;

namespace SmartHome.BusinessLogic.Test;

[TestClass]
public class UserServiceTest
{
    private Mock<IGenericRepository<User>>? userRepositoryMock;
    private Mock<IGenericRepository<Role>>? roleRepositoryMock;
    private Mock<IRoleLogic>? roleLogicMock;
    private UserService? userService;

    [TestInitialize]

    public void Initialize()
    {
        userRepositoryMock = new Mock<IGenericRepository<User>>(MockBehavior.Strict);
        roleRepositoryMock = new Mock<IGenericRepository<Role>>(MockBehavior.Strict);
        roleLogicMock = new Mock<IRoleLogic>(MockBehavior.Strict);
        userService = new UserService(userRepositoryMock.Object, roleLogicMock.Object);
    }

    [TestMethod]
    public void CreateHomeOwnerTest_Ok()
    {
        var homeOwner = new User
        {
            Name = "Juan",
            Surname = "Perez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "juanperez@gmail.com",
        };

        roleLogicMock.Setup(x => x.GetHomeOwnerRole()).Returns(new Role { Name = "HomeOwner" });

        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns((User)null);

        userRepositoryMock.Setup(x => x.Add(homeOwner)).Returns(homeOwner);

        homeOwner.Id = Guid.NewGuid();
        var homeOwnerResult = userService.CreateHomeOwner(homeOwner);

        userRepositoryMock.VerifyAll();
        Assert.AreEqual(homeOwner, homeOwnerResult);
        Assert.AreEqual(expected: homeOwner.Role.Name, "HomeOwner");
    }

    [TestMethod]
    public void Create_HomeOwnerWithExistingEmail_Test()
    {
        var homeOwner = new User
        {
            Name = "Juan",
            Surname = "Perez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "juanperez@gmail.com",
            Role = new Role { Name = "HomeOwner" }
        };

        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(homeOwner);

        Exception exception = null;

        try
        {
            userService.CreateHomeOwner(homeOwner);
        }
        catch (Exception e)
        {
            exception = e;
        }

        userRepositoryMock.VerifyAll();
        Assert.IsInstanceOfType(exception, typeof(UserException));
        Assert.AreEqual("User with that email already exists", exception.Message);
    }

    [TestMethod]

    public void Create_HomeOwnerWithNoAInvalidEmail_Test()
    {
        var homeOwner = new User
        {
            Name = "Juan",
            Surname = "Perez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "juanperezgmail.com",
            Role = new Role { Name = "HomeOwner" }
        };

        Exception exception = null;

        try
        {
            userService.CreateHomeOwner(homeOwner);
        }
        catch (Exception e)
        {
            exception = e;
        }

        userRepositoryMock.Verify(x => x.Find(It.IsAny<Func<User, bool>>()), Times.Never);
        Assert.IsInstanceOfType(exception, typeof(UserException));
        Assert.AreEqual("Invalid email, must contain @ and .", exception.Message);
    }

    [TestMethod]

    public void Create_HomeOwnerWithNoDotInvalidEmail_Test()
    {
        var homeOwner = new User
        {
            Name = "Juan",
            Surname = "Perez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "juanperez@gmailcom",
            Role = new Role { Name = "HomeOwner" }
        };

        Exception exception = null;

        try
        {
            userService.CreateHomeOwner(homeOwner);
        }
        catch (Exception e)
        {
            exception = e;
        }

        userRepositoryMock.Verify(x => x.Find(It.IsAny<Func<User, bool>>()), Times.Never);
        Assert.IsInstanceOfType(exception, typeof(UserException));
        Assert.AreEqual("Invalid email, must contain @ and .", exception.Message);
    }

    [TestMethod]

    public void Create_HomeOwnerWithNoSpecialCharacterInvalidPassword_Test()
    {
        var homeOwner = new User
        {
            Name = "Juan",
            Surname = "Perez",
            Password = "Password1234",
            CreationDate = DateTime.Today,
            Email = "juanperez@gmail.com",
            Role = new Role { Name = "HomeOwner" }
        };

        Exception exception = null;

        try
        {
            userService.CreateHomeOwner(homeOwner);
        }
        catch (Exception e)
        {
            exception = e;
        }

        userRepositoryMock.Verify(x => x.Find(It.IsAny<Func<User, bool>>()), Times.Never);
        Assert.IsInstanceOfType(exception, typeof(UserException));
        Assert.AreEqual("Invalid password, must contain special character and be longer than 6 characters", exception.Message);
    }

    [TestMethod]

    public void Create_HomeOwnerWithShortInvalidPassword_Test()
    {
        var homeOwner = new User
        {
            Name = "Juan",
            Surname = "Perez",
            Password = "P@14",
            CreationDate = DateTime.Today,
            Email = "juanperez@gmail.com",
            Role = new Role { Name = "HomeOwner" }
        };

        Exception exception = null;

        try
        {
            userService.CreateHomeOwner(homeOwner);
        }
        catch (Exception e)
        {
            exception = e;
        }

        userRepositoryMock.Verify(x => x.Find(It.IsAny<Func<User, bool>>()), Times.Never);
        Assert.IsInstanceOfType(exception, typeof(UserException));
        Assert.AreEqual("Invalid password, must contain special character and be longer than 6 characters", exception.Message);
    }

    [TestMethod]

    public void Create_HomeOwnerWithNoName_Test()
    {
        var homeOwner = new User
        {
            Name = string.Empty,
            Surname = "Perez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "juanperez@gmail.com",
            Role = new Role { Name = "HomeOwner" }
        };

        Exception exception = null;

        try
        {
            userService.CreateHomeOwner(homeOwner);
        }
        catch (Exception e)
        {
            exception = e;
        }

        userRepositoryMock.Verify(x => x.Find(It.IsAny<Func<User, bool>>()), Times.Never);
        Assert.IsInstanceOfType(exception, typeof(UserException));
        Assert.AreEqual("Invalid name, cannot be empty", exception.Message);
    }

    [TestMethod]

    public void Create_HomeOwnerWithNoSurname_Test()
    {
        var homeOwner = new User
        {
            Name = "Juan",
            Surname = string.Empty,
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "juanperez@gmail.com",
            Role = new Role { Name = "HomeOwner" }
        };

        Exception exception = null;

        try
        {
            userService.CreateHomeOwner(homeOwner);
        }
        catch (Exception e)
        {
            exception = e;
        }

        userRepositoryMock.Verify(x => x.Find(It.IsAny<Func<User, bool>>()), Times.Never);
        Assert.IsInstanceOfType(exception, typeof(UserException));
        Assert.AreEqual("Invalid surname, cannot be empty", exception.Message);
    }

    [TestMethod]

    public void Create_BusinessOwner_Test()
    {
        var businessOwner = new User
        {
            Name = "Pedro",
            Surname = "Rodrigues",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "pedroRodriguez@gmail.com",
            Complete = false
        };

        roleLogicMock.Setup(x => x.GetBusinessOwnerRole()).Returns(new Role { Name = "BusinessOwner" });

        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns((User)null);

        userRepositoryMock.Setup(x => x.Add(businessOwner)).Returns(businessOwner);

        businessOwner.Id = Guid.NewGuid();
        var businessOwnerResult = userService.CreateBusinessOwner(businessOwner);

        Assert.IsNotNull(businessOwnerResult);
        Assert.AreEqual(businessOwner.Role.Name, "BusinessOwner");
        Assert.AreEqual(businessOwner, businessOwnerResult);
    }

    [TestMethod]

    public void Create_Admin_Test()
    {
        var admin = new User
        {
            Name = "Pedro",
            Surname = "Rodrigues",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "pedroRodriguez@gmail.com"
        };

        roleLogicMock.Setup(x => x.GetAdminRole()).Returns(new Role { Name = "Admin", Id = Guid.Parse(SeedDataConstants.ADMIN_ROLE_ID) });
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns((User)null);
        userRepositoryMock.Setup(x => x.Add(admin)).Returns(admin);

        var adminResult = userService.CreateAdmin(admin);

        Assert.IsNotNull(adminResult);

        Assert.AreEqual(admin.Role.Name, "Admin");
        Assert.AreEqual(admin.Role.Id, Guid.Parse(SeedDataConstants.ADMIN_ROLE_ID));

        Assert.AreEqual(admin, adminResult);
    }

    [TestMethod]
    public void Delete_Admin_Test()
    {
        var adminRole = new Role { Name = "Admin", Id = Guid.Parse(SeedDataConstants.ADMIN_ROLE_ID) };
        var adminId = Guid.NewGuid();
        var admin = new User
        {
            Id = adminId,
            Name = "Pedro",
            Surname = "Azambuja",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "pedroRodriguez@gmail.com",
            Role = adminRole
        };
        var admin2 = new User
        {
            Id = Guid.NewGuid(),
            Name = "Toto",
            Surname = "Zelaschi",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "toto@example.com",
            Role = adminRole
        };
        var adminList = new List<User> { admin, admin2 };

        userRepositoryMock.Setup(x => x.FindAll()).Returns(adminList);
        userRepositoryMock.Setup(x => x.FindAllFiltered(It.IsAny<Expression<Func<User, bool>>>()))
                          .Returns((Expression<Func<User, bool>> filter) => adminList.Where(filter.Compile()).ToList());

        userRepositoryMock.Setup(x => x.Delete(It.IsAny<Guid>()))
                          .Callback<Guid>(id => adminList.RemoveAll(u => u.Id == id));

        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(admin);
        roleLogicMock.Setup(x => x.GetAdminRole()).Returns(adminRole);

        userService.DeleteAdmin(adminId);

        var adminListResult = userService.GetUsers(null, null, null, null);

        userRepositoryMock.VerifyAll();
        Assert.IsNotNull(adminListResult);
        Assert.AreEqual(1, adminListResult.Count());
        Assert.AreEqual("Toto", adminListResult.ElementAt(0).Name);
        Assert.AreEqual("Zelaschi", adminListResult.ElementAt(0).Surname);
    }

    [TestMethod]
    public void Delete_Only_Admin_Throws_Exception_Test()
    {
        var adminRole = new Role { Name = "Admin", Id = Guid.Parse(SeedDataConstants.ADMIN_ROLE_ID) };
        var adminId = Guid.NewGuid();
        var admin = new User
        {
            Id = adminId,
            Name = "Pedro",
            Surname = "Azambuja",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "pedroRodriguez@gmail.com",
            Role = adminRole
        };
        var adminList = new List<User> { admin };

        roleLogicMock.Setup(x => x.GetAdminRole()).Returns(adminRole);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns((User)null);
        userRepositoryMock.Setup(x => x.Add(admin)).Returns(admin);
        userRepositoryMock.Setup(x => x.FindAll()).Returns(adminList);
        userService.CreateAdmin(admin);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(admin);

        try
        {
            userService.DeleteAdmin(adminId);
        }
        catch (Exception e)
        {
            userRepositoryMock.VerifyAll();
            Assert.IsInstanceOfType(e, typeof(UserException));
            Assert.AreEqual("Cannot delete the only admin user", e.Message);
        }
    }

    [TestMethod]
    public void Delete_Non_Existing_Admin_Throws_Exception_Test()
    {
        var adminId = Guid.NewGuid();
        var admin = new User
        {
            Id = adminId,
            Name = "Pedro",
            Surname = "Azambuja",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "pedroRodriguez@gmail.com"
        };
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns((User)null);
        roleLogicMock.Setup(x => x.GetAdminRole()).Returns(new Role { Name = "Admin", Id = Guid.Parse(SeedDataConstants.ADMIN_ROLE_ID) });
        try
        {
            userService.DeleteAdmin(adminId);
        }
        catch (Exception e)
        {
            userRepositoryMock.VerifyAll();
            Assert.IsInstanceOfType(e, typeof(UserException));
            Assert.AreEqual("Admin not found", e.Message);
        }
    }

    [TestMethod]
    public void Update_Admin_Role_Test()
    {
        var adminRole = new Role { Name = "Admin", Id = Guid.Parse(SeedDataConstants.ADMIN_ROLE_ID) };
        var adminHomeOwnerRole = new Role { Name = "AdminHomeOwner", Id = Guid.Parse(SeedDataConstants.ADMIN_HOME_OWNER_ROLE_ID) };
        var adminId = Guid.NewGuid();

        var admin = new User
        {
            Id = adminId,
            Name = "Pedro",
            Surname = "Azambuja",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "pedroRodriguez@gmail.com",
            Role = adminRole
        };

        var adminResult = new User
        {
            Id = adminId,
            Name = "Pedro",
            Surname = "Azambuja",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "pedroRodriguez@gmail.com",
            Role = adminHomeOwnerRole
        };

        userRepositoryMock.Setup(x => x.Update(It.IsAny<User>()))
                          .Callback<User>(u => u.Role = adminHomeOwnerRole)
                          .Returns(adminResult);

        roleLogicMock.Setup(x => x.GetAdminHomeOwnerRole()).Returns(adminHomeOwnerRole);

        userService.UpdateAdminRole(admin);

        userRepositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
        roleLogicMock.Verify(x => x.GetAdminHomeOwnerRole(), Times.Once);

        Assert.AreEqual(adminHomeOwnerRole, admin.Role);
    }

    [TestMethod]
    public void Update_BusinessOwner_Role_Test()
    {
        var businessOwnerRole = new Role { Name = "BusinessOwner", Id = Guid.Parse(SeedDataConstants.BUSINESS_OWNER_ROLE_ID) };
        var businessOwnerHomeOwnerRole = new Role { Name = "BusinessOwnerHomeOwner", Id = Guid.Parse(SeedDataConstants.BUSINESS_OWNER_HOME_OWNER_ROLE_ID) };
        var businessOwnerId = Guid.NewGuid();

        var businessOwner = new User
        {
            Id = businessOwnerId,
            Name = "Pedro",
            Surname = "Azambuja",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "pedroRodriguez@gmail.com",
            Role = businessOwnerRole
        };

        var businessOwnerResult = new User
        {
            Id = businessOwnerId,
            Name = "Pedro",
            Surname = "Azambuja",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "pedroRodriguez@gmail.com",
            Role = businessOwnerHomeOwnerRole
        };

        userRepositoryMock.Setup(x => x.Update(It.IsAny<User>()))
                          .Callback<User>(u => u.Role = businessOwnerHomeOwnerRole)
                          .Returns(businessOwnerResult);

        roleLogicMock.Setup(x => x.GetBusinessOwnerHomeOwnerRole()).Returns(businessOwnerHomeOwnerRole);

        userService.UpdateBusinessOwnerRole(businessOwner);

        userRepositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
        roleLogicMock.Verify(x => x.GetBusinessOwnerHomeOwnerRole(), Times.Once);

        Assert.AreEqual(businessOwnerHomeOwnerRole, businessOwner.Role);
    }

    [TestMethod]
    public void GetUsers_WithoutFilters_ReturnsAllUsers()
    {
        var users = new List<User>
        {
            new User
            {
                Password = "Password@1234",
                Name = "Juan",
                Surname = "Perez",
                Role = new Role { Name = "Admin" },
                Email = "juan@example.com"
            },
            new User
            {
                Password = "Password@1234",
                Name = "Pedro",
                Surname = "Rodriguez",
                Role = new Role { Name = "User" },
                Email = "pedro@example.com"
            },
        };

        userRepositoryMock.Setup(x => x.FindAllFiltered(It.IsAny<Expression<Func<User, bool>>>())).Returns(users);

        var result = userService.GetUsers(null, null, null, null);

        userRepositoryMock.Verify(x => x.FindAllFiltered(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public void GetUsers_WithRoleFilter_ReturnsFilteredUsers()
    {
        var users = new List<User>
        {
            new User
            {
                Password = "Password1234",
                Name = "Juan",
                Surname = "Perez",
                Role = new Role { Name = "Admin" },
                Email = "juan@example.com"
            }
        };

        userRepositoryMock.Setup(x => x.FindAllFiltered(It.IsAny<Expression<Func<User, bool>>>())).Returns(users);

        var result = userService.GetUsers(null, null, "Admin", null);

        userRepositoryMock.Verify(x => x.FindAllFiltered(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetUsers_WithFullNameFilter_ReturnsFilteredUsers()
    {
        var users = new List<User>
        {
            new User
            {
                Password = "Password1234",
                Name = "Juan",
                Surname = "Perez",
                Role = new Role { Name = "Admin" },
                Email = "juan@example.com"
            }
        };

        userRepositoryMock.Setup(x => x.FindAllFiltered(It.IsAny<Expression<Func<User, bool>>>())).Returns(users);

        var result = userService.GetUsers(null, null, null, "Juan Perez");

        userRepositoryMock.Verify(x => x.FindAllFiltered(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetUsers_WithFiltersAndPagination_ReturnsPagedFilteredUsers()
    {
        var users = new List<User>
        {
            new User
            {
                Password = "Password1234",
                Name = "Juan",
                Surname = "Perez",
                Role = new Role { Name = "Admin" },
                Email = "juan@example.com"
            }
        };

        userRepositoryMock.Setup(x => x.FindAllFiltered(It.IsAny<Expression<Func<User, bool>>>(), 1, 10)).Returns(users);

        var result = userService.GetUsers(1, 10, "Admin", "Juan Perez");

        userRepositoryMock.Verify(x => x.FindAllFiltered(It.IsAny<Expression<Func<User, bool>>>(), 1, 10), Times.Once);
        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetUsers_WithPaginationOnly_ReturnsPagedUsers()
    {
        var users = new List<User>
        {
            new User
            {
                Password = "Password1234",
                Name = "Pedro",
                Surname = "Rodriguez",
                Role = new Role { Name = "User" },
                Email = "pedro@example.com"
            }
        };

        userRepositoryMock.Setup(x => x.FindAllFiltered(It.IsAny<Expression<Func<User, bool>>>(), 1, 10)).Returns(users);

        var result = userService.GetUsers(1, 10, null, null);

        userRepositoryMock.Verify(x => x.FindAllFiltered(It.IsAny<Expression<Func<User, bool>>>(), 1, 10), Times.Once);
        Assert.AreEqual(1, result.Count());
    }
}
