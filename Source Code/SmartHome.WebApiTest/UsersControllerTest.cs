﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.PaginationModels.Out;
using SmartHome.WebApi.WebModels.UserModels.Out;

namespace SmartHome.WebApi.Test;

[TestClass]
public class UsersControllerTest
{
    private Mock<IUsersLogic>? usersLogicMock;
    private UsersController? usersController;
    private readonly Role admin = new Role { Name = "Administrator" };
    private readonly Role businessOwner = new Role() { Name = "BusinessOwner" };
    private readonly Role homeOwner = new Role() { Name = "HomeOwner" };

    [TestInitialize]
    public void TestInitialize()
    {
        usersLogicMock = new Mock<IUsersLogic>(MockBehavior.Strict);
        usersController = new UsersController(usersLogicMock.Object);
    }

    [TestMethod]
    public void GetAllUsersTest_OK()
    {
        IEnumerable<User> users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                Name = "a",
                Surname = "b",
                Password = "psw",
                Email = "mail@mail.com",
                Role = admin,
                CreationDate = DateTime.Today
            },
            new User
            {
                Id = Guid.NewGuid(),
                Name = "c",
                Surname = "d",
                Password = "psw",
                Email = "mail2@mail.com",
                Role = businessOwner,
                CreationDate = DateTime.Today
            },
            new User
            {
                Id = Guid.NewGuid(),
                Name = "e",
                Surname = "f",
                Password = "psw",
                Email = "mail3@mail.com",
                Role = homeOwner,
                CreationDate = DateTime.Today
            }
        };

        usersLogicMock.Setup(a => a.GetUsers(null, null, null, null))
            .Returns(users);

        var expected = new OkObjectResult(new List<UserResponseModel>
        {
            new UserResponseModel(users.First()),
            new UserResponseModel(users.ElementAt(1)),
            new UserResponseModel(users.Last())
        });
        List<UserResponseModel> expectedObject = (expected.Value as List<UserResponseModel>)!;

        var result = usersController.GetUsers(null, null, null, null) as OkObjectResult;
        var objectResult = (result.Value as List<UserResponseModel>)!;

        usersLogicMock.VerifyAll();
        Assert.AreEqual(result.StatusCode, expected.StatusCode);
        Assert.AreEqual(expectedObject.First(), objectResult.First());
    }

    [TestMethod]
    public void GetUsersTest_WithPagination_OK()
    {
        var user1 = new User()
        {
            Id = Guid.NewGuid(),
            Name = "Alice",
            Surname = "Smith",
            Password = "psw1",
            Email = "alice@mail.com",
            Role = businessOwner
        };

        var user2 = new User()
        {
            Id = Guid.NewGuid(),
            Name = "Bob",
            Surname = "Johnson",
            Password = "psw2",
            Email = "bob@mail.com",
            Role = businessOwner
        };

        var user3 = new User()
        {
            Id = Guid.NewGuid(),
            Name = "Charlie",
            Surname = "Brown",
            Password = "psw3",
            Email = "charlie@mail.com",
            Role = businessOwner
        };
        var pageNumber = 1;
        var pageSize = 2;
        IEnumerable<User> users = new List<User>
        {
            user1,
            user2,
            user3
        };
        var expectedReturnedUsers = new List<User>
        {
            user1,
            user2
        };
        usersLogicMock.Setup(u => u.GetUsers(pageNumber, pageSize, null, null))
            .Returns(expectedReturnedUsers);

        var result = usersController.GetUsers(pageNumber, pageSize, null, null) as OkObjectResult;

        Assert.IsNotNull(result);
        var resultValue = result.Value as PaginatedResponse<UserResponseModel>;
        Assert.IsNotNull(resultValue);
        var returnedUsers = resultValue.Data;
        var totalCount = resultValue.TotalCount;

        Assert.AreEqual(pageSize, returnedUsers.Count);
        Assert.AreEqual(totalCount, expectedReturnedUsers.Count);
        Assert.AreEqual(user1.Name, returnedUsers[0].Name);
        Assert.AreEqual(user2.Name, returnedUsers[1].Name);
        Assert.AreEqual(pageNumber, resultValue.PageNumber);
        Assert.AreEqual(pageSize, resultValue.PageSize);
    }

    [TestMethod]
    public void GetUsersTest_FilterByRole_OK()
    {
        var user1 = new User()
        {
            Id = Guid.NewGuid(),
            Name = "Alice",
            Surname = "Smith",
            Password = "psw1",
            Email = "alice@mail.com",
            Role = businessOwner
        };

        var user2 = new User()
        {
            Id = Guid.NewGuid(),
            Name = "Bob",
            Surname = "Johnson",
            Password = "psw2",
            Email = "bob@mail.com",
            Role = admin
        };

        var user3 = new User()
        {
            Id = Guid.NewGuid(),
            Name = "Charlie",
            Surname = "Brown",
            Password = "psw3",
            Email = "charlie@mail.com",
            Role = businessOwner
        };

        IEnumerable<User> users = new List<User>
        {
            user1,
            user2,
            user3
        };
        var expectedReturnedUsers = new List<User>
        {
            user1,
            user3
        };
        usersLogicMock.Setup(u => u.GetUsers(null, null, businessOwner.Name, null))
            .Returns(expectedReturnedUsers);

        var result = usersController.GetUsers(null, null, businessOwner.Name, null) as OkObjectResult;

        Assert.IsNotNull(result);
        var resultValue = result.Value as List<UserResponseModel>;
        Assert.IsNotNull(resultValue);
        Assert.AreEqual(2, resultValue.Count);
    }

    [TestMethod]
    public void GetUsersTest_FilterByFullName_OK()
    {
        var user1 = new User()
        {
            Id = Guid.NewGuid(),
            Name = "Alice",
            Surname = "Smith",
            Password = "psw1",
            Email = "alice@mail.com",
            Role = businessOwner
        };

        var user2 = new User()
        {
            Id = Guid.NewGuid(),
            Name = "Bob",
            Surname = "Johnson",
            Password = "psw2",
            Email = "bob@mail.com",
            Role = businessOwner
        };

        IEnumerable<User> users = new List<User>
        {
            user1,
            user2
        };
        var expectedReturnedUsers = new List<User>
        {
            user1
        };
        usersLogicMock.Setup(u => u.GetUsers(null, null, null, "Alice Smith"))
            .Returns(expectedReturnedUsers);

        var result = usersController.GetUsers(null, null, null, "Alice Smith") as OkObjectResult;

        Assert.IsNotNull(result);
        var resultValue = result.Value as List<UserResponseModel>;
        Assert.IsNotNull(resultValue);
        Assert.AreEqual(1, resultValue.Count);
        Assert.AreEqual(user1.Name, resultValue[0].Name);
    }

    [TestMethod]
    public void GetUsersTest_FilterByFullName_NotFound()
    {
        var user1 = new User()
        {
            Id = Guid.NewGuid(),
            Name = "Alice",
            Surname = "Smith",
            Password = "psw1",
            Email = "alice@mail.com",
            Role = businessOwner
        };

        IEnumerable<User> users = new List<User>
        {
            user1
        };
        var expectedReturnedUsers = new List<User>();
        usersLogicMock.Setup(u => u.GetUsers(null, null, null, "John Doe"))
            .Returns(expectedReturnedUsers);

        var result = usersController.GetUsers(null, null, null, "John Doe") as OkObjectResult;

        Assert.IsNotNull(result);
        var resultValue = result.Value as List<UserResponseModel>;
        Assert.IsNotNull(resultValue);
        Assert.AreEqual(0, resultValue.Count);
    }

    [TestMethod]
    public void UsersController_NullUsersLogic_ThrowsArgumentNullException()
    {
        try
        {
            var controller = new UsersController(null);
        }
        catch (ArgumentNullException ex)
        {
            Assert.AreEqual("usersLogic", ex.ParamName);
        }
    }
}
