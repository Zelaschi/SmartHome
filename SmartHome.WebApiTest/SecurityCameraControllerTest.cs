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

namespace SmartHome.WebApiTest;
[TestClass]
public class SecurityCameraControllerTest
{
    private Mock<ISecurityCameraLogic>? securityCameraLogicMock;
    private SecurityCameraController? securityCameraController;

    [TestInitialize]
    public void TestInitialize()
    {
        securityCameraLogicMock = new Mock<ISecurityCameraLogic>(MockBehavior.Strict);
        securityCameraController = new SecurityCameraController(securityCameraLogicMock.Object);
    }
}
