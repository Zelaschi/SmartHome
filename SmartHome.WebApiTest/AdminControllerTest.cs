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
using SmartHome.WebApi.WebModels.AdminModels.In;
using SmartHome.WebApi.WebModels.AdminModels.Out;

namespace SmartHome.WebApiTest;
[TestClass]
public class AdminControllerTest
{
    private Mock<IAdminLogic>? adminLogicMock;
    private AdminController? adminController;

    [TestInitialize]
    public void TestInitialize()
    {
        adminLogicMock = new Mock<IAdminLogic>(MockBehavior.Strict);
        adminController = new AdminController(adminLogicMock.Object);
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
        adminLogicMock.Setup(a => a.CreateAdmin(It.IsAny<User>())).Returns(admin);

        var expectedResult = new AdminResponseModel(admin);
        var expectedObjecResult = new CreatedAtActionResult("CreateAdmin", "CreateAdmin", new { Id = admin.Id }, expectedResult);

        var result = adminController.CreateAdmin(adminRequestModel) as CreatedAtActionResult;
        var adminResult = result.Value as AdminResponseModel;

        adminLogicMock.VerifyAll();
        Assert.IsTrue(expectedObjecResult.StatusCode.Equals(result.StatusCode) && expectedResult.Equals(adminResult));
    }
}
