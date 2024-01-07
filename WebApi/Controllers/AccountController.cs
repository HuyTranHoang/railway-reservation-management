using System.Security.Claims;
using System.Text;
using Application.Common.Models.Authentication;
using Application.Services;
using Domain.Exceptions;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace WebApi.Controllers;

public class AccountController : BaseApiController
{
    private readonly JwtService _jwtService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly EmailService _emailService;
    private readonly IConfiguration _config;

    public AccountController(JwtService jwtService,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        EmailService emailService,
        IConfiguration config)
    {
        _jwtService = jwtService;
        _signInManager = signInManager;
        _userManager = userManager;
        _emailService = emailService;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.UserName);

        if (user == null) return Unauthorized(new ErrorResponse(401, "Invalid username or password"));

        if (user.EmailConfirmed == false)
            return Unauthorized(new ErrorResponse(401, "Please confirm your email to login"));

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded) return Unauthorized(new ErrorResponse(401, "Invalid username or password"));

        return CreateApplicationUserDto(user);
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await CheckEmailExists(registerDto.Email))
        {
            return BadRequest(new ValidateInputError(400, "Email already exists"));
        }

        var user = new ApplicationUser
        {
            FirstName = registerDto.FirstName.ToLower(),
            LastName = registerDto.LastName.ToLower(),
            Email = registerDto.Email.ToLower(),
            UserName = registerDto.Email.ToLower()
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);

        try
        {
            if (await SendConfirmEmailAsync(user))
            {
                return Ok(new JsonResult(
                    new { title = "Account Created",
                        message = "Please check your email for confirmation link and login" }));
            }

            return BadRequest(new ErrorResponse(400, "Failed to send email. Please contact support"));

        }
        catch (Exception)
        {
            return BadRequest(new ErrorResponse(400, "Failed to send email. Please contact support"));
        }
    }

    [HttpPut("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto confirmEmail)
    {
        var user = await _userManager.FindByEmailAsync(confirmEmail.Email);
        if (user == null)
            return Unauthorized(new ErrorResponse(401, "This email has not been registered"));

        if (user.EmailConfirmed)
            return BadRequest(new ErrorResponse(400, "This email has already been confirmed"));

        try
        {
            var decodedTokenBytes = WebEncoders.Base64UrlDecode(confirmEmail.Token);
            var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
            {
                return Ok(new JsonResult(new { title = "Email Confirmed",message = "You can now login" }));
            }

            return BadRequest(new ErrorResponse(400, "Invalid token. Please try again"));
        }
        catch (Exception)
        {
            return BadRequest(new ErrorResponse(400, "Invalid token. Please try again"));
        }
    }

    [HttpPost("resend-email-confirmation-link/{email}")]
    public async Task<IActionResult> ResendEmailConfirmationLink(string email)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
            return BadRequest(new ErrorResponse(400, "Invalid email address"));

        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            return Unauthorized(new ErrorResponse(401, "This email has not been registered"));

        if (user.EmailConfirmed)
            return BadRequest(new ErrorResponse(400, "This email has already been confirmed"));

        try
        {
            if (await SendConfirmEmailAsync(user))
            {
                return Ok(new JsonResult(
                    new { title = "Email Confirmation Link Sent",
                        message = "Please check your email for confirmation link and login" }));
            }

            return BadRequest(new ErrorResponse(400, "Failed to send email. Please contact support"));

        }
        catch (Exception)
        {
            return BadRequest(new ErrorResponse(400, "Failed to send email. Please contact support"));
        }
    }

    [Authorize]
    [HttpGet("refresh-user-token")]
    public async Task<ActionResult<UserDto>> RefreshUserToken()
    {
        var user = await _userManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Email));

        if (user == null) return Unauthorized("Invalid username or password");

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

    private async Task<bool> CheckEmailExists(string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    private async Task<bool> SendConfirmEmailAsync(ApplicationUser user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var url = $"{_config["JWT:ClientUrl"]}/{_config["Email:ConfirmEmailPath"]}?email={user.Email}&token={token}";

        var body = $"<h3>Hello: {user.FirstName} {user.LastName}" +
                   $"<h5>Welcome to { _config["Email:ApplicationName"] }</h1>" +
                   $"<p>Please confirm your email by <a href='{url}'>clicking here</a></p>" +
                   $"<p> Thank your, </p>" +
                   $"<br>{_config["Email:ApplicationName"]} Team";

        var emailSend = new EmailSendDto(user.Email, "Confirm your email", body);

        return await _emailService.SendEmailAsync(emailSend);

    }
    #endregion
}