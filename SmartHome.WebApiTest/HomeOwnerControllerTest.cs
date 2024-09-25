using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.HomeOwnerModels.In;
using SmartHome.WebApi.WebModels.HomeOwnerModels.Out;

namespace SmartHome.WebApiTest;

[TestClass]
public class HomeOwnerControllerTest
{
    private Mock<IHomeOwnerLogic>? homeOwnerLogicMock;
    private HomeOwnerController? homeOwnerController;

    [TestInitialize]
    public void TestInitialize()
    {
        homeOwnerLogicMock = new Mock<IHomeOwnerLogic>(MockBehavior.Strict);
        homeOwnerController = new HomeOwnerController(homeOwnerLogicMock.Object);
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
        homeOwnerLogicMock.Setup(h => h.CreateHomeOwner(It.IsAny<User>())).Returns(homeOwner);

        var expectedResult = new HomeOwnerResponseModel(homeOwner);
        var expectedObjectResult = new CreatedAtActionResult("CreateHomeOwner", "CreateHomeOwner", new { Id = homeOwner.Id }, expectedResult);

        var result = homeOwnerController.CreateHomeOwner(homeOwnerRequestModel) as CreatedAtActionResult;
        var homeOwnerResult = result.Value as HomeOwnerResponseModel;

        homeOwnerLogicMock.VerifyAll();
        Assert.IsTrue(expectedObjectResult.StatusCode.Equals(result.StatusCode) && expectedResult.Equals(homeOwnerResult));
    }
}
