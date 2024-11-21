using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.HomeOwnerModels.In;
using SmartHome.WebApi.WebModels.HomeOwnerModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/v2/homeOwners")]
[ApiController]
public sealed class HomeOwnersController : ControllerBase
{
    private readonly IHomeOwnerLogic _homeOwnerLogic;

    public HomeOwnersController(IHomeOwnerLogic homeOwnerLogic)
    {
        _homeOwnerLogic = homeOwnerLogic;
    }

    [HttpPost]
    public IActionResult CreateHomeOwner([FromBody] HomeOwnerRequestModel homeOwnerRequestModel)
    {
        var createResponse = new HomeOwnerResponseModel(_homeOwnerLogic.CreateHomeOwner(homeOwnerRequestModel.ToEntitiy()));
        return CreatedAtAction("CreateHomeOwner", new { createResponse.Id }, createResponse);
    }
}
