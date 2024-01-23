namespace Application.Common.Models
{
    public class CancellationDto
    {
        public int Id { get; set; }
        
        public int TicketId { get; set; }

        public string TicketCode { get; set; }

        public int CancellationRuleId { get; set; }

        public double CancellationRuleFee { get; set; }

        public string Reason { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}