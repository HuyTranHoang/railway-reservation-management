namespace Application.Common.Models.QueryParams;

public class DistanceFareQueryParams : QueryParams
{
    public int TrainCompanyId { get; set; }
    public int Distance { get; set; }

}