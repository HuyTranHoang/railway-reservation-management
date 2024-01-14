namespace Application.Common.Models.QueryParams
{
    public class SeatQueryParams : QueryParams
    {
        public int SeatTypeId { get; set; }

        public int CompartmentId { get; set; }
    }
}