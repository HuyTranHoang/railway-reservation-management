namespace Application.Common.Models
{
    public class UpcomingScheduleDto
    {
        public int ScheduleId { get; set; }
        public string ScheduleName { get; set; }
        public DateTime DepartureTime { get; set; }
        public int TotalSeats { get; set; }
        public int SeatsBooked { get; set; }
        public int SeatsAvailable => TotalSeats - SeatsBooked;
    }
}