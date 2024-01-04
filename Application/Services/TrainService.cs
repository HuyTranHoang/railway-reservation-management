namespace Application.Services;

public class TrainService : ITrainService
{
    private readonly IMapper _mapper;

    private readonly ITrainRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public TrainService(ITrainRepository repositoy, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repositoy;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task AddAsync(Train train)
    {
        if (NameExists(_repository, train.Name, train.Id))
        {
            throw new BadRequestException(400, "Name already exists");
        }

        _repository.Add(train);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Train train)
    {
        _repository.Delete(train);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PagedList<TrainDto>> GetAllDtoAsync(TrainQueryParams queryParams)
    {
        var query = await _repository.GetQueryWithTrainCompanyAsync();

        if (queryParams.TrainCompanyId != 0)
        {
            query = query.Where(t => t.TrainCompanyId == queryParams.TrainCompanyId);
        }

        if (!string.IsNullOrEmpty(queryParams.SearchTerm))
            query = query.Where(t => t.Name.Contains(queryParams.SearchTerm));

        query = queryParams.Sort switch
        {
            "nameAsc" => query.OrderBy(t => t.Name),
            "nameDesc" => query.OrderByDescending(t => t.Name),
            _ => query.OrderBy(t => t.CreatedAt)
        };


        var trainsDtoQuery = query.Select(t => _mapper.Map<TrainDto>(t));

        return await PagedList<TrainDto>.CreateAsync(trainsDtoQuery, queryParams.PageNumber,
            queryParams.PageSize);
    }

    public async Task<Train> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<TrainDto> GetDtoByIdAsync(int id)
    {
        var trainDto = await _repository.GetByIdAsync(id);
        return _mapper.Map<TrainDto>(trainDto);
    }

    public async Task SoftDeleteAsync(Train train)
    {
        _repository.SoftDelete(train);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(Train train)
    {
        var trainInDb = await _repository.GetByIdAsync(train.Id);

        if (trainInDb == null) throw new NotFoundException(nameof(Train), train.Id);

        if (NameExists(_repository, train.Name, train.Id))
        {
            throw new BadRequestException(400, "Name already exists");
        }

        trainInDb.Name = train.Name;
        trainInDb.TrainCompanyId = train.TrainCompanyId;
        trainInDb.UpdatedAt = DateTime.Now;

        await _unitOfWork.SaveChangesAsync();
    }


    #region Private Helper Methods

    private static bool NameExists(ITrainRepository repository, string name, int trainId)
    {
        return repository.GetQueryAsync().Result.Any(t => t.Name == name && t.Id != trainId);
    }


    #endregion
}