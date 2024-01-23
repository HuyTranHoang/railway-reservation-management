namespace Application.Common.Models
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public string AspNetUserId { get; set; }
        public string AspNetUserFullName { get; set; }
        public string AspNetUserEmail { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Passenger> Passengers { get; set; }
        public Payment Payments { get; set; }
        public List<Ticket> Tickets { get; set; }
        // public Ticket Ticket { get; set; }
        public int CarriageId { get; set; }
        public int SeatId { get; set; }
    }
}