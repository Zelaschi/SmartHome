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
using SmartHome.WebApi.WebModels.DeviceModels.Out;
using SmartHome.WebApi.WebModels.HomeModels.In;
using SmartHome.WebApi.WebModels.HomeModels.Out;

namespace SmartHome.WebApiTest;

[TestClass]

public class HomeControllerTest
{
    private Mock<IHomeLogic>? homeLogicMock;
    private HomeController? homeController;
    private readonly Role homeOwner = new Role() { Name = "HomeOwner" };

    [TestInitialize]
    public void TestInitialize()
    {
        homeLogicMock = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeController = new HomeController(homeLogicMock.Object);
    }

    [TestMethod]
    public void CreateHomeTest_Ok()
    {
        var user1 = new User() { Id = Guid.NewGuid(), Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var homeRequestModel = new CreateHomeRequestModel()
        {
            Owner = user1,
            MainStreet = "Cuareim",
            DoorNumber = "1234",
            Latitude = "12",
            Longitude = "34",
            MaxMembers = 5
        };

        Home home = homeRequestModel.ToEntity();
        home.Id = Guid.NewGuid();

        homeLogicMock.Setup(h => h.CreateHome(It.IsAny<Home>())).Returns(home);
        var expectedResult = new HomeResponseModel(home);
        var expectedDeviceResult = new CreatedAtActionResult("CreateHome", "CreateHome", new { Id = home.Id }, expectedResult);

        var result = homeController.CreateHome(homeRequestModel) as CreatedAtActionResult;
        var homeResult = result.Value as HomeResponseModel;

        homeLogicMock.VerifyAll();
        Assert.IsTrue(expectedDeviceResult.StatusCode.Equals(result.StatusCode) && expectedResult.Equals(homeResult));
    }
}
