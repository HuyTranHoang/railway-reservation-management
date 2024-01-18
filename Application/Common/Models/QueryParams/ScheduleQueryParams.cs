namespace Application.Common.Models.QueryParams;

public class ScheduleQueryParams : QueryParams
{
    public int TrainId { get; set; }
    public int DepartureStationId { get; set; }
    public int ArrivalStationId { get; set; }
    public DateTime? DepartureTime { get; set; }
    public DateTime? ArrivalTime { get; set; }

}
