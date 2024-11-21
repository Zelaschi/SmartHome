using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Constants;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.DTOs;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.Businesses.In;
using SmartHome.WebApi.WebModels.Businesses.Out;
using SmartHome.WebApi.WebModels.PaginationModels.Out;

namespace SmartHome.WebApi.Test;
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
        var user1 = new User
        {
            Id = Guid.NewGuid(),
            Name = "a",
            Surname = "b",
            Password = "psw1",
            Email = "mail1@mail.com",
            Role = businessOwner
        };
        var company1 = new Business
        {
            Id = Guid.NewGuid(),
            Name = "hikvision",
            Logo = "logo1",
            RUT = "rut1",
            BusinessOwner = user1
        };
        var user2 = new User
        {
            Id = Guid.NewGuid(),
            Name = "c",
            Surname = "d",
            Password = "psw2",
            Email = "mail2@mail.com",
            Role = businessOwner
        };
        var company2 = new Business
        { Id = Guid.NewGuid(), Name = "kolke", Logo = "logo2", RUT = "rut2", BusinessOwner = user2 };

        IEnumerable<Business> companies = new List<Business>()
        {
            company1,
            company2
        };

        businessesLogicMock.Setup(b => b.GetBusinesses(null, null, null, null)).Returns(companies);

        var expected = new OkObjectResult(new List<BusinessesResponseModel>
        {
            new BusinessesResponseModel(companies.First()),
            new BusinessesResponseModel(companies.Last())
        });
        List<BusinessesResponseModel> expectedObject = (expected.Value as List<BusinessesResponseModel>)!;

        var result = businessesController.GetBusinesses(null, null, null, null) as OkObjectResult;
        var objectResult = (result.Value as List<BusinessesResponseModel>)!;

        businessesLogicMock.VerifyAll();
        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode) && expectedObject.First().Equals(objectResult.First()));
    }

    [TestMethod]
    public void GetBusinessesTest_WithPagination_OK()
    {
        var user1 = new User
        {
            Id = Guid.NewGuid(),
            Name = "a",
            Surname = "b",
            Password = "psw1",
            Email = "mail1@mail.com",
            Role = businessOwner
        };

        var company1 = new Business
        {
            Id = Guid.NewGuid(),
            Name = "hikvision",
            Logo = "logo1",
            RUT = "rut1",
            BusinessOwner = user1
        };

        var company2 = new Business
        {
            Id = Guid.NewGuid(),
            Name = "kolke",
            Logo = "logo2",
            RUT = "rut2",
            BusinessOwner = user1
        };

        var company3 = new Business
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
        };
        var pageNumber = 1;
        var pageSize = 2;

        businessesLogicMock.Setup(b => b.GetBusinesses(pageNumber, pageSize, null, null)).Returns(companies);

        var result = businessesController.GetBusinesses(pageNumber, pageSize, null, null) as OkObjectResult;

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
        var hikvision = "hikvision";
        var user1 = new User
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
            Name = hikvision,
            Logo = "logo1",
            RUT = "rut1",
            BusinessOwner = user1
        };

        var company2 = new Business
        {
            Id = Guid.NewGuid(),
            Name = "kolke",
            Logo = "logo2",
            RUT = "rut2",
            BusinessOwner = user1
        };

        var company3 = new Business
        {
            Id = Guid.NewGuid(),
            Name = "example",
            Logo = "logo3",
            RUT = "rut3",
            BusinessOwner = user1
        };

        IEnumerable<Business> companies = new List<Business>
        {
            company1
        };

        businessesLogicMock.Setup(b => b.GetBusinesses(null, null, hikvision, null)).Returns(companies);

        var result = businessesController.GetBusinesses(null, null, hikvision, null) as OkObjectResult;

        Assert.IsNotNull(result);

        var resultValue = result.Value as List<BusinessesResponseModel>;
        Assert.IsNotNull(resultValue);

        Assert.AreEqual(1, resultValue.Count);
        Assert.AreEqual(company1.Name, resultValue[0].Name);
    }

    [TestMethod]
    public void GetBusinessesTest_FilterByFullName_OK()
    {
        var user1 = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Surname = "Doe",
            Password = "psw1",
            Email = "mail1@mail.com",
            Role = businessOwner
        };
        var user2 = new User
        {
            Id = Guid.NewGuid(),
            Name = "Juan",
            Surname = "Masa",
            Password = "psw1",
            Email = "mail1@mail.com",
            Role = businessOwner
        };

        var company1 = new Business
        {
            Id = Guid.NewGuid(),
            Name = "hikvision",
            Logo = "logo1",
            RUT = "rut1",
            BusinessOwner = user2
        };

        var company2 = new Business
        {
            Id = Guid.NewGuid(),
            Name = "kolke",
            Logo = "logo2",
            RUT = "rut2",
            BusinessOwner = user1
        };

        var company3 = new Business
        {
            Id = Guid.NewGuid(),
            Name = "example",
            Logo = "logo3",
            RUT = "rut3",
            BusinessOwner = user1
        };

        IEnumerable<Business> companies = new List<Business>
        {
        company2,
        company3
        };

        businessesLogicMock.Setup(b => b.GetBusinesses(null, null, null, "John Doe")).Returns(companies);

        var result = businessesController.GetBusinesses(null, null, null, "John Doe") as OkObjectResult;

        Assert.IsNotNull(result);

        var resultValue = result.Value as List<BusinessesResponseModel>;
        Assert.IsNotNull(resultValue);

        Assert.AreEqual(2, resultValue.Count);
    }

    [TestMethod]
    public void GetBusinessesTest_FilterByFullName_NotFound()
    {
        var user1 = new User
        {
            Id = Guid.NewGuid(),
            Name = "Alice",
            Surname = "Smith",
            Password = "psw1",
            Email = "mail1@mail.com",
            Role = businessOwner
        };

        var company1 = new Business
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
        IEnumerable<Business> returnedCompanies = new List<Business>();

        businessesLogicMock.Setup(b => b.GetBusinesses(null, null, null, "John Doe")).Returns(returnedCompanies);

        var result = businessesController.GetBusinesses(null, null, null, "John Doe") as OkObjectResult;

        Assert.IsNotNull(result);

        var resultValue = result.Value as List<BusinessesResponseModel>;
        Assert.IsNotNull(resultValue);

        Assert.AreEqual(0, resultValue.Count);
    }

    [TestMethod]
    public void RegisterBusinessTest_Ok()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "a",
            Surname = "b",
            Password = "psw1",
            Email = "mail1@mail.com",
            Role = businessOwner
        };
        var businessRequestModel = new BusinessRequestModel
        {
            Name = "businessName",
            Logo = "businessLogo",
            RUT = "businessRUT",
        };
        var business = businessRequestModel.ToEntity();

        HttpContext httpContext = new DefaultHttpContext();
        httpContext.Items.Add(UserStatic.User, user);

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        businessesController = new BusinessesController(businessesLogicMock.Object) { ControllerContext = controllerContext };

        businessesLogicMock.Setup(b => b.CreateBusiness(It.IsAny<Business>(), It.IsAny<User>()))
            .Returns(business);

        business.BusinessOwner = user;

        var expectedResult = new BusinessesResponseModel(business);
        var expectedObjectResult = new CreatedAtActionResult("CreateBusiness", "CreateBusiness", new { Id = business.Id }, expectedResult);

        var result = businessesController.CreateBusiness(businessRequestModel) as CreatedAtActionResult;
        var businessResult = result.Value as BusinessesResponseModel;

        businessesLogicMock.VerifyAll();
        Assert.AreEqual(expectedObjectResult.StatusCode, result.StatusCode);
        Assert.AreEqual(expectedResult, businessResult);
    }

    [TestMethod]
    public void CreateBusiness_UserIdMissing_ReturnsUnauthorized()
    {
        var businessRequestModel = new BusinessRequestModel
        {
            Name = "Business Test",
            Logo = "LogoUrl",
            RUT = "123456789"
        };

        HttpContext httpContext = new DefaultHttpContext();
        var controllerContext = new ControllerContext { HttpContext = httpContext };

        var businessController = new BusinessesController(businessesLogicMock.Object)
        {
            ControllerContext = controllerContext
        };

        var result = businessController.CreateBusiness(businessRequestModel) as UnauthorizedObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(401, result.StatusCode);
        Assert.AreEqual("UserId is missing", result.Value);
    }

    [TestMethod]
    public void AddValidatorToBusiness_UserAuthenticated_ShouldAddValidator()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Business Owner",
            Surname = "Owner",
            Password = "password",
            Email = "owner@example.com",
            Role = businessOwner
        };

        var validatorId = Guid.NewGuid();
        var updatedBusiness = new Business
        {
            Id = Guid.NewGuid(),
            Name = "Updated Business",
            Logo = "logo.jpg",
            RUT = "123456789",
            BusinessOwner = user
        };

        var httpContext = new DefaultHttpContext();
        httpContext.Items.Add(UserStatic.User, user);

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        businessesController = new BusinessesController(businessesLogicMock.Object)
        {
            ControllerContext = controllerContext
        };

        businessesLogicMock.Setup(b => b.AddValidatorToBusiness(user, validatorId))
            .Returns(updatedBusiness);

        var validator = new ValidatorIdRequestModel { Id = validatorId };

        var result = businessesController.AddValidatorToBusiness(validator) as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
        var businessResult = result.Value as Business;
        Assert.IsNotNull(businessResult);
        Assert.AreEqual(updatedBusiness.Id, businessResult.Id);
        Assert.AreEqual(updatedBusiness.Name, businessResult.Name);
        businessesLogicMock.VerifyAll();
    }

    [TestMethod]
    public void AddValidatorToBusiness_UserNotAuthenticated_ShouldReturnUnauthorized()
    {
        var validatorId = Guid.NewGuid();

        var httpContext = new DefaultHttpContext();
        var controllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        businessesController = new BusinessesController(businessesLogicMock.Object)
        {
            ControllerContext = controllerContext
        };

        var validator = new ValidatorIdRequestModel { Id = validatorId };

        var result = businessesController.AddValidatorToBusiness(validator) as UnauthorizedObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(401, result.StatusCode);
        Assert.AreEqual("UserId is missing", result.Value);
    }

    [TestMethod]
    public void GetAllValidators_ShouldReturnAllValidators()
    {
        var validators = new List<DTOValidator>
        {
            new DTOValidator { ValidatorId = Guid.NewGuid(), Name = "Validator 1" },
            new DTOValidator { ValidatorId = Guid.NewGuid(), Name = "Validator 2" }
        };

        businessesLogicMock.Setup(b => b.GetAllValidators()).Returns(validators);

        var result = businessesController.GetAllValidators() as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);

        var resultValue = result.Value as List<DTOValidator>;
        Assert.IsNotNull(resultValue);
        Assert.AreEqual(2, resultValue.Count);
        Assert.AreEqual(validators[0].Name, resultValue[0].Name);
        Assert.AreEqual(validators[1].Name, resultValue[1].Name);

        businessesLogicMock.VerifyAll();
    }

    [TestMethod]
    public void GetBusinessById_ValidUser_ReturnsBusiness()
    {
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "TestUser",
            Surname = "UserSurname",
            Email = "testuser@example.com",
            Password = "password",
            Role = businessOwner,
            CreationDate = DateTime.Now
        };

        var business = new Business
        {
            Id = Guid.NewGuid(),
            Name = "Test Business",
            Logo = "TestLogo",
            RUT = "1234",
            BusinessOwner = user
        };

        var httpContext = new DefaultHttpContext();
        httpContext.Items[UserStatic.User] = user;
        businessesController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        businessesLogicMock
            .Setup(x => x.GetBusinessByUser(user))
            .Returns(business);

        var result = businessesController.GetBusinessById();

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);

        var responseModel = okResult.Value as BusinessesResponseModel;
        Assert.IsNotNull(responseModel);
        Assert.AreEqual(business.Id.ToString(), responseModel.Id);
        Assert.AreEqual(business.Name, responseModel.Name);

        businessesLogicMock.Verify(x => x.GetBusinessByUser(user), Times.Once);
    }

    [TestMethod]
    public void GetBusinessById_UserMissing_ReturnsUnauthorized()
    {
        var httpContext = new DefaultHttpContext();
        businessesController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        var result = businessesController.GetBusinessById();

        Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
        var unauthorizedResult = result as UnauthorizedObjectResult;
        Assert.AreEqual("User is missing", unauthorizedResult?.Value);
    }
}
