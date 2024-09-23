using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.DeviceModels.In;
using SmartHome.WebApi.WebModels.DeviceModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly ILoginLogic _loginLogic;

    public AuthenticationController(ILoginLogic loginLogic)
    {
        _loginLogic = loginLogic;
    }
}
