using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.HomeMemberModels.In;
using SmartHome.WebApi.WebModels.HomeMemberModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/homeMembers")]
[ApiController]
public class HomeMemberController : ControllerBase
{
    private readonly IHomeMemberLogic _homeMemberLogic;

    public HomeMemberController(IHomeMemberLogic homeMemberLogic)
    {
        _homeMemberLogic = homeMemberLogic ?? throw new ArgumentNullException(nameof(homeMemberLogic));
    }

    [HttpPost]
    public CreatedAtActionResult CreateHomeMember(HomeMemberRequestModel homeMemberRequestModel)
    {
        var createResponse = new HomeMemberResponseModel(_homeMemberLogic.CreateHomeMember(homeMemberRequestModel.ToEntitiy()));
        return CreatedAtAction("CreateHomeMember", new { createResponse.HomeMemberId }, createResponse);
    }
}
