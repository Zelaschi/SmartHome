using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/homeOwners")]
[ApiController]
public class HomeOwnerController : ControllerBase
{
    private readonly IHomeOwnerLogic _homeOwnerLogic;

    public HomeOwnerController(IHomeOwnerLogic homeOwnerLogic)
    {
        _homeOwnerLogic = homeOwnerLogic;
    }
}
