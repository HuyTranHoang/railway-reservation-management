
namespace Application.Common.Models
{
    public class BookingHistoryDto
    {
        public List<TicketDto> UpcomingTrips { get; set; }
        public List<TicketDto> PastTrips { get; set; }
        public List<TicketDto> Cancellations { get; set; }

    }
}