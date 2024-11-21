using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Constants;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.SecurityCameraModels.In;
using SmartHome.WebApi.WebModels.SecurityCameraModels.Out;

namespace SmartHome.WebApi.Test;
[TestClass]
public class SecurityCameraControllerTest
{
    private Mock<ISecurityCameraLogic>? securityCameraLogicMock;
    private SecurityCamerasController? securityCameraController;

    [TestInitialize]
    public void TestInitialize()
    {
        securityCameraLogicMock = new Mock<ISecurityCameraLogic>(MockBehavior.Strict);
        securityCameraController = new SecurityCamerasController(securityCameraLogicMock.Object);
    }

    [TestMethod]
    public void RegisterSecurityCameraTest_OK()
    {
        var businessOwnerRole = new Role() { Name = "BusinessOwner" };
        var businessOwner = new BusinessLogic.Domain.User() { Id = Guid.NewGuid(), Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = businessOwnerRole, CreationDate = DateTime.Today };
        var company = new Business() { Id = Guid.NewGuid(), Name = "hikvision", Logo = "logo1", RUT = "rut1", BusinessOwner = businessOwner };
        var securityCameraRequestModel = new SecurityCameraRequestModel()
        {
            Type = "CameraType",
            ModelNumber = "modelNumber",
            Description = "description",
            Name = "CameraName",
            Photos = new List<string>(),
            InDoor = true,
            OutDoor = false,
            MovementDetection = true,
            PersonDetection = true,
        };

        var securityCamera = securityCameraRequestModel.ToEntity();
        securityCamera.Id = Guid.NewGuid();
        securityCamera.Business = company;

        HttpContext httpContext = new DefaultHttpContext();
        httpContext.Items.Add(UserStatic.User, businessOwner);

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        securityCameraController = new SecurityCamerasController(securityCameraLogicMock.Object) { ControllerContext = controllerContext };

        securityCameraLogicMock.Setup(d => d.CreateSecurityCamera(It.IsAny<SecurityCamera>(), It.IsAny<BusinessLogic.Domain.User>())).Returns(securityCamera);
        var expectedResult = new SecurityCameraResponseModel(securityCamera);
        var expectedSecurityCameraResult = new CreatedAtActionResult("CreateSecurityCamera", "CreateSecurityCamera", new { Id = securityCamera.Id }, expectedResult);

        var result = securityCameraController.CreateSecurityCamera(securityCameraRequestModel) as CreatedAtActionResult;
        var securityCameraResult = result.Value as SecurityCameraResponseModel;

        securityCameraLogicMock.VerifyAll();
        Assert.IsTrue(expectedSecurityCameraResult.StatusCode.Equals(result.StatusCode) && expectedResult.Equals(securityCameraResult));
    }

    [TestMethod]
    public void CreateSecurityCamera_UserIdMissing_ReturnsUnauthorized()
    {
        var securityCameraRequestModel = new SecurityCameraRequestModel
        {
            Type = "CameraType",
            ModelNumber = "modelNumber",
            Description = "description",
            Name = "CameraName",
            Photos = new List<string>(),
            InDoor = true,
            OutDoor = false,
            MovementDetection = true,
            PersonDetection = true
        };

        HttpContext httpContext = new DefaultHttpContext();
        var controllerContext = new ControllerContext { HttpContext = httpContext };

        securityCameraController.ControllerContext = controllerContext;

        var result = securityCameraController.CreateSecurityCamera(securityCameraRequestModel) as UnauthorizedObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(401, result.StatusCode);
        Assert.AreEqual("UserId is missing", result.Value);
    }
}
