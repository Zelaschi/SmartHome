using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.SecurityCameraModels.In;
using SmartHome.WebApi.WebModels.SecurityCameraModels.Out;

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
        var user = HttpContext.Items["User"] as User;

        if (user == null)
        {
            return Unauthorized("UserId is missing");
        }

        var response = new SecurityCameraResponseModel(_securityCameraLogic.CreateSecurityCamera(securityCameraRequestModel.ToEntity(), user));
        return CreatedAtAction("CreateSecurityCamera", new { response.Id }, response);
    }
}
