using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.Businesses.Out;
using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/businesses")]
[ApiController]
[ExceptionFilter]
public class BusinessesController : ControllerBase
{
    private readonly IBusinessesLogic _businessesLogic;

    public BusinessesController(IBusinessesLogic businessesLogic)
    {
        _businessesLogic = businessesLogic;
    }

    [HttpGet]
    public IActionResult GetAllBusinesses()
    {
        return Ok(_businessesLogic.GetAllBusinesses().Select(businesses => new BusinessesResponseModel(businesses)).ToList());
    }
}
