namespace Application.Common.Models
{
    public class RoundTripDto
    {
        public int Id { get; set; }

        public int TrainCompanyId { get; set; }

        public string TrainCompanyName { get; set; }
        
        public string TrainCompanyLogo { get; set; }

        public double Discount { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}