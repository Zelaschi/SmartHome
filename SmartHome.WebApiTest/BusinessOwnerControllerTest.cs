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
using SmartHome.WebApi.WebModels.BusinessOwnerModels.In;
using SmartHome.WebApi.WebModels.BusinessOwnerModels.Out;
using SmartHome.WebApi.WebModels.HomeOwnerModels.Out;

namespace SmartHome.WebApiTest;
[TestClass]
public class BusinessOwnerControllerTest
{
    private Mock<IBusinessOwnerLogic>? businessOwnerLogicMock;
    private BusinessOwnerController? businessOwnerController;

    [TestInitialize]
    public void TestInitialize()
    {
        businessOwnerLogicMock = new Mock<IBusinessOwnerLogic>(MockBehavior.Strict);
        businessOwnerController = new BusinessOwnerController(businessOwnerLogicMock.Object);
    }

    [TestMethod]
    public void RegisterBusinessOwnerTest_OK()
    {
        var businessOwnerRequestModel = new BusinessOwnerRequestModel()
        {
            Name = "businessOwnerName",
            Surname = "businessOwnerSurname",
            Password = "businessOwnerPassword",
            Email = "businessOwnerMail@domain.com"
        };

        var businessOwner = businessOwnerRequestModel.ToEntitiy();
        businessOwnerLogicMock.Setup(b => b.CreateBusinessOwner(It.IsAny<User>())).Returns(businessOwner);

        var expectedResult = new BusinessOwnerResponseModel(businessOwner);
        var expectedObjecResult = new CreatedAtActionResult("CreateBusinessOwner", "CreateBusinessOwner", new { Id = businessOwner.Id }, expectedResult);

        var result = businessOwnerController.CreateBusinessOwner(businessOwnerRequestModel) as CreatedAtActionResult;
        var businessOwnerResult = result.Value as BusinessOwnerResponseModel;

        businessOwnerLogicMock.VerifyAll();
        Assert.IsTrue(expectedObjecResult.StatusCode.Equals(result.StatusCode) && expectedResult.Equals(businessOwnerResult));
    }
}
