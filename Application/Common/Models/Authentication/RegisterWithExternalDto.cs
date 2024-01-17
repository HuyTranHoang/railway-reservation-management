using System.ComponentModel.DataAnnotations;

namespace Application.Common.Models.Authentication;

public class RegisterWithExternalDto
{
    [Required]
    [StringLength(15, MinimumLength = 3, ErrorMessage = "First name must be at least {2}, and maximum {1} characters.")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(15, MinimumLength = 3, ErrorMessage = "Last name must be at least {2}, and maximum {1} characters.")]

    public string LastName { get; set; }

    [Required]
    [RegularExpression(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$", ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }

    [Required]
    public string AccessToken { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public string Provider { get; set; }
}