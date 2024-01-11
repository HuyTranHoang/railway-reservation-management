using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Controllers;

public class UsersController : BaseApiController
{
    private readonly UserManager<ApplicationUser> _userManager;
    public UsersController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApplicationUserDto>>> GetUsers([FromQuery] QueryParams queryParams)
    {
        var usersDto = _userManager.Users
            .Select(u => new ApplicationUserDto
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber
            })
            .ToList();

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
            PhoneNumber = user.PhoneNumber
        };

        return Ok(userDto);
    }
}
