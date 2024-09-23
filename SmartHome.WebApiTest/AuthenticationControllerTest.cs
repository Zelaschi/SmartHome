using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;

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
}
