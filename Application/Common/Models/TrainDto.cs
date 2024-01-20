namespace Application.Common.Models;

public class TrainDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int TrainCompanyId { get; set; }

    public string TrainCompanyName { get; set; }
    
    public string TrainCompanyLogo { get; set; }

    public string Status { get; set; }

    public DateTime CreatedAt { get; set; }
}