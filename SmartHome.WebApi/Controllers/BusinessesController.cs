using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.Businesses.In;
using SmartHome.WebApi.WebModels.Businesses.Out;
using SmartHome.WebApi.WebModels.PaginationModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/businesses")]
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
    public IActionResult GetAllBusinesses(
        [FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null, [FromQuery] string? businessName = null, [FromQuery] string? fullName = null)
    {
        var query = _businessesLogic.GetAllBusinesses();

        if (!string.IsNullOrEmpty(businessName))
        {
            query = query.Where(u => u.Name == businessName);
        }

        if (!string.IsNullOrEmpty(fullName))
        {
            var searchTerm = fullName.ToLower();
            query = query.Where(u =>
                (u.BusinessOwner.Name.ToLower() + " " + u.BusinessOwner.Surname.ToLower()).Contains(searchTerm) ||
                (u.BusinessOwner.Surname.ToLower() + " " + u.BusinessOwner.Name.ToLower()).Contains(searchTerm)
            );
        }

        var totalCount = query.Count();
        List<BusinessesResponseModel> businessesResponse;
        if (pageNumber != null && pageSize != null)
        {
            var pagedData = query
            .Skip(((int)pageNumber - 1) * (int)pageSize)
            .Take((int)pageSize)
            .ToList();

            businessesResponse = pagedData.Select(businesses => new BusinessesResponseModel(businesses)).ToList();
            return Ok(new PaginatedResponse<BusinessesResponseModel>(businessesResponse, totalCount, (int)pageNumber, (int)pageSize));
        }
        else
        {
            businessesResponse = query.Select(businesses => new BusinessesResponseModel(businesses)).ToList();
            return Ok(businessesResponse);
        }
    }
}
