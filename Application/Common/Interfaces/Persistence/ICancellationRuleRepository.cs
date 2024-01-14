namespace Application.Common.Interfaces.Persistence;

public interface ICancellationRuleRepository : IRepository<CancellationRule>
{
    Task<List<CancellationRule>> GetAllNoPagingAsync();
}