namespace Application.Common.Models;

public class ApplicationUserDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsLocked { get; set; }

    public DateTime CreatedAt { get; set; }
    public IEnumerable<string> Roles { get; set; }
}
