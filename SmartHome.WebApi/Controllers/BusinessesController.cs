using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.Businesses.Out;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.Businesses.In;
using SmartHome.BusinessLogic.Domain;

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

    [HttpGet]
    public IActionResult GetAllBusinesses()
    {
        return Ok(_businessesLogic.GetAllBusinesses().Select(businesses => new BusinessesResponseModel(businesses)).ToList());
    }
}
