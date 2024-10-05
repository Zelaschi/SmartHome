using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.SecurityCameraModels.In;
using SmartHome.WebApi.WebModels.SecurityCameraModels.Out;
using SmartHome.WebApi.Filters;
using SmartHome.BusinessLogic.InitialSeedData;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/securityCameras")]
[ApiController]
[AuthenticationFilter]
[ExceptionFilter]
public sealed class SecurityCameraController : ControllerBase
{
    private readonly ISecurityCameraLogic _securityCameraLogic;
    public SecurityCameraController(ISecurityCameraLogic securityCameraLogic)
    {
        _securityCameraLogic = securityCameraLogic;
    }

    [AuthorizationFilter(SeedDataConstants.CREATE_DEVICE_PERMISSION_ID)]
    [HttpPost]
    public IActionResult CreateSecurityCamera([FromBody] SecurityCameraRequestModel securityCameraRequestModel)
    {
        var response = new SecurityCameraResponseModel(_securityCameraLogic.CreateSecurityCamera(securityCameraRequestModel.ToEntity()));
        return CreatedAtAction("CreateSecurityCamera", new { response.Id }, response);
    }
}
