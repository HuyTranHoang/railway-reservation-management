using Domain.Exceptions;

namespace Application.Services;

public class CancellationRuleService : ICancellationRuleService
{
    private readonly ICancellationRuleRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CancellationRuleService(ICancellationRuleRepository repository,
        IUnitOfWork unitOfWork ,IMapper mapper)
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

        query = queryParams.Sort switch
        {
            "feeAsc" => query.OrderBy(cr => cr.Fee),
            "feeDesc" => query.OrderByDescending(cr => cr.Fee),
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
}