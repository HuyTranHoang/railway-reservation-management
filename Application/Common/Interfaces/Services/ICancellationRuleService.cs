namespace Application.Common.Interfaces.Services;

public interface ICancellationRuleService : IService<CancellationRule>
{
    Task<PagedList<CancellationRuleDto>> GetAllDtoAsync(QueryParams queryParams);
    Task<CancellationRuleDto> GetDtoByIdAsync(int id);
}