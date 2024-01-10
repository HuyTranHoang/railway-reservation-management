namespace Application.Common.Interfaces.Services
{
    public interface IDailyCashTransactionService : IService<DailyCashTransaction>
    {
        Task<PagedList<DailyCashTransactionDto>> GetAllDtoAsync(QueryParams queryParams);
        Task<bool> DoWork();
    }
}