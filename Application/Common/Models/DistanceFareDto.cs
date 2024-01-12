
namespace Application.Common.Models;

public class DistanceFareDto
{
    public int Id { get; set; }

    public int TrainCompanyId { get; set; }

    public string TrainCompanyName { get; set; }

    public int Distance { get; set; }

    public double Price { get; set; }

    public DateTime CreatedAt { get; set; }
}