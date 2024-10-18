using Microsoft.AspNetCore.Mvc;
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
    private readonly IInteligentLampLogic _inteligentLampLogic;
    public InteligentLampsController(IInteligentLampLogic inteligentLampLogic)
    {
        _inteligentLampLogic = inteligentLampLogic ?? throw new ArgumentNullException(nameof(inteligentLampLogic));
    }

    [AuthorizationFilter(SeedDataConstants.CREATE_DEVICE_PERMISSION_ID)]
    [HttpPost]
    public IActionResult CreateInteligentLamp(InteligentLampRequestModel deviceRequestModel)
    {
        var user = HttpContext.Items["User"] as User;

        if (user == null)
        {
            return Unauthorized("UserId is missing");
        }

        var result = new InteligentLampResponseModel(_inteligentLampLogic.CreateInteligentLamp(deviceRequestModel.ToEntity(), user));
        return CreatedAtAction("CreateInteligentLamp", new { result.Id }, result);
    }
}
