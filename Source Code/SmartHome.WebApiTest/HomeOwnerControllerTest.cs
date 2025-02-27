﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.HomeOwnerModels.In;
using SmartHome.WebApi.WebModels.HomeOwnerModels.Out;

namespace SmartHome.WebApi.Test;

[TestClass]
public class HomeOwnerControllerTest
{
    private Mock<IHomeOwnerLogic>? homeOwnerLogicMock;
    private HomeOwnersController? homeOwnerController;

    [TestInitialize]
    public void TestInitialize()
    {
        homeOwnerLogicMock = new Mock<IHomeOwnerLogic>(MockBehavior.Strict);
        homeOwnerController = new HomeOwnersController(homeOwnerLogicMock.Object);
    }

    [TestMethod]
    public void RegisterHomeOwnerTest_OK()
    {
        var homeOwnerRequestModel = new HomeOwnerRequestModel()
        {
            Name = "homeOwnerName",
            Surname = "homeOwnerSurname",
            Password = "homeOwnerPassword",
            Email = "homeOwnerMail@domain.com",
            ProfilePhoto = "profilePhotoPath"
        };

        var homeOwner = homeOwnerRequestModel.ToEntitiy();
        homeOwnerLogicMock.Setup(h => h.CreateHomeOwner(It.IsAny<User>()))
            .Returns(homeOwner);

        var expectedResult = new HomeOwnerResponseModel(homeOwner);
        var expectedObjectResult = new CreatedAtActionResult("CreateHomeOwner", "CreateHomeOwner", new { Id = homeOwner.Id }, expectedResult);

        var result = homeOwnerController.CreateHomeOwner(homeOwnerRequestModel) as CreatedAtActionResult;
        var homeOwnerResult = result.Value as HomeOwnerResponseModel;

        homeOwnerLogicMock.VerifyAll();
        Assert.AreEqual(expectedObjectResult.StatusCode, result.StatusCode);
        Assert.AreEqual(expectedResult, homeOwnerResult);
    }
}
