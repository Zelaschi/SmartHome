﻿using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.HomeOwnerModels.In;
using SmartHome.WebApi.WebModels.HomeOwnerModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/homeOwners")]
[ApiController]
[AuthenticationFilter]
[ExceptionFilter]
public sealed class HomeOwnerController : ControllerBase
{
    private readonly IHomeOwnerLogic _homeOwnerLogic;

    public HomeOwnerController(IHomeOwnerLogic homeOwnerLogic)
    {
        _homeOwnerLogic = homeOwnerLogic;
    }

    [HttpPost]
    public IActionResult CreateHomeOwner(HomeOwnerRequestModel homeOwnerRequestModel)
    {
        var createResponse = new HomeOwnerResponseModel(_homeOwnerLogic.CreateHomeOwner(homeOwnerRequestModel.ToEntitiy()));
        return CreatedAtAction("CreateHomeOwner", new { createResponse.Id }, createResponse);
    }
}
