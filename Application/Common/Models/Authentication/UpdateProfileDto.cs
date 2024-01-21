using System.ComponentModel.DataAnnotations;

namespace Application.Common.Models.Authentication;

public class UpdateProfileDto
{
    [Required]
    [StringLength(15, MinimumLength = 3, ErrorMessage = "First name must be at least {2}, and maximum {1} characters.")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(15, MinimumLength = 3, ErrorMessage = "First name must be at least {2}, and maximum {1} characters.")]
    public string LastName { get; set; }

    [RegularExpression(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$",
        ErrorMessage = "Invalid Phone Number")]
    public string PhoneNumber { get; set; }
}