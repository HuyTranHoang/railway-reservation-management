namespace Application.Common.Models;

public class ScheduleDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int TrainId { get; set; }

    public string TrainName { get; set; }

    public int DepartureStationId { get; set; }

    public string DepartureStationName { get; set; }

    public int ArrivalStationId { get; set; }

    public string ArrivalStationName { get; set; }

    public DateTime DepartureDate { get; set; }

    public DateTime ArrivalDate { get; set; }

    public DateTime DepartureTime { get; set; }

    public int Duration { get; set; }

    public string Status { get; set; }
}
