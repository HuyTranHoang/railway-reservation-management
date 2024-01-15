namespace Application.Common.Models.QueryParams
{
    public class BookingQueryParams : QueryParams
    {
        public int DepartureStationId { get; set; }
        public int ArrivalStationId { get; set; }
        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
    }
}