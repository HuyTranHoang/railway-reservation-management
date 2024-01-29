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

        [HttpGet("last7Days")]
        public async Task<IActionResult> GetDashboardLast7Days()
        {
            try
            {
                double[] ticketPriceSum = await _dashboardService.GetTicketPriceSumLast7DaysAsync();
                double[] ticketPriceCancelSum = await _dashboardService.GetTicketPriceCancelSumLast7DaysAsync();

                List<object> results = new List<object>();

                for (int i = 0; i < ticketPriceSum.Length; i++)
                {
                    var result = new
                    {
                        ticketPriceSum = ticketPriceSum[i],
                        ticketPriceCancelSum = ticketPriceCancelSum[i]
                    };
                    results.Add(result);
                }


                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}