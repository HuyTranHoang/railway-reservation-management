namespace Application.Common.Models
{
    public class DailyCashTransactionDto
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public double TotalReceived { get; set; }

        public double TotalRefunded { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}