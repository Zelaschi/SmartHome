using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.BusinessOwnerModels.In;
using SmartHome.WebApi.WebModels.BusinessOwnerModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/businessOwners")]
[ApiController]
[AuthenticationFilter]
[ExceptionFilter]
public sealed class BusinessOwnerController : ControllerBase
{
    private readonly IBusinessOwnerLogic _businessOwnerLogic;
    public BusinessOwnerController(IBusinessOwnerLogic businessOwnerLogic)
    {
        _businessOwnerLogic = businessOwnerLogic;
    }

    [AuthorizationFilter(SeedDataConstants.CREATE_BUSINESS_OWNER_ACCOUNT_PERMISSION_ID)]
    [HttpPost]
    public IActionResult CreateBusinessOwner([FromBody] BusinessOwnerRequestModel businessOwnerRequestModel)
    {
        var createResponse = new BusinessOwnerResponseModel(_businessOwnerLogic.CreateBusinessOwner(businessOwnerRequestModel.ToEntitiy()));
        return CreatedAtAction("CreateBusinessOwner", new { createResponse.Id }, createResponse);
    }
}
