using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;

namespace SmartHome.WebApiTest;

[TestClass]
public class HomeOwnerControllerTest
{
    private Mock<IHomeOwnerLogic>? homeOwnerLogicMock;
    private HomeOwnerController? homeOwnerController;

    [TestInitialize]
    public void TestInitialize()
    {
        homeOwnerLogicMock = new Mock<IHomeOwnerLogic>();
        homeOwnerController = new HomeOwnerController(homeOwnerLogicMock.Object);
    }
}
