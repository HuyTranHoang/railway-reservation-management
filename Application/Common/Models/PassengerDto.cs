﻿namespace Application.Common.Models;

public class PassengerDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string CardId { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
}