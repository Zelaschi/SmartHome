using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.HomeModels.Out;
using SmartHome.WebApi.WebModels.HomeModels.In;
using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[ExceptionFilter]
public class HomeController : ControllerBase
{
    private readonly IHomeLogic _homeLogic;

    public HomeController(IHomeLogic homeLogic)
    {
        _homeLogic = homeLogic;
    }

    [HttpPost]
    public IActionResult CreateHome([FromBody] CreateHomeRequestModel homeRequestModel)
    {
        var response = new HomeResponseModel(_homeLogic.CreateHome(homeRequestModel.ToEntity()));
        return CreatedAtAction("CreateHome", new { response.Id }, response);
    }
}
