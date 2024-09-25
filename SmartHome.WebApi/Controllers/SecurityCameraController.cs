using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.SecurityCameraModels.In;
using SmartHome.WebApi.WebModels.SecurityCameraModels.Out;
using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/securityCameras")]
[ApiController]
[ExceptionFilter]
public class SecurityCameraController : ControllerBase
{
    private readonly ISecurityCameraLogic _securityCameraLogic;
    public SecurityCameraController(ISecurityCameraLogic securityCameraLogic)
    {
        _securityCameraLogic = securityCameraLogic;
    }

    [HttpPost]
    public IActionResult CreateSecurityCamera(SecurityCameraRequestModel securityCameraRequestModel)
    {
        var response = new SecurityCameraResponseModel(_securityCameraLogic.CreateSecurityCamera(securityCameraRequestModel.ToEntity()));
        return CreatedAtAction("CreateSecurityCamera", new { response.Id }, response);
    }
}
