﻿namespace Application.Common.Models.Authentication;

public class UserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Jwt { get; set; }
}