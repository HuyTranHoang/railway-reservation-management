using System.ComponentModel.DataAnnotations;

namespace Application.Common.Models.Authentication;

public class ConfirmEmailDto
{
    [Required]
    public string Token { get; set; }
    [Required]
    [RegularExpression(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$", ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
}