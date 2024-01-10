namespace Application.Common.Models.QueryParams
{
    public class TicketQueryParams : QueryParams
    {
        public int PassengerId { get; set; }
        public int TrainId { get; set; }
        public int DistanceFareId { get; set; }
        public int CarriageId { get; set; }
        public int SeatId { get; set; }
        public int ScheduleId { get; set; }
        public int PaymentId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}