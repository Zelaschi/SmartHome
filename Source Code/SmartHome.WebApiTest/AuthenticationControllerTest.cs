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

    [TestInitialize]
    public void TestInitialize()
    {
        loginLogicMock = new Mock<ILoginLogic>(MockBehavior.Strict);
        authController = new AuthenticationController(loginLogicMock.Object);
    }

    [TestMethod]
    public void LoginTest_Ok()
    {
        var loginRequest = new LoginRequestModel()
        {
            Email = "aemail@domain.com",
            Password = "aPassword"
        };

        var token = Guid.NewGuid();
        var sessionAndSP = new DTOSessionAndSystemPermissions
        {
            SessionId = token,
            SystemPermissions = new List<SystemPermission>
            {
                new SystemPermission
                {
                    Name = "READ",
                    Description = "read"
                }
            }
        };

        var expectedLoginResponse = new LoginResponseModel(sessionAndSP);

        loginLogicMock.Setup(l => l.LogIn(It.IsAny<string>(), It.IsAny<string>()))
                      .Returns(sessionAndSP);

        var result = authController.LogIn(loginRequest) as OkObjectResult;
        Assert.IsNotNull(result, "Expected OkObjectResult, but got null");

        var objectResult = result.Value as LoginResponseModel;
        Assert.IsNotNull(objectResult, "Expected LoginResponseModel, but got null");
        Assert.AreEqual(200, result.StatusCode, "Expected status code 200");
        Assert.AreEqual(token, objectResult.Token, "Tokens do not match");
        CollectionAssert.AreEqual(expectedLoginResponse.SystemPermissions, objectResult.SystemPermissions);
    }
}
