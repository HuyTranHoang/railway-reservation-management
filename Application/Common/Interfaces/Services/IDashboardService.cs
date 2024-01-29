namespace Application.Common.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<int> GetTicketCountTodayAsync();
        Task<double> GetTicketPriceTodayAsync();
        Task<double> GetTicketPriceCancelTodayAsync();
        Task<int> GetUserCountTodayAsync();

    }
}