using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Constants;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.InteligentLampModels.In;
using SmartHome.WebApi.WebModels.InteligentLampModels.Out;

namespace SmartHome.WebApi.Test;

[TestClass]
public class InteligentLampControllerTest
{
    private Mock<ICreateDeviceLogic>? inteligentLampLogicMock;
    private InteligentLampsController? inteligentLampsController;
    private readonly Role homeOwner = new Role() { Name = "HomeOwner" };

    [TestInitialize]
    public void TestInitialize()
    {
        inteligentLampLogicMock = new Mock<ICreateDeviceLogic>();
        inteligentLampsController = new InteligentLampsController(inteligentLampLogicMock.Object);
    }

    [TestMethod]
    public void CreateInteligentLampTest_Ok()
    {
        var user1 = new User() { Id = Guid.NewGuid(), Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var company1 = new Business() { Id = Guid.NewGuid(), Name = "hikvision", Logo = "logo1", RUT = "rut1", BusinessOwner = user1 };
        var deviceRequestModel = new InteligentLampRequestModel()
        {
            Name = "Lampara inteligente",
            Description = "Lampara inteligente",
            ModelNumber = "1234",
            Photos = []
        };

        Device device = deviceRequestModel.ToEntity();
        device.Business = company1;
        device.Id = Guid.NewGuid();
        var httpContext = new DefaultHttpContext();
        httpContext.Items.Add(UserStatic.User, user1);
        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        inteligentLampsController = new InteligentLampsController(inteligentLampLogicMock.Object) { ControllerContext = controllerContext };
        inteligentLampLogicMock.Setup(d => d.CreateDevice(It.IsAny<Device>(), It.IsAny<User>(), It.IsAny<string>())).Returns(device);
        var expectedResult = new InteligentLampResponseModel(device);
        var expectedObjectResult = new CreatedAtActionResult("CreateInteligentLamp", "InteligentLamp", new { Id = device.Id }, expectedResult);

        var result = inteligentLampsController.CreateInteligentLamp(deviceRequestModel) as CreatedAtActionResult;
        var deviceResult = result.Value as InteligentLampResponseModel;

        inteligentLampLogicMock.VerifyAll();
        Assert.IsTrue(expectedObjectResult.StatusCode.Equals(result.StatusCode) && expectedResult.Equals(deviceResult));
    }

    [TestMethod]
    public void CreateInteligentLampTest_UserIdMissing()
    {
        var deviceRequestModel = new InteligentLampRequestModel()
        {
            Name = "Lampara inteligente",
            Description = "Lampara inteligente",
            ModelNumber = "1234",
            Photos = []
        };

        var httpContext = new DefaultHttpContext();
        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        inteligentLampsController = new InteligentLampsController(inteligentLampLogicMock.Object)
        {
            ControllerContext = controllerContext
        };

        var result = inteligentLampsController.CreateInteligentLamp(deviceRequestModel) as UnauthorizedObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(401, result.StatusCode);
        Assert.AreEqual("UserId is missing", result.Value);
    }

    [TestMethod]
    public void InteligentLampsController_NullCreateDeviceLogic_ThrowsArgumentNullException()
    {
        try
        {
            var controller = new InteligentLampsController(null);
        }
        catch (ArgumentNullException ex)
        {
            Assert.AreEqual("createDeviceLogic", ex.ParamName);
        }
    }
}
