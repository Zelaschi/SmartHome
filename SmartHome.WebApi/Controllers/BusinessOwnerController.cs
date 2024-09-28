using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.BusinessOwnerModels.In;
using SmartHome.WebApi.WebModels.BusinessOwnerModels.Out;
using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/businessOwners")]
[ApiController]
[ExceptionFilter]
public sealed class BusinessOwnerController : ControllerBase
{
    private readonly IBusinessOwnerLogic _businessOwnerLogic;
    public BusinessOwnerController(IBusinessOwnerLogic businessOwnerLogic)
    {
        _businessOwnerLogic = businessOwnerLogic;
    }

    [HttpPost]
    public IActionResult CreateBusinessOwner(BusinessOwnerRequestModel businessOwnerRequestModel)
    {
        var createResponse = new BusinessOwnerResponseModel(_businessOwnerLogic.CreateBusinessOwner(businessOwnerRequestModel.ToEntitiy()));
        return CreatedAtAction("CreateBusinessOwner", new { createResponse.Id }, createResponse);
    }
}
