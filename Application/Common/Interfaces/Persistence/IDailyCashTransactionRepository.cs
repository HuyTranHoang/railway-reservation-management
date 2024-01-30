namespace Application.Common.Interfaces.Persistence
{
    public interface IDailyCashTransactionRepository : IRepository<DailyCashTransaction>
    {
        Task SaveDailyCashTransaction(double totalReceived, double totalRefunded);
        Task<List<DailyCashTransaction>> GetAllNoPagingAsync();
        Task<List<DailyCashTransaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}