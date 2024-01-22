using Domain.Constants;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
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
        var query = _userManager.Users
            .Where(u => u.UserName != SD.SuperAdminEmail);

        if (!string.IsNullOrEmpty(queryParams.SearchTerm))
            query = query.Where(ct => ct.FirstName.Contains(queryParams.SearchTerm.Trim()) ||
                                      ct.LastName.Contains(queryParams.SearchTerm.Trim()) ||
                                      ct.Email.Contains(queryParams.SearchTerm.Trim()) ||
                                      ct.PhoneNumber.Contains(queryParams.SearchTerm.Trim()));

        query = queryParams.Sort switch
        {
            "firstNameAsc" => query.OrderBy(u => u.FirstName),
            "firstNameDesc" => query.OrderByDescending(u => u.FirstName),
            "lastNameAsc" => query.OrderBy(u => u.LastName),
            "lastNameDesc" => query.OrderByDescending(u => u.LastName),
            "emailAsc" => query.OrderBy(u => u.Email),
            "emailDesc" => query.OrderByDescending(u => u.Email),
            "phoneNumberAsc" => query.OrderBy(u => u.PhoneNumber),
            "phoneNumberDesc" => query.OrderByDescending(u => u.PhoneNumber),
            "isLockedAsc" => query.OrderBy(u => u.LockoutEnd),
            "isLockedDesc" => query.OrderByDescending(u => u.LockoutEnd),
            "createdAtDesc" => query.OrderByDescending(u => u.CreatedAt),
            _ => query.OrderBy(u => u.CreatedAt)
        };

        var usersDtoPagedList = await PagedList<ApplicationUser>.CreateAsync(query, queryParams.PageNumber,
            queryParams.PageSize);

        var usersDto = usersDtoPagedList.Select(user => new ApplicationUserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            CreatedAt = user.CreatedAt,
            IsLocked = _userManager.IsLockedOutAsync(user).Result,
            Roles = _userManager.GetRolesAsync(user).Result
        }).ToList();

        var paginationHeader = new PaginationHeader(queryParams.PageNumber, queryParams.PageSize,
            usersDtoPagedList.TotalCount, usersDtoPagedList.TotalPages);

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
            CreatedAt = user.CreatedAt,
            IsLocked = _userManager.IsLockedOutAsync(user).Result,
            Roles = _userManager.GetRolesAsync(user).Result
        };

        return Ok(userDto);
    }

    [HttpGet("get-user-by-id/{id}")]
    public async Task<ActionResult<UserAddEditDto>> GetUserById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null) return BadRequest(new ValidateInputError(400, "User with this id does not exist."));

        var userDto = new UserAddEditDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            Roles = string.Join(",", _userManager.GetRolesAsync(user).Result)
        };

        return Ok(userDto);
    }

    [HttpPost("add-edit-user")]
    public async Task<IActionResult> AddEditUser(UserAddEditDto userDto)
    {
        ApplicationUser user;

        if (string.IsNullOrEmpty(userDto.Id))
        {
            if (string.IsNullOrEmpty(userDto.Password) || userDto.Password.Length < 6)
            {
                ModelState.AddModelError("errors", "Password is required and must be at least 6 characters.");
                return BadRequest(ModelState);
            }
        }
        else
        {

        }

        return Ok();
    }

    [Authorize(policy: "SuperAdminOrAdminPolicy")]
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

    [Authorize(policy: "SuperAdminOrAdminPolicy")]
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

    [HttpGet("get-application-roles")]
    public async Task<ActionResult<IEnumerable<string>>> GetApplicationRoles()
    {
        var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

        return Ok(roles);
    }

    private bool IsAdminUserId(string id)
    {
        return _userManager.FindByIdAsync(id).GetAwaiter().GetResult().Email == SD.SuperAdminEmail;
    }
}
