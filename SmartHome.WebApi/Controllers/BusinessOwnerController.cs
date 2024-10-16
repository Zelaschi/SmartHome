using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.BusinessOwnerModels.In;
using SmartHome.WebApi.WebModels.BusinessOwnerModels.Out;
using SmartHome.WebApi.Filters;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Domain;

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

    [HttpPatch("/homeOwnerPermissions")]

    public IActionResult UpdateBusinessOwnerRole()
    {
        var user = HttpContext.Items["User"] as User;

        if (user == null)
        {
            return Unauthorized("UserId is missing");
        }

        _businessOwnerLogic.UpdateBusinessOwnerRole(user);

        return Ok("BusinessOwner Permissions Updated successfully");
    }
}
