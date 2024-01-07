using Application.Common.Models.Authentication;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Controllers;

public class AccountController : BaseApiController
{
    private readonly JwtService _jwtService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(JwtService jwtService,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        _jwtService = jwtService;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.UserName);

        if (user == null) return Unauthorized("Invalid username or password");

        if (user.EmailConfirmed == false) return Unauthorized("Email not confirmed");

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded) return Unauthorized("Invalid username or password");

        return CreateApplicationUserDto(user);
    }


    #region Private Helper Methods
    private UserDto CreateApplicationUserDto(ApplicationUser user)
    {
        return new UserDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Jwt = _jwtService.GenerateJwtToken(user)
        };
    }
    #endregion
}