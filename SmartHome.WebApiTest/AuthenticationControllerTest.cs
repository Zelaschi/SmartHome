using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.LoginModels.In;
using SmartHome.WebApi.WebModels.LoginModels.Out;

namespace SmartHome.WebApiTest;

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
        var expectedLoginResponse = new LoginResponseModel() { Token = token };

        loginLogicMock.Setup(l => l.LogIn(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedLoginResponse.ToEntity());

        var expected = new OkObjectResult(expectedLoginResponse);

        // ACT
        var result = authController.LogIn(loginRequest) as OkObjectResult;
        var objectResult = (result.Value as LoginResponseModel)!;

        // ASSERT
        loginLogicMock.VerifyAll();
        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode) && expectedLoginResponse.Equals(objectResult));
    }
}
