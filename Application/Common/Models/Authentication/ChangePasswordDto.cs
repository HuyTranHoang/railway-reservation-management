using System.ComponentModel.DataAnnotations;

namespace Application.Common.Models.Authentication;

public class ChangePasswordDto
{
    [Required]
    [StringLength(15, MinimumLength = 6, ErrorMessage = "Password must be at least {2}, and maximum {1} characters.")]
    public string CurrentPassword { get; set; }

    [Required]
    [StringLength(15, MinimumLength = 6, ErrorMessage = "Password must be at least {2}, and maximum {1} characters.")]
    public string NewPassword { get; set; }
}