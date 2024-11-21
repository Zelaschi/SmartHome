using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.WebModels.DeviceModels.Out;

namespace SmartHome.WebApi.Test;

[TestClass]
public class DeviceTypesControllerTest
{
    private Mock<IDeviceLogic>? deviceLogicMock;
    private DeviceTypesController? deviceTypeController;

    [TestInitialize]

    public void TestInitialize()
    {
        deviceLogicMock = new Mock<IDeviceLogic>();
        deviceTypeController = new DeviceTypesController(deviceLogicMock.Object);
    }

    [TestMethod]

    public void GetAll_DeviceTypesTest_Ok()
    {
        IEnumerable<string> deviceTypes = new List<string>()
        {
            "Window Sensor",
            "Security Camera"
        };

        deviceLogicMock.Setup(d => d.GetAllDeviceTypes())
            .Returns(deviceTypes);

        var expected = new OkObjectResult(new List<DeviceTypesResponseModel>
        {
            new DeviceTypesResponseModel(deviceTypes.First()),
            new DeviceTypesResponseModel(deviceTypes.Last())
        });

        List<DeviceTypesResponseModel> expectedObject = (expected.Value as List<DeviceTypesResponseModel>)!;

        var result = deviceTypeController.GetAllDeviceTypes() as OkObjectResult;
        var objectResult = (result.Value as List<DeviceTypesResponseModel>)!;

        deviceLogicMock.VerifyAll();
        Assert.AreEqual(result.StatusCode, expected.StatusCode);
        Assert.AreEqual(expectedObject.First().Type, objectResult.First().Type);
    }

    [TestMethod]
    public void DeviceTypesController_NullDeviceLogic_ThrowsArgumentNullException()
    {
        try
        {
            var controller = new DeviceTypesController(null);
        }
        catch (ArgumentNullException ex)
        {
            Assert.AreEqual("deviceLogic", ex.ParamName);
        }
    }
}
