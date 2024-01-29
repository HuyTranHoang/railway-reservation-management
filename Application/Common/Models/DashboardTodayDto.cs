namespace Application.Common.Models
{
    public class DashboardTodayDto
    {
        public int UserRegistered { get; set; }
        public int TicketSold { get; set; }
        public double TotalRevenue { get; set; }
        public double RefundAmount { get; set; }
    }
}