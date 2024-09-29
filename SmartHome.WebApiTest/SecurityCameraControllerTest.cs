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
using SmartHome.WebApi.WebModels.SecurityCameraModels.In;
using SmartHome.WebApi.WebModels.SecurityCameraModels.Out;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

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

    [TestMethod]
    public void RegisterSecurityCameraTest_OK()
    {
        var businessOwnerRole = new Role() { Name = "BusinessOwner" };
        var businessOwner = new BusinessLogic.Domain.User() { Id = Guid.NewGuid(), Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = businessOwnerRole, CreationDate = DateTime.Today };
        var company = new Business() { Id = Guid.NewGuid(), Name = "hikvision", Logo = "logo1", RUT = "rut1", BusinessOwner = businessOwner };
        var securityCameraRequestModel = new SecurityCameraRequestModel()
        {
            ModelNumber = "modelNumber",
            Description = "description",
            Photos = "photoPath",
            InDoor = true,
            OutDoor = false,
            MovementDetection = true,
            PersonDetection = true,
            Company = company,
        };

        var securityCamera = securityCameraRequestModel.ToEntity();
        securityCamera.Id = Guid.NewGuid();

        securityCameraLogicMock.Setup(d => d.CreateSecurityCamera(It.IsAny<SecurityCamera>())).Returns(securityCamera);
        var expectedResult = new SecurityCameraResponseModel(securityCamera);
        var expectedSecurityCameraResult = new CreatedAtActionResult("CreateSecurityCamera", "CreateSecurityCamera", new { Id = securityCamera.Id }, expectedResult);

        var result = securityCameraController.CreateSecurityCamera(securityCameraRequestModel) as CreatedAtActionResult;
        var securityCameraResult = result.Value as SecurityCameraResponseModel;

        securityCameraLogicMock.VerifyAll();
        Assert.IsTrue(expectedSecurityCameraResult.StatusCode.Equals(result.StatusCode) && expectedResult.Equals(securityCameraResult));
    }
}
