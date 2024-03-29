namespace Application.Common.Interfaces.Services
{
    public interface IDailyCashTransactionService : IService<DailyCashTransaction>
    {
        Task<PagedList<DailyCashTransactionDto>> GetAllDtoAsync(QueryParams queryParams);
        Task<bool> DoWork();
        Task<DailyCashTransactionDto> GetDtoByIdAsync(int id);
        Task<List<DailyCashTransactionDto>> GetAllDtoNoPagingAsync();
        Task<List<DailyCashTransactionDto>> GetAllDtoByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}