using System.ComponentModel.DataAnnotations;

namespace Application.Common.Models.Authentication;

public class LoginWithExternalDto
{
    [Required] public string AccessToken { get; set; }

    [Required] public string UserId { get; set; }

    [Required] public string Provider { get; set; }
}