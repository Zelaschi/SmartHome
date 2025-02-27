﻿using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Constants;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.WebModels.AdminModels.In;
using SmartHome.WebApi.WebModels.AdminModels.Out;

namespace SmartHome.WebApi.Controllers;

[Route("api/v2/admins")]
[ApiController]
[AuthenticationFilter]
public sealed class AdminsController : ControllerBase
{
    private readonly IAdminLogic _adminLogic;
    public AdminsController(IAdminLogic adminLogic)
    {
        _adminLogic = adminLogic;
    }

    [AuthorizationFilter(SeedDataConstants.CREATE_OR_DELETE_ADMIN_ACCOUNT_PERMISSION_ID)]
    [HttpPost]
    public IActionResult CreateAdmin([FromBody] AdminRequestModel adminRequestModel)
    {
        var createResponse = new AdminResponseModel(_adminLogic.CreateAdmin(adminRequestModel.ToEntitiy()));
        return CreatedAtAction("CreateAdmin", new { createResponse.Id }, createResponse);
    }

    [AuthorizationFilter(SeedDataConstants.CREATE_OR_DELETE_ADMIN_ACCOUNT_PERMISSION_ID)]
    [HttpDelete("{adminId}")]
    public IActionResult DeleteAdmin([FromRoute] Guid adminId)
    {
        _adminLogic.DeleteAdmin(adminId);
        return Ok("The admin was deleted successfully");
    }

    [AuthorizationFilter(SeedDataConstants.CREATE_OR_DELETE_ADMIN_ACCOUNT_PERMISSION_ID)]
    [HttpPatch("homeOwnerPermissions")]
    public IActionResult UpdateAdminRole()
    {
        var user = HttpContext.Items[UserStatic.User] as User;

        if (user == null)
        {
            return Unauthorized("UserId is missing");
        }

        _adminLogic.UpdateAdminRole(user);

        return Ok("Admin Permissions Updated successfully");
    }
}
