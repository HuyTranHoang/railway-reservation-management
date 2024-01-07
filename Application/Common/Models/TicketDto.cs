namespace Application.Common.Models
{
    public class TicketDto
    {
        public int Id { get; set; }
        
        public int PassengerId { get; set; }

        public string PassengerName { get; set; }

        public int TrainId { get; set; }

        public string TrainName { get; set; }

        public int CarriageIdId { get; set; }

        public string CarriageName { get; set; }


        public int DistanceFareId { get; set; }

        public int SeatId { get; set; }

        public string SeatName { get; set; }

        public int ScheduleId { get; set; }

        public string ScheduleName { get; set; }

        public int PaymentId { get; set; }

        public string Status { get; set; }


    }
}