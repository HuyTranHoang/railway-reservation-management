namespace Application.Common.Models.QueryParams;

public class DistanceFareQueryParams : QueryParams
{
    public int TrainCompanyId { get; set; }
    public int MinDistance { get; set; }
    public int MaxDistance { get; set; }
    public int Price { get; set; }

}