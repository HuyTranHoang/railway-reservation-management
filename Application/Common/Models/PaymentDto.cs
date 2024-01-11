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
    }
}