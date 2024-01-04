namespace Application.Common.Models;

public class CancellationRuleDto
{
    public int Id { get; set; }
    public int DepartureDateDifference { get; set; }
    public double Fee { get; set; }
    public string Status { get; set; }
}