using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SmartHome.Domain;
using SmartHome.Interfaces;
using SmartHome.WebApi.Controllers;

namespace SmartHome.WebApiTest;
public class AdminControllerTest
{

    private Mock<IAdminLogic> adminLogicMock;
    private AdminController adminController;

    [TestInitialize]
    public void TestInitialize()
    {
        adminLogicMock = new Mock<IAdminLogic>(MockBehavior.Strict);
        adminController = new AdminController(adminLogicMock.Object);
    }
    [TestMethod]
    public void GetAllUsersTest()
    {

    }
}
