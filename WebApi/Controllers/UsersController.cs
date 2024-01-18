using Domain.Constants;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers;

public class UsersController : BaseApiController
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApplicationUserDto>>> GetUsers([FromQuery] QueryParams queryParams)
    {
        var usersList = await _userManager.Users
            .Where(u => u.UserName != SD.SuperAdminEmail)
            .ToListAsync();

        var usersDto = usersList.Select(user => new ApplicationUserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            IsLocked = _userManager.IsLockedOutAsync(user).Result,
            Roles = _userManager.GetRolesAsync(user).Result
        }).ToList();

        var paginationHeader = new PaginationHeader(queryParams.PageNumber, queryParams.PageSize,
            usersDto.Count, usersDto.Count);

        Response.AddPaginationHeader(paginationHeader);

        return Ok(usersDto);
    }

    [HttpGet("{email}")]
    public async Task<ActionResult<ApplicationUserDto>> GetUser(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null) return BadRequest(new ValidateInputError(400, "User with this email does not exist."));

        var userDto = new ApplicationUserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            IsLocked = _userManager.IsLockedOutAsync(user).Result,
            Roles = _userManager.GetRolesAsync(user).Result
        };

        return Ok(userDto);
    }

    [HttpPut("lock-user/{id}")]
    public async Task<ActionResult> LockUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null) return BadRequest(new ValidateInputError(400, "User with this id does not exist."));

        if (IsAdminUserId(id))
        {
            return BadRequest(new ValidateInputError(400, "Super Admin change is not allowed."));
        }

        await _userManager.SetLockoutEnabledAsync(user, true);
        await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));

        return NoContent();
    }

    [HttpPut("unlock-user/{id}")]
    public async Task<ActionResult> UnlockUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null) return BadRequest(new ValidateInputError(400, "User with this id does not exist."));

        if (IsAdminUserId(id))
        {
            return BadRequest(new ValidateInputError(400, "Super Admin change is not allowed."));
        }

        await _userManager.SetLockoutEnabledAsync(user, false);
        await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);

        return NoContent();
    }

    private bool IsAdminUserId(string id)
    {
        return _userManager.FindByIdAsync(id).GetAwaiter().GetResult().Email == SD.SuperAdminEmail;
    }
}
