
namespace Application.Common.Models;

public class CarriageDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int TrainId { get; set; }
    
    public string TrainName { get; set; }

    public int NumberOfCompartment { get; set; }

    // public double ServiceCharge { get; set; }

    public string Status { get; set; }
}
