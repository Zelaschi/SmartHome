using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.LoginModels.In;
using SmartHome.WebApi.WebModels.LoginModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/v2/authentication")]
[ApiController]
[ExceptionFilter]
public sealed class AuthenticationController : ControllerBase
{
    private readonly ILoginLogic _loginLogic;

    public AuthenticationController(ILoginLogic loginLogic)
    {
        _loginLogic = loginLogic;
    }

    [HttpPost]
    public IActionResult LogIn([FromBody] LoginRequestModel loginRequest)
    {
        return Ok(new LoginResponseModel(_loginLogic.LogIn(loginRequest.Email, loginRequest.Password)));
    }
}
