using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.HomeOwnerModels.In;

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

    public CreatedAtActionResult CreateHomeOwner(HomeOwnerRequestModel homeOwnerRequestModel)
    {
        throw new NotImplementedException();
    }
}
