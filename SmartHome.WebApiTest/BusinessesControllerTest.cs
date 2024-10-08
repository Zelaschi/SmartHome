using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.Businesses.In;
using SmartHome.WebApi.WebModels.Businesses.Out;
using SmartHome.WebApi.WebModels.PaginationModels.Out;

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
        var user2 = new User() { Id = Guid.NewGuid(), Name = "c", Surname = "d", Password = "psw2", Email = "mail2@mail.com", Role = businessOwner };
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

        var result = businessesController.GetAllBusinesses(null, null, null, null) as OkObjectResult;
        var objectResult = (result.Value as List<BusinessesResponseModel>)!;

        businessesLogicMock.VerifyAll();
        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode) && expectedObject.First().Equals(objectResult.First()));
    }

    [TestMethod]
    public void GetBusinessesTest_WithPagination_OK()
    {
        // Arrange
        var user1 = new User()
        {
            Id = Guid.NewGuid(),
            Name = "a",
            Surname = "b",
            Password = "psw1",
            Email = "mail1@mail.com",
            Role = businessOwner
        };

        var company1 = new Business()
        {
            Id = Guid.NewGuid(),
            Name = "hikvision",
            Logo = "logo1",
            RUT = "rut1",
            BusinessOwner = user1
        };

        var company2 = new Business()
        {
            Id = Guid.NewGuid(),
            Name = "kolke",
            Logo = "logo2",
            RUT = "rut2",
            BusinessOwner = user1
        };

        var company3 = new Business()
        {
            Id = Guid.NewGuid(),
            Name = "example",
            Logo = "logo3",
            RUT = "rut3",
            BusinessOwner = user1
        };

        IEnumerable<Business> companies = new List<Business>
        {
        company1,
        company2,
        company3
        };

        businessesLogicMock.Setup(b => b.GetAllBusinesses()).Returns(companies);

        // Act
        var pageNumber = 1;
        var pageSize = 2;
        var result = businessesController.GetAllBusinesses(pageNumber, pageSize, null, null) as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);

        var resultValue = result.Value as PaginatedResponse<BusinessesResponseModel>;
        Assert.IsNotNull(resultValue);

        var returnedCompanies = resultValue.Data;
        var totalCount = resultValue.TotalCount;

        Assert.AreEqual(pageSize, returnedCompanies.Count);
        Assert.AreEqual(totalCount, companies.Count());
        Assert.AreEqual(company1.Name, returnedCompanies[0].Name);
        Assert.AreEqual(company2.Name, returnedCompanies[1].Name);
        Assert.AreEqual(pageNumber, resultValue.PageNumber);
        Assert.AreEqual(pageSize, resultValue.PageSize);
    }

    [TestMethod]
    public void GetBusinessesTest_FilterByBusinessName_OK()
    {
        // Arrange
        var user1 = new User()
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Surname = "Doe",
            Password = "psw1",
            Email = "mail1@mail.com",
            Role = businessOwner
        };

        var company1 = new Business()
        {
            Id = Guid.NewGuid(),
            Name = "hikvision",
            Logo = "logo1",
            RUT = "rut1",
            BusinessOwner = user1
        };

        var company2 = new Business()
        {
            Id = Guid.NewGuid(),
            Name = "kolke",
            Logo = "logo2",
            RUT = "rut2",
            BusinessOwner = user1
        };

        var company3 = new Business()
        {
            Id = Guid.NewGuid(),
            Name = "example",
            Logo = "logo3",
            RUT = "rut3",
            BusinessOwner = user1
        };

        IEnumerable<Business> companies = new List<Business>
    {
        company1,
        company2,
        company3
    };

        businessesLogicMock.Setup(b => b.GetAllBusinesses()).Returns(companies);

        // Act
        var result = businessesController.GetAllBusinesses(null, null, "hikvision", null) as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);

        var resultValue = result.Value as List<BusinessesResponseModel>;
        Assert.IsNotNull(resultValue);

        Assert.AreEqual(1, resultValue.Count);
        Assert.AreEqual(company1.Name, resultValue[0].Name);
    }

    [TestMethod]
    public void GetBusinessesTest_FilterByFullName_OK()
    {
        // Arrange
        var user1 = new User()
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Surname = "Doe",
            Password = "psw1",
            Email = "mail1@mail.com",
            Role = businessOwner
        };
        var user2 = new User()
        {
            Id = Guid.NewGuid(),
            Name = "Juan",
            Surname = "Masa",
            Password = "psw1",
            Email = "mail1@mail.com",
            Role = businessOwner
        };

        var company1 = new Business()
        {
            Id = Guid.NewGuid(),
            Name = "hikvision",
            Logo = "logo1",
            RUT = "rut1",
            BusinessOwner = user2
        };

        var company2 = new Business()
        {
            Id = Guid.NewGuid(),
            Name = "kolke",
            Logo = "logo2",
            RUT = "rut2",
            BusinessOwner = user1
        };

        var company3 = new Business()
        {
            Id = Guid.NewGuid(),
            Name = "example",
            Logo = "logo3",
            RUT = "rut3",
            BusinessOwner = user1
        };

        IEnumerable<Business> companies = new List<Business>
        {
        company1,
        company2,
        company3
        };

        businessesLogicMock.Setup(b => b.GetAllBusinesses()).Returns(companies);

        // Act
        var result = businessesController.GetAllBusinesses(null, null, null, "John Doe") as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);

        var resultValue = result.Value as List<BusinessesResponseModel>;
        Assert.IsNotNull(resultValue);

        Assert.AreEqual(2, resultValue.Count);
    }

    [TestMethod]
    public void GetBusinessesTest_FilterByFullName_NotFound()
    {
        // Arrange
        var user1 = new User()
        {
            Id = Guid.NewGuid(),
            Name = "Alice",
            Surname = "Smith",
            Password = "psw1",
            Email = "mail1@mail.com",
            Role = businessOwner
        };

        var company1 = new Business()
        {
            Id = Guid.NewGuid(),
            Name = "hikvision",
            Logo = "logo1",
            RUT = "rut1",
            BusinessOwner = user1
        };

        IEnumerable<Business> companies = new List<Business>
    {
        company1
    };

        businessesLogicMock.Setup(b => b.GetAllBusinesses()).Returns(companies);

        // Act
        var result = businessesController.GetAllBusinesses(null, null, null, "John Doe") as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);

        var resultValue = result.Value as List<BusinessesResponseModel>;
        Assert.IsNotNull(resultValue);

        Assert.AreEqual(0, resultValue.Count);
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
