namespace Application.Common.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<int> GetTicketCountTodayAsync();
        Task<double> GetTicketPriceTodayAsync();
        Task<double> GetTicketPriceCancelTodayAsync();
        Task<int> GetUserCountTodayAsync();
        Task<double[]> GetTicketPriceSumLast7DaysAsync();
        Task<double[]> GetTicketPriceCancelSumLast7DaysAsync();

    }
}