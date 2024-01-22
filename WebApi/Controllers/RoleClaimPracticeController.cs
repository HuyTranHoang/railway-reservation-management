using Domain.Constants;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers;

public class RoleClaimPracticeController : BaseApiController
{
    [HttpGet("public")]
    public IActionResult GetPublic()
    {
        return Ok("Public endpoint");
    }

    #region Role

    [HttpGet("admin-role")]
    [Authorize(Roles = SD.AdminRole)]
    public IActionResult GetAdminRole()
    {
        return Ok("Admin role endpoint");
    }

    [HttpGet("super-admin-role")]
    [Authorize(Roles = SD.SuperAdminRole)]
    public IActionResult GetSuperAdminRole()
    {
        return Ok("Super admin role endpoint");
    }

    #endregion

    #region Claim Policy

    [HttpGet("admin-policy")]
    [Authorize(policy: "AdminPolicy")]
    public IActionResult GetAdminPolicy()
    {
        return Ok("Admin policy endpoint");
    }

    [HttpGet("super-admin-policy")]
    [Authorize(policy: "SuperAdminPolicy")]
    public IActionResult GetSuperAdminPolicy()
    {
        return Ok("Super admin policy endpoint");
    }


    #endregion


}