using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.InteligentLampModels.In;
using SmartHome.WebApi.WebModels.InteligentLampModels.Out;

namespace SmartHome.WebApiTest;

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
        // Arrange
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
        httpContext.Items.Add("User", user1);

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        inteligentLampsController = new InteligentLampsController(inteligentLampLogicMock.Object) { ControllerContext = controllerContext };

        inteligentLampLogicMock.Setup(d => d.CreateDevice(It.IsAny<Device>(), It.IsAny<User>(), It.IsAny<string>())).Returns(device);

        var expectedResult = new InteligentLampResponseModel(device);
        var expectedObjectResult = new CreatedAtActionResult("CreateInteligentLamp", "InteligentLamp", new { Id = device.Id }, expectedResult);

        // Act
        var result = inteligentLampsController.CreateInteligentLamp(deviceRequestModel) as CreatedAtActionResult;
        var deviceResult = result.Value as InteligentLampResponseModel;

        // Assert
        inteligentLampLogicMock.VerifyAll();
        Assert.IsTrue(expectedObjectResult.StatusCode.Equals(result.StatusCode) && expectedResult.Equals(deviceResult));
    }
}
