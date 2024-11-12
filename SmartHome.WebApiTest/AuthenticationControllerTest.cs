using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.DTOs;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.LoginModels.In;
using SmartHome.WebApi.WebModels.LoginModels.Out;

namespace SmartHome.WebApi.Test;

[TestClass]
public class AuthenticationControllerTest
{
    private Mock<ILoginLogic>? loginLogicMock;
    private AuthenticationController? authController;

    // private readonly Session session = new Session() { SessionId = new Guid(), UserId = new Guid() };

    [TestInitialize]
    public void TestInitialize()
    {
        loginLogicMock = new Mock<ILoginLogic>(MockBehavior.Strict);
        authController = new AuthenticationController(loginLogicMock.Object);
    }

    [TestMethod]
    public void LoginTest_Ok()
    {
        // ARRANGE
        var loginRequest = new LoginRequestModel() { Email = "aemail@domain.com", Password = "aPassword" };
        var token = Guid.NewGuid();
        var sessionAndSP = new DTOSessionAndSystemPermissions
        {
            SessionId = Guid.NewGuid(),
            SystemPermissions = new List<SystemPermission>()
        };
        var expectedLoginResponse = new LoginResponseModel(sessionAndSP) { Token = token };

        loginLogicMock.Setup(l => l.LogIn(It.IsAny<string>(), It.IsAny<string>())).Returns(sessionAndSP);

        var expected = new OkObjectResult(expectedLoginResponse);

        // ACT
        var result = authController.LogIn(loginRequest) as OkObjectResult;
        var objectResult = (result.Value as LoginResponseModel)!;

        // ASSERT
        loginLogicMock.VerifyAll();
        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode) && expectedLoginResponse.Equals(objectResult));
    }
}
