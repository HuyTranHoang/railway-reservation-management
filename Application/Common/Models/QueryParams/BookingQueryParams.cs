namespace Application.Common.Models.QueryParams
{
    public class BookingQueryParams : QueryParams
    {
        public int DepartureStationId { get; set; }
        public int ArrivalStationId { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
    }
}