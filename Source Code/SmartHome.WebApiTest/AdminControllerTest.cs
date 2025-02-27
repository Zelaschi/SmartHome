﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Constants;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.AdminModels.In;
using SmartHome.WebApi.WebModels.AdminModels.Out;

namespace SmartHome.WebApi.Test;
[TestClass]
public class AdminControllerTest
{
    private Mock<IAdminLogic>? adminLogicMock;
    private AdminsController? adminController;
    private readonly Role admin = new Role() { Name = "Admin" };

    [TestInitialize]
    public void TestInitialize()
    {
        adminLogicMock = new Mock<IAdminLogic>(MockBehavior.Strict);
        adminController = new AdminsController(adminLogicMock.Object);
    }

    [TestMethod]
    public void CreateAdminTest_OK()
    {
        var adminRequestModel = new AdminRequestModel()
        {
            Name = "adminName",
            Surname = "adminSurname",
            Password = "adminPassword",
            Email = "adminMail@domain.com"
        };

        var admin = adminRequestModel.ToEntitiy();
        adminLogicMock.Setup(a => a.CreateAdmin(It.IsAny<User>()))
            .Returns(admin);

        var expectedResult = new AdminResponseModel(admin);
        var expectedObjecResult = new CreatedAtActionResult("CreateAdmin", "CreateAdmin", new { admin.Id }, expectedResult);

        var result = adminController.CreateAdmin(adminRequestModel) as CreatedAtActionResult;
        var adminResult = result.Value as AdminResponseModel;

        adminLogicMock.VerifyAll();
        Assert.AreEqual(expectedObjecResult.StatusCode, result.StatusCode);
        Assert.AreEqual(expectedResult, adminResult);
    }

    [TestMethod]
    public void DeleteAdminTest_OK()
    {
        var adminId = Guid.NewGuid();

        var adminToDelete = new User()
        {
            Id = adminId,
            Name = "adminName",
            Surname = "adminSurname",
            Password = "adminPassword",
            Email = "admin@gmail.com",
            Role = admin
        };

        adminLogicMock.Setup(a => a.DeleteAdmin(adminId));

        var result = adminController.DeleteAdmin(adminId) as OkObjectResult;

        adminLogicMock.VerifyAll();
        Assert.AreEqual(result.StatusCode, 200);
        Assert.AreEqual(result.Value, "The admin was deleted successfully");
    }

    [TestMethod]
    public void UpdateAdminRoleTest_OK()
    {
        var user = new User()
        {
            Id = Guid.NewGuid(),
            Name = "adminName",
            Surname = "adminSurname",
            Password = "adminPassword",
            Email = "admin@gmail.com",
            Role = admin
        };

        var httpContextMock = new Mock<HttpContext>();
        var items = new Dictionary<object, object>();
        items[UserStatic.User] = user;

        httpContextMock.Setup(x => x.Items).Returns(items);

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContextMock.Object
        };

        adminController.ControllerContext = controllerContext;

        adminLogicMock.Setup(a => a.UpdateAdminRole(user)).Verifiable();
        var result = adminController.UpdateAdminRole() as OkObjectResult;

        adminLogicMock.VerifyAll();
        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
        Assert.AreEqual("Admin Permissions Updated successfully", result.Value);
    }

    [TestMethod]
    public void UpdateAdminRole_UserIsMissing_ReturnsUnauthorized()
    {
        HttpContext httpContext = new DefaultHttpContext();
        var controllerContext = new ControllerContext() { HttpContext = httpContext };

        adminController.ControllerContext = controllerContext;

        var result = adminController.UpdateAdminRole() as UnauthorizedObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(401, result.StatusCode);
        Assert.AreEqual("UserId is missing", result.Value);
    }
}
