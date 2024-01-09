namespace Application.Common.Models;

public class CarriageTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double ServiceCharge { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}