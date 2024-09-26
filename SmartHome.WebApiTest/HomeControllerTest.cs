using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
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

    [TestMethod]

    public void GetAllHomesByUserIdTest_Ok()
    {
        var user1Id = Guid.NewGuid();
        var user1 = new User() { Id = user1Id, Name = "a", Surname = "b", Password = "psw1", Email = "user1@gmail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var user2 = new User() { Id = Guid.NewGuid(), Name = "c", Surname = "d", Password = "psw2", Email = "user2@hotmail.com", Role = homeOwner, CreationDate = DateTime.Today };
        var home1 = new Home() { Id = Guid.NewGuid(), MainStreet = "Cuareim", DoorNumber = "1234", Latitude = "12", Longitude = "34", MaxMembers = 5, Owner = user1};
        var home2 = new Home() { Id = Guid.NewGuid(), MainStreet = "18 de Julio", DoorNumber = "5678", Latitude = "56", Longitude = "78", MaxMembers = 10, Owner = user2};
        var homes = new List<Home>() { home1, home2 };
        user1.Houses = homes;

        IUsersLogic usersLogicMock = new Mock<IUsersLogic>().Object;
        homeLogicMock.Setup(h => h.GetAllHomesByUserId(It.IsAny<Guid>())).Returns(homes);

        var expected = new OkObjectResult(new List<HomeResponseModel>
        {
            new HomeResponseModel(homes.First()),
            new HomeResponseModel(homes.Last())
        });

        var result = homeController.GetAllHomesByUserId(user1Id) as OkObjectResult;
        var objectResult = (result.Value as List<HomeResponseModel>)!;

        var expectedObject = (expected.Value as List<HomeResponseModel>)!;
        homeLogicMock.VerifyAll();

        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode) && expectedObject.First().Equals(objectResult.First()));
    }
}
