using Domain.Exceptions;

namespace Application.Services;

public class DistanceFareService : IDistanceFareService
{
    private readonly IMapper _mapper;

    private readonly IDistanceFareRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DistanceFareService(IDistanceFareRepository repositoy, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repositoy;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task AddAsync(DistanceFare distanceFare)
    {
        _repository.Add(distanceFare);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(DistanceFare distanceFare)
    {
        _repository.Delete(distanceFare);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PagedList<DistanceFareDto>> GetAllDtoAsync(DistanceFareQueryParams queryParams)
    {
        var query = await _repository.GetQueryWithTrainCompanyAsync();

        if (queryParams.TrainCompanyId != 0)
        {
            query = query.Where(t => t.TrainCompanyId == queryParams.TrainCompanyId);
        }

        if (!string.IsNullOrEmpty(queryParams.SearchTerm))
            query = query.Where(t => t.TrainCompany.Name.Contains(queryParams.SearchTerm.Trim()));

        if (queryParams.MinDistance != 0)
        {

        }

        query = queryParams.Sort switch
        {
            "trainCompanyNameAsc" => query.OrderBy(t => t.TrainCompany.Name),
            "trainCompanyNameDesc" => query.OrderByDescending(t => t.TrainCompany.Name),
            "distanceAsc" => query.OrderBy(t => t.Distance),
            "distanceDesc" => query.OrderByDescending(t => t.Distance),
            "priceAsc" => query.OrderBy(t => t.Price),
            "priceDesc" => query.OrderByDescending(t => t.Price),
            "createdAtDesc" => query.OrderByDescending(t => t.CreatedAt),
            _ => query.OrderBy(t => t.CreatedAt)
        };


        var distanceFaresDtoQuery = query.Select(t => _mapper.Map<DistanceFareDto>(t));

        return await PagedList<DistanceFareDto>.CreateAsync(distanceFaresDtoQuery, queryParams.PageNumber,
            queryParams.PageSize);
    }

    public async Task<DistanceFare> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<DistanceFareDto> GetDtoByDistanceAsync(int distance)
    {
        var distanceFareDto = await _repository.GetByDistanceAsync(distance);
        return _mapper.Map<DistanceFareDto>(distanceFareDto);
    }

    public async Task<DistanceFareDto> GetDtoByIdAsync(int id)
    {
        var distanceFareDto = await _repository.GetByIdAsync(id);
        return _mapper.Map<DistanceFareDto>(distanceFareDto);
    }

    public async Task SoftDeleteAsync(DistanceFare distanceFare)
    {
        _repository.SoftDelete(distanceFare);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(DistanceFare distanceFare)
    {
        var distanceFareInDb = await _repository.GetByIdAsync(distanceFare.Id);

        if (distanceFareInDb == null) throw new NotFoundException(nameof(DistanceFare), distanceFare.Id);

        distanceFareInDb.TrainCompanyId = distanceFare.TrainCompanyId;
        distanceFareInDb.Distance = distanceFare.Distance;
        distanceFareInDb.Price = distanceFare.Price;
        distanceFareInDb.UpdatedAt = DateTime.Now;

        await _unitOfWork.SaveChangesAsync();
    }
}