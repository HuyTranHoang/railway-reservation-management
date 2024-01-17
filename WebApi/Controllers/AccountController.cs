﻿using System.Globalization;
using System.Security.Claims;
using System.Text;
using Application.Common.Models.Authentication;
using Application.Services;
using Domain.Constants;
using Domain.Exceptions;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers;

public class AccountController : BaseApiController
{
    private readonly JwtService _jwtService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly EmailService _emailService;
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly HttpClient _facebookHttpClient;

    public AccountController(JwtService jwtService,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        EmailService emailService,
        IConfiguration config,
        IWebHostEnvironment hostingEnvironment)
    {
        _jwtService = jwtService;
        _signInManager = signInManager;
        _userManager = userManager;
        _emailService = emailService;
        _config = config;
        _hostingEnvironment = hostingEnvironment;
        _facebookHttpClient = new HttpClient
        {
            BaseAddress = new Uri("https://graph.facebook.com")
        };
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
                    new
                    {
                        title = "Account Created",
                        message = "Please check your email for confirmation link and login"
                    }));
            }

            return BadRequest(new ErrorResponse(400, "Failed to send email. Please contact support"));

        }
        catch (Exception)
        {
            return BadRequest(new ErrorResponse(400, "Failed to send email. Please contact support"));
        }
    }

    [HttpPost("register-with-third-party")]
    public async Task<ActionResult<UserDto>> RegisterWithThirdParty(RegisterWithExternalDto model)
    {
        if (model.Provider.Equals(SD.Facebook))
        {
            try
            {
                if (!FacebookValidatedAsync(model.AccessToken, model.UserId).GetAwaiter().GetResult())
                {
                    return Unauthorized(new ErrorResponse(401, "Unable to register with facebook"));
                }
            }
            catch (Exception)
            {
                return Unauthorized(new ErrorResponse(401, "Unable to register with facebook"));
            }
        } else if (model.Provider.Equals(SD.Google))
        {

        }
        else
        {
            return BadRequest(new ErrorResponse(400, "Invalid provider"));
        }

        var user = await _userManager.FindByNameAsync(model.UserId);
        if (user != null)
            return BadRequest(new ErrorResponse(400,
                $"You have already registered. Please login with your {model.Provider}"));

        var userToAdd = new ApplicationUser
        {
            FirstName = model.FirstName.ToLower(),
            LastName = model.LastName.ToLower(),
            Email = model.Email.ToLower(),
            UserName = model.UserId.ToLower(),
            Provider = model.Provider,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(userToAdd);

        if (!result.Succeeded) return BadRequest(result.Errors);

        return CreateApplicationUserDto(userToAdd);
    }

    [HttpPost("login-with-third-party")]
    public async Task<ActionResult<UserDto>> LoginWithThirdParty(LoginWithExternalDto model)
    {
        if (model.Provider.Equals(SD.Facebook))
        {
            try
            {
                if (!FacebookValidatedAsync(model.AccessToken, model.UserId).GetAwaiter().GetResult())
                {
                    return Unauthorized(new ErrorResponse(401, "Unable to login with facebook"));
                }
            }
            catch (Exception)
            {
                return Unauthorized(new ErrorResponse(401, "Unable to login with facebook"));
            }
            
        } else if (model.Provider.Equals(SD.Google))
        {

        }
        else
        {
            return BadRequest(new ErrorResponse(400, "Invalid provider"));
        }

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == model.UserId && x.Provider == model.Provider);
        if (user == null)
        {
            // Người dùng chưa đăng ký, trả về thông báo và yêu cầu chuyển hướng đến trang đăng ký
            return Ok(new { IsUserRegistered = false });
        }

        // Người dùng đã đăng ký, tiến hành đăng nhập và trả về thông tin người dùng
        var userDto = CreateApplicationUserDto(user);
        return Ok(new { IsUserRegistered = true, User = userDto });
    }

    [HttpPut("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto confirmEmailDto)
    {
        var user = await _userManager.FindByEmailAsync(confirmEmailDto.Email);
        if (user == null)
            return Unauthorized(new ErrorResponse(401, "This email has not been registered"));

        if (user.EmailConfirmed)
            return BadRequest(new ErrorResponse(400, "This email has already been confirmed"));

        try
        {
            var decodedTokenBytes = WebEncoders.Base64UrlDecode(confirmEmailDto.Token);
            var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
            {
                return Ok(new JsonResult(new { title = "Email Confirmed", message = "You can now login" }));
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
                    new
                    {
                        title = "Email Confirmation Link Sent",
                        message = "Please check your email for confirmation link and login"
                    }));
            }

            return BadRequest(new ErrorResponse(400, "Failed to send email. Please contact support"));

        }
        catch (Exception)
        {
            return BadRequest(new ErrorResponse(400, "Failed to send email. Please contact support"));
        }
    }

    [HttpPost("forgot-password/{email}")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
            return BadRequest(new ErrorResponse(400, "Invalid email address"));

        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            return Unauthorized(new ErrorResponse(401, "This email has not been registered"));

        try
        {
            if (await SendForgotPasswordEmailAsync(user))
            {
                return Ok(new JsonResult(
                    new
                    {
                        title = "Reset Password Link Sent",
                        message = "Please check your email for reset password link"
                    }));
            }
            return BadRequest(new ErrorResponse(400, "Failed to send email. Please contact support"));

        }
        catch (Exception)
        {
            return BadRequest(new ErrorResponse(400, "Failed to send email. Please contact support"));
        }
    }

    [HttpPut("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
        if (user == null)
            return Unauthorized(new ErrorResponse(401, "This email has not been registered"));

        try
        {
            var decodedTokenBytes = WebEncoders.Base64UrlDecode(resetPasswordDto.Token);
            var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

            var result = await _userManager.ResetPasswordAsync(user, decodedToken, resetPasswordDto.NewPassword);

            if (result.Succeeded)
            {
                return Ok(new JsonResult(new { title = "Password Reset Success", message = "You can now login" }));
            }

            return BadRequest(new ErrorResponse(400, "Invalid token. Please try again"));
        }
        catch (Exception)
        {
            return BadRequest(new ErrorResponse(400, "Invalid token. Please try again"));
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

        var templatePath = Path.Combine(_hostingEnvironment.WebRootPath, "confirm_email_template.html");

        using var reader = new StreamReader(templatePath);
        var emailTemplate = await reader.ReadToEndAsync();

        emailTemplate = emailTemplate.Replace("{FirstName}", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(user.FirstName));
        emailTemplate = emailTemplate.Replace("{LastName}", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(user.LastName));

        emailTemplate = emailTemplate.Replace("{ApplicationName}", _config["Email:ApplicationName"]);
        emailTemplate = emailTemplate.Replace("{ConfirmationLink}", url);

        var emailSend = new EmailSendDto(user.Email, "Confirm your email", emailTemplate);

        return await _emailService.SendEmailAsync(emailSend);
    }

    private async Task<bool> SendForgotPasswordEmailAsync(ApplicationUser user)
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var url = $"{_config["JWT:ClientUrl"]}/{_config["Email:ResetPasswordPath"]}?email={user.Email}&token={token}";

        var templatePath = Path.Combine(_hostingEnvironment.WebRootPath, "forgot_password_template.html");

        using var reader = new StreamReader(templatePath);
        var emailTemplate = await reader.ReadToEndAsync();

        emailTemplate = emailTemplate.Replace("{FirstName}", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(user.FirstName));
        emailTemplate = emailTemplate.Replace("{LastName}", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(user.LastName));

        emailTemplate = emailTemplate.Replace("{ApplicationName}", _config["Email:ApplicationName"]);
        emailTemplate = emailTemplate.Replace("{ResetPasswordLink}", url);

        var emailSend = new EmailSendDto(user.Email, "Reset your password", emailTemplate);

        return await _emailService.SendEmailAsync(emailSend);
    }

    private async Task<bool> FacebookValidatedAsync(string accessToken, string userId)
    {
        var facebookKeys = _config["Facebook:AppId"] + "|" + _config["Facebook:AppSecret"];
        var fbResult = await _facebookHttpClient.GetFromJsonAsync<FacebookResultDto>(
            $"/debug_token?input_token={accessToken}&access_token={facebookKeys}");

        if (fbResult == null || fbResult.Data.Is_Valid == false || !fbResult.Data.User_Id.Equals(userId))
            return false;

        return true;
    }
    #endregion
}