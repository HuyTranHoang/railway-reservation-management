using Domain.Exceptions;

namespace Application.Services;

public class CancellationRuleService : ICancellationRuleService
{
    private readonly ICancellationRuleRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CancellationRuleService(ICancellationRuleRepository repository,
        IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CancellationRule> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task AddAsync(CancellationRule cancellationRule)
    {
        _repository.Add(cancellationRule);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(CancellationRule cancellationRule)
    {
        var cancellationRuleInDb = await _repository.GetByIdAsync(cancellationRule.Id);

        if (cancellationRuleInDb == null)
            throw new NotFoundException(nameof(CancellationRule), cancellationRule.Id);

        cancellationRuleInDb.DepartureDateDifference = cancellationRule.DepartureDateDifference;
        cancellationRuleInDb.Fee = cancellationRule.Fee;
        cancellationRuleInDb.Status = cancellationRule.Status;
        cancellationRuleInDb.CreatedAt = cancellationRule.CreatedAt;

        _repository.Update(cancellationRuleInDb);

        await _unitOfWork.SaveChangesAsync();

    }

    public async Task DeleteAsync(CancellationRule cancellationRule)
    {
        _repository.Delete(cancellationRule);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(CancellationRule cancellationRule)
    {
        _repository.SoftDelete(cancellationRule);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PagedList<CancellationRuleDto>> GetAllDtoAsync(QueryParams queryParams)
    {
        var query = await _repository.GetQueryAsync();

        if (!string.IsNullOrEmpty(queryParams.SearchTerm))
            query = query.Where(cr => cr.Fee.ToString().Contains(queryParams.SearchTerm.Trim()) ||
                                       cr.DepartureDateDifference.ToString().Contains(queryParams.SearchTerm.Trim()));

        query = queryParams.Sort switch
        {
            "feeAsc" => query.OrderBy(cr => cr.Fee),
            "feeDesc" => query.OrderByDescending(cr => cr.Fee),
            "departureDateDifferenceAsc" => query.OrderBy(cr => cr.DepartureDateDifference),
            "departureDateDifferenceDesc" => query.OrderByDescending(cr => cr.DepartureDateDifference),
            "statusAsc" => query.OrderBy(cr => cr.Status),
            "statusDesc" => query.OrderByDescending(cr => cr.Status),
            "createdAtDesc" => query.OrderByDescending(cr => cr.CreatedAt),
            _ => query.OrderBy(cr => cr.CreatedAt)
        };

        var cancellationRuleDtoQuery = query.Select(p => _mapper.Map<CancellationRuleDto>(p));

        return await PagedList<CancellationRuleDto>.CreateAsync(cancellationRuleDtoQuery, queryParams.PageNumber,
            queryParams.PageSize);
    }

    public async Task<CancellationRuleDto> GetDtoByIdAsync(int id)
    {
        var cancellationRule = await _repository.GetByIdAsync(id);
        return _mapper.Map<CancellationRuleDto>(cancellationRule);
    }

    public async Task<List<CancellationRuleDto>> GetAllDtoNoPagingAsync()
    {
        var cancellationRules = await _repository.GetAllNoPagingAsync();
        return _mapper.Map<List<CancellationRuleDto>>(cancellationRules);
    }
}