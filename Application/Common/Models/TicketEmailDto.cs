namespace Application.Common.Models;

public class TicketEmailDto
{
    public string DepartureStationName { get; set; }
    public string ArrivalStationName { get; set; }
    public string TicketCode { get; set; }
    public string DepartureDate { get; set; }
    public string DepartureTime { get; set; }
    public string TrainName { get; set; }
    public string PassengerName { get; set; }
    public string PassengerCardId { get; set; }
    public string CarriageTypeName { get; set; }
    public string CompartmentName { get; set; }
    public string SeatName { get; set; }
    public double Price { get; set; }
}
