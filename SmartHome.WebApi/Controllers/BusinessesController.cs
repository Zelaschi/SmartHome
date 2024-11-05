using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.Businesses.Out;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.Businesses.In;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.WebApi.WebModels.UserModels.Out;
using System.Linq;
using SmartHome.WebApi.WebModels.PaginationModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/v2/businesses")]
[ApiController]
[AuthenticationFilter]
[ExceptionFilter]
public sealed class BusinessesController : ControllerBase
{
    private readonly IBusinessesLogic _businessesLogic;

    public BusinessesController(IBusinessesLogic businessesLogic)
    {
        _businessesLogic = businessesLogic;
    }

    [AuthorizationFilter(SeedDataConstants.CREATE_BUSINESS_PERMISSION_ID)]
    [HttpPost]
    public IActionResult CreateBusiness([FromBody] BusinessRequestModel businessRequestModel)
    {
        var user = HttpContext.Items["User"] as User;

        if (user == null)
        {
            return Unauthorized("UserId is missing");
        }

        var response = new BusinessesResponseModel(_businessesLogic.CreateBusiness(businessRequestModel.ToEntity(), user));
        return CreatedAtAction("CreateBusiness", new { response.Id }, response);
    }

    [AuthorizationFilter(SeedDataConstants.LIST_ALL_BUSINESSES_PERMISSION_ID)]
    [HttpGet]
    public IActionResult GetBusinesses(
        [FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null, [FromQuery] string? businessName = null, [FromQuery] string? fullName = null)
    {
        var query = _businessesLogic.GetBusinesses(pageNumber, pageSize, businessName, fullName);
        var businessesResponse = query.Select(businesses => new BusinessesResponseModel(businesses)).ToList();
        if (pageNumber != null && pageSize != null)
        {
            return Ok(new PaginatedResponse<BusinessesResponseModel>(businessesResponse, query.Count(), (int)pageNumber, (int)pageSize));
        }
        else
        {
            return Ok(businessesResponse);
        }
    }
}
