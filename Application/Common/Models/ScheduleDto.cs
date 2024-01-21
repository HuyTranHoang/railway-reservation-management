namespace Application.Common.Models;

public class ScheduleDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int TrainId { get; set; }

    public string TrainName { get; set; }

    public string TrainCompanyName { get; set; }

    public string TrainCompanyLogo { get; set; }

    public int DepartureStationId { get; set; }

    public string DepartureStationName { get; set; }

    public string DepartureStationAddress { get; set; }

    public int ArrivalStationId { get; set; }

    public string ArrivalStationName { get; set; }
    
    public string ArrivalStationAddress { get; set; }

    public DateTime DepartureTime { get; set; }

    public DateTime ArrivalTime { get; set; }

    public int Duration { get; set; }

    public double Price { get; set; }

    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<ScheduleCarriageType> ScheduleCarriageTypes { get; set; }
}

public class ScheduleCarriageType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double ServiceCharge { get; set; }
}