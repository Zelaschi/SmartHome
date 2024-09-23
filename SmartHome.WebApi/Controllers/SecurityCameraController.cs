using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.SecurityCameraModels.In;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/securityCameras")]
[ApiController]
public class SecurityCameraController : ControllerBase
{
    private readonly ISecurityCameraLogic _securityCameraLogic;
    public SecurityCameraController(ISecurityCameraLogic securityCameraLogic)
    {
        _securityCameraLogic = securityCameraLogic;
    }

    public CreatedAtActionResult CreateSecurityCamera(SecurityCameraRequestModel securityCameraRequestModel)
    {
        throw new NotImplementedException();
    }
}
