﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.UserModels.Out;

namespace SmartHome.WebApiTest;

[TestClass]
public class UsersControllerTest
{
    private Mock<IUsersLogic>? usersLogicMock;
    private UsersController? usersController;
    private readonly Role admin = new Role() { Name = "Administrator" };
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
            new User() { Id = Guid.NewGuid(), Name = "a", Surname = "b", Password = "psw", Email = "mail@mail.com", Role = admin, CreationDate = DateTime.Today },
            new User() { Id = Guid.NewGuid(), Name = "c", Surname = "d", Password = "psw", Email = "mail2@mail.com", Role = businessOwner, CreationDate = DateTime.Today },
            new User() { Id = Guid.NewGuid(), Name = "e", Surname = "f", Password = "psw", Email = "mail3@mail.com", Role = homeOwner, CreationDate = DateTime.Today }
        };

        usersLogicMock.Setup(a => a.GetAllUsers()).Returns(users);

        var expected = new OkObjectResult(new List<UserResponseModel>
        {
            new UserResponseModel(users.First()),
            new UserResponseModel(users.ElementAt(1)),
            new UserResponseModel(users.Last())
        });
        List<UserResponseModel> expectedObject = (expected.Value as List<UserResponseModel>)!;

        // ACT
        var result = usersController.GetAllUsers(null, null, null) as OkObjectResult;
        var objectResult = (result.Value as List<UserResponseModel>)!;

        // ASSERT
        usersLogicMock.VerifyAll();
        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode) && expectedObject.First().Equals(objectResult.First()));
    }
}
