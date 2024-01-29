namespace WebApi.Controllers
{
    public class DashboardController : BaseApiController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("dashboardDataToday")]
        public async Task<IActionResult> GetDashboardData()
        {
            var userRegisteredCount = await _dashboardService.GetUserCountTodayAsync();
            var ticketSoldCount = await _dashboardService.GetTicketCountTodayAsync();
            var totalRevenue = await _dashboardService.GetTicketPriceTodayAsync();
            var refundAmount = await _dashboardService.GetTicketPriceCancelTodayAsync();

            var dashboardDataToday = new DashboardTodayDto
            {
                UserRegistered = userRegisteredCount,
                TicketSold = ticketSoldCount,
                TotalRevenue = totalRevenue,
                RefundAmount = refundAmount
            };

            return Ok(dashboardDataToday);
        }
    }
}