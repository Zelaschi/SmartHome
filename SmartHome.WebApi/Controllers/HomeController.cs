using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.HomeModels.Out;
using SmartHome.WebApi.WebModels.HomeModels.In;
using SmartHome.WebApi.Filters;
using System.Reflection.Metadata.Ecma335;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[ExceptionFilter]
public sealed class HomeController : ControllerBase
{
    private readonly IHomeLogic _homeLogic;

    public HomeController(IHomeLogic homeLogic)
    {
        _homeLogic = homeLogic;
    }

    [HttpPost("{homeId}/homeDevices")]
    public IActionResult AddDeviceToHome([FromRoute] Guid homeId, [FromBody] Guid deviceId)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public IActionResult CreateHome([FromBody] CreateHomeRequestModel homeRequestModel)
    {
        var response = new HomeResponseModel(_homeLogic.CreateHome(homeRequestModel.ToEntity()));
        return CreatedAtAction("CreateHome", new { response.Id }, response);
    }

    [HttpGet("{userId}")]
    public IActionResult GetAllHomesByUserId([FromRoute] Guid userId)
    {
        return Ok(_homeLogic.GetAllHomesByUserId(userId).Select(home => new HomeResponseModel(home)).ToList());
    }
}
