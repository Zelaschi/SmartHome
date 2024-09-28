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
using SmartHome.WebApi.WebModels.HomeMemberModels.In;
using SmartHome.WebApi.WebModels.HomeMemberModels.Out;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace SmartHome.WebApiTest;

[TestClass]
public class HomeMemberControllerTest
{
    private Mock<IHomeMemberLogic>? homeMemberLogicMock;
    private HomeMemberController? homeMemberController;

    [TestInitialize]
    public void TestInitialize()
    {
        homeMemberLogicMock = new Mock<IHomeMemberLogic>(MockBehavior.Strict);
        homeMemberController = new HomeMemberController(homeMemberLogicMock.Object);
    }

    [TestMethod]
    public void RegisterHomeMemberTest_OK()
    {
        // ARRANGE
        var homeMemberRequestModel = new HomeMemberRequestModel()
        {
            HomeMemberId = Guid.NewGuid(),
            HomePermissions = new List<HomePermission>(),
            Notifications = new List<Notification>(),
        };

        var homeMember = homeMemberRequestModel.ToEntitiy();
        homeMemberLogicMock.Setup(h => h.CreateHomeMember(It.IsAny<HomeMember>())).Returns(homeMember);

        var expectedResult = new HomeMemberResponseModel(homeMember);
        var expectedObjectResult = new CreatedAtActionResult("CreateHomeMember", "CreateHomeMember", new { HomeMemberId = homeMember.HomeMemberId }, expectedResult);

        // ACT
        var result = homeMemberController.CreateHomeMember(homeMemberRequestModel) as CreatedAtActionResult;
        var homeMemberResult = result.Value as HomeMemberResponseModel;

        // ASSERT
        homeMemberLogicMock.VerifyAll();
        Assert.IsTrue(expectedObjectResult.StatusCode.Equals(result.StatusCode) && expectedResult.HomeMemberId.Equals(homeMemberResult.HomeMemberId));
    }

    [TestMethod]

    public void GetAllHomeMembersTest_Ok()
    {
        // ARRANGE
        IEnumerable<HomeMember> homeMembers = new List<HomeMember>()
        {
            new HomeMember() { HomeMemberId = Guid.NewGuid(), HomePermissions = new List<HomePermission>(), Notifications = new List<Notification>() },
            new HomeMember(){ HomeMemberId = Guid.NewGuid(), HomePermissions = new List<HomePermission>(), Notifications = new List<Notification>() }
        };

        homeMemberLogicMock.Setup(h => h.GetAllHomeMembers()).Returns(homeMembers);

        var expected = new OkObjectResult(new List<HomeMemberResponseModel>
        {
            new HomeMemberResponseModel(homeMembers.First()),
            new HomeMemberResponseModel(homeMembers.Last())
        });

        List<HomeMemberResponseModel> expectedObject = (expected.Value as List<HomeMemberResponseModel>)!;

        // ACT
        var result = homeMemberController.GetAllHomeMembers() as OkObjectResult;
        var objectResult = (result.Value as List<HomeMemberResponseModel>)!;

        // ASSERT
        homeMemberLogicMock.VerifyAll();
        Assert.IsTrue(result.StatusCode.Equals(expected.StatusCode) && expectedObject.First().HomeMemberId.Equals(objectResult.First().HomeMemberId));
    }
}
