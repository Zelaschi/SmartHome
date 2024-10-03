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

namespace SmartHome.BusinessLogicTest;

[TestClass]
public class UserLogicTest
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

    public void GetAll_Users_Test()
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

        var admin = new User
        {
            Name = "Raul",
            Surname = "Gonzales",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "juanperez@gmail.com",
            Role = new Role { Name = "Administrator" }
        };

        IEnumerable<User> users = new List<User>
        {
            homeOwner,
            admin
        };

        userRepositoryMock.Setup(x => x.FindAll()).Returns((IList<User>)users);

        IEnumerable<User> usersResult = userService.GetAllUsers();

        userRepositoryMock.VerifyAll();
        Assert.AreEqual(users, usersResult);
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

        userRepositoryMock.Setup(x => x.Add(admin)).Returns(admin);

        var adminResult = userService.CreateAdmin(admin);

        Assert.IsNotNull(adminResult);

        Assert.AreEqual(admin.Role.Name, "Admin");
        Assert.AreEqual(admin.Role.Id, Guid.Parse(SeedDataConstants.ADMIN_ROLE_ID));

        Assert.AreEqual(admin, adminResult);
    }
}
