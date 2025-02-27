﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Constants;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.BusinessOwnerModels.In;
using SmartHome.WebApi.WebModels.BusinessOwnerModels.Out;
using User = SmartHome.BusinessLogic.Domain.User;

namespace SmartHome.WebApi.Test;
[TestClass]
public class BusinessOwnerControllerTest
{
    private Mock<IBusinessOwnerLogic>? businessOwnerLogicMock;
    private BusinessOwnersController? businessOwnerController;
    private readonly Role businessOwner = new Role
    {
        Name = "BusinessOwner"
    };

    [TestInitialize]
    public void TestInitialize()
    {
        businessOwnerLogicMock = new Mock<IBusinessOwnerLogic>(MockBehavior.Strict);
        businessOwnerController = new BusinessOwnersController(businessOwnerLogicMock.Object);
    }

    [TestMethod]
    public void RegisterBusinessOwnerTest_OK()
    {
        var businessOwnerRequestModel = new BusinessOwnerRequestModel
        {
            Name = "businessOwnerName",
            Surname = "businessOwnerSurname",
            Password = "businessOwnerPassword",
            Email = "businessOwnerMail@domain.com"
        };

        var businessOwner = businessOwnerRequestModel.ToEntitiy();
        businessOwnerLogicMock.Setup(b => b.CreateBusinessOwner(It.IsAny<User>()))
            .Returns(businessOwner);

        var expectedResult = new BusinessOwnerResponseModel(businessOwner);
        var expectedObjecResult = new CreatedAtActionResult("CreateBusinessOwner", "CreateBusinessOwner", new { Id = businessOwner.Id }, expectedResult);

        var result = businessOwnerController.CreateBusinessOwner(businessOwnerRequestModel) as CreatedAtActionResult;
        var businessOwnerResult = result.Value as BusinessOwnerResponseModel;

        businessOwnerLogicMock.VerifyAll();
        Assert.AreEqual(expectedObjecResult.StatusCode, result.StatusCode);
        Assert.AreEqual(expectedResult, businessOwnerResult);
    }

    [TestMethod]
    public void UpdateBusinessOwnerRoleTest_OK()
    {
        var user = new User()
        {
            Id = Guid.NewGuid(),
            Name = "businessOwnerName",
            Surname = "businessOwnerSurname",
            Password = "businessOwnerPassword",
            Email = "businessOwnerMail@domain.com",
            Role = businessOwner
        };

        var httpContextMock = new Mock<HttpContext>();
        var items = new Dictionary<object, object>();
        items[UserStatic.User] = user;

        httpContextMock.Setup(x => x.Items).Returns(items);

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContextMock.Object
        };

        businessOwnerController.ControllerContext = controllerContext;

        businessOwnerLogicMock.Setup(a => a.UpdateBusinessOwnerRole(user)).Verifiable();
        var result = businessOwnerController.UpdateBusinessOwnerRole() as OkObjectResult;

        businessOwnerLogicMock.VerifyAll();
        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
        Assert.AreEqual("BusinessOwner Permissions Updated successfully", result.Value);
    }

    [TestMethod]
    public void UpdateBusinessOwnerRole_UserIdMissing_ReturnsUnauthorized()
    {
        var httpContext = new DefaultHttpContext();
        var controllerContext = new ControllerContext { HttpContext = httpContext };

        businessOwnerController.ControllerContext = controllerContext;

        var result = businessOwnerController.UpdateBusinessOwnerRole() as UnauthorizedObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(401, result.StatusCode);
        Assert.AreEqual("UserId is missing", result.Value);
    }
}
