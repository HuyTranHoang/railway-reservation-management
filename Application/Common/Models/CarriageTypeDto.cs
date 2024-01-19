namespace Application.Common.Models;

public class CarriageTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int NumberOfCompartments { get; set; }
    public int NumberOfSeats { get; set; }
    public int NumberOfSeatTypes { get; set;}
    public double ServiceCharge { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}