using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.WebApi.Controllers;

public class HomeController : ControllerBase
{
    private readonly IHomeLogic _homeLogic;

    public HomeController(IHomeLogic homeLogic)
    {
        _homeLogic = homeLogic;
    }

    [HttpPost]
    public IActionResult CreateHome()
    {
    }
}
