using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.DeviceModels.In;
using SmartHome.WebApi.WebModels.DeviceModels.Out;
using SmartHome.WebApi.WebModels.LoginModels.In;
using SmartHome.WebApi.WebModels.LoginModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly ILoginLogic _loginLogic;

    public AuthenticationController(ILoginLogic loginLogic)
    {
        _loginLogic = loginLogic;
    }

    [HttpPost]
    public IActionResult LogIn(LoginRequestModel loginRequest)
    {
        return Ok(new LoginResponseModel() { Token = _loginLogic.LogIn(loginRequest.Email, loginRequest.Password) });
    }
}
