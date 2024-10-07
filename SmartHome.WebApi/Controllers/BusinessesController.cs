using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.Businesses.Out;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.Businesses.In;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.WebApi.WebModels.QueryParams;
using SmartHome.WebApi.WebModels.UserModels.Out;

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
        [FromQuery] Pagination paginationParams, [FromQuery] string? businessName = null, [FromQuery] string? fullName = null)
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

        var pagedData = query
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToList();

        var usersResponse = pagedData.Select(businesses => new BusinessesResponseModel(businesses)).ToList();

        return Ok(new
        {
            Data = usersResponse,
            TotalCount = totalCount,
            PageNumber = paginationParams.PageNumber,
            PageSize = paginationParams.PageSize
        });
    }
}
