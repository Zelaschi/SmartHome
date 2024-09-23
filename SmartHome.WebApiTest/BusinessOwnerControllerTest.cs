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
public class BusinessOwnerControllerTest
{
    private Mock<IBusinessOwnerLogic>? businessOwnerLogicMock;
    private BusinessOwnerController? businessOwnerController;

    [TestInitialize]
    public void TestInitialize()
    {
        businessOwnerLogicMock = new Mock<IBusinessOwnerLogic>(MockBehavior.Strict);
        businessOwnerController = new BusinessOwnerController(businessOwnerLogicMock.Object);
    }
}
