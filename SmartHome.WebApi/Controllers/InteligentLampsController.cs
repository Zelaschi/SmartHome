using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Constants;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.InteligentLampModels.In;
using SmartHome.WebApi.WebModels.InteligentLampModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/v2/inteligentLamps")]
[ApiController]
[AuthenticationFilter]
[ExceptionFilter]
public class InteligentLampsController : ControllerBase
{
    private readonly ICreateDeviceLogic _createDeviceLogic;
    public InteligentLampsController(ICreateDeviceLogic createDeviceLogic)
    {
        _createDeviceLogic = createDeviceLogic ?? throw new ArgumentNullException(nameof(createDeviceLogic));
    }

    [AuthorizationFilter(SeedDataConstants.CREATE_DEVICE_PERMISSION_ID)]
    [HttpPost]
    public IActionResult CreateInteligentLamp([FromBody] InteligentLampRequestModel deviceRequestModel)
    {
        var user = HttpContext.Items[UserStatic.User] as User;

        if (user == null)
        {
            return Unauthorized("UserId is missing");
        }

        var result = new InteligentLampResponseModel(_createDeviceLogic.CreateDevice(deviceRequestModel.ToEntity(), user, "Inteligent Lamp"));
        return CreatedAtAction("CreateInteligentLamp", new { result.Id }, result);
    }
}
