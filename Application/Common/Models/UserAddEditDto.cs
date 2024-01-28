using System.ComponentModel.DataAnnotations;

namespace Application.Common.Models;

public class UserAddEditDto
{
    public string Id { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Roles { get; set; }
}