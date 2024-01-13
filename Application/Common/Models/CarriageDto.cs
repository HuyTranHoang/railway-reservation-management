
namespace Application.Common.Models;

public class CarriageDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int TrainId { get; set; }

    public string TrainName { get; set; }

    public int CarriageTypeId { get; set; }
    public string CarriageTypeName { get; set; }

    public int NumberOfCompartments { get; set; }

    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
