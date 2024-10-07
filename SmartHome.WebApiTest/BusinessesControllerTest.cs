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
using SmartHome.WebApi.WebModels.Businesses.In;
using SmartHome.WebApi.WebModels.Businesses.Out;
using SmartHome.WebApi.WebModels.BusinessOwnerModels.In;
using SmartHome.WebApi.WebModels.QueryParams;

namespace SmartHome.WebApiTest;
[TestClass]
public class BusinessesControllerTest
{
    private Mock<IBusinessesLogic>? businessesLogicMock;
    private BusinessesController? businessesController;
    private readonly Role businessOwner = new Role() { Name = "businessOwner" };
    [TestInitialize]
    public void TestInitialize()
    {
        businessesLogicMock = new Mock<IBusinessesLogic>(MockBehavior.Strict);
        businessesController = new BusinessesController(businessesLogicMock.Object);
    }

    [TestMethod]
    public void GetBusinessesTest_OK()
    {
        var user1 = new User() { Id = Guid.NewGuid(), Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = businessOwner };
        var company1 = new Business() { Id = Guid.NewGuid(), Name = "hikvision", Logo = "logo1", RUT = "rut1", BusinessOwner = user1 };
        var user2 = new User() { Id = Guid.NewGuid(), Name = "c", Surname = "d", Password = "psw2", Email = "mail2@mail.com", Role = businessOwner};
        var company2 = new Business() { Id = Guid.NewGuid(), Name = "kolke", Logo = "logo2", RUT = "rut2", BusinessOwner = user2 };

        IEnumerable<Business> companies = new List<Business>()
        {
            company1,
            company2
        };

        businessesLogicMock.Setup(b => b.GetAllBusinesses()).Returns(companies);

        var expected = new OkObjectResult(new List<BusinessesResponseModel>
        {
            new BusinessesResponseModel(companies.First()),
            new BusinessesResponseModel(companies.Last())
        });
        List<BusinessesResponseModel> expectedObject = (expected.Value as List<BusinessesResponseModel>)!;

        var result = businessesController.GetAllBusinesses(null, null,null,null) as OkObjectResult;
        var objectResult = (result.Value as List<BusinessesResponseModel>)!;

        businessesLogicMock.VerifyAll();
        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode) && expectedObject.First().Equals(objectResult.First()));
    }

    [TestMethod]
    public void RegisterBusinessTest_Ok()
    {
        var user = new User() { Id = Guid.NewGuid(), Name = "a", Surname = "b", Password = "psw1", Email = "mail1@mail.com", Role = businessOwner };
        var businessRequestModel = new BusinessRequestModel()
        {
            Name = "businessName",
            Logo = "businessLogo",
            RUT = "businessRUT",
        };
        var business = businessRequestModel.ToEntity();

        HttpContext httpContext = new DefaultHttpContext();
        httpContext.Items.Add("User", user);

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        businessesController = new BusinessesController(businessesLogicMock.Object) { ControllerContext = controllerContext };

        businessesLogicMock.Setup(b => b.CreateBusiness(It.IsAny<Business>(), It.IsAny<User>())).Returns(business);

        business.BusinessOwner = user;

        var expectedResult = new BusinessesResponseModel(business);
        var expectedObjectResult = new CreatedAtActionResult("CreateBusiness", "CreateBusiness", new { Id = business.Id }, expectedResult);

        var result = businessesController.CreateBusiness(businessRequestModel) as CreatedAtActionResult;
        var businessResult = result.Value as BusinessesResponseModel;

        businessesLogicMock.VerifyAll();
        Assert.IsTrue(expectedObjectResult.StatusCode.Equals(result.StatusCode) && expectedResult.Equals(businessResult));
    }
}
