using Domain.Exceptions;

namespace Application.Services;

public class TrainCompanyService : ITrainCompanyService
{
    private readonly IMapper _mapper;
    private readonly ITrainCompanyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public TrainCompanyService(ITrainCompanyRepository repositoy, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repositoy;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedList<TrainCompanyDto>> GetAllDtoAsync(QueryParams queryParams)
    {
        var query = await _repository.GetQueryAsync();

        if (!string.IsNullOrEmpty(queryParams.SearchTerm))
        {
            query = query.Where(t => t.Name.Contains(queryParams.SearchTerm.Trim()));
        }

        query = queryParams.Sort switch
        {
            "nameAsc" => query.OrderBy(p => p.Name),
            "nameDesc" => query.OrderByDescending(p => p.Name),
            "createdAtDesc" => query.OrderByDescending(p => p.CreatedAt),
            _ => query.OrderBy(p => p.CreatedAt)
        };

        var trainCompaniesDtoQuery = query.Select(p => _mapper.Map<TrainCompanyDto>(p));

        return await PagedList<TrainCompanyDto>
            .CreateAsync(trainCompaniesDtoQuery, queryParams.PageNumber, queryParams.PageSize);
    }

    public async Task<List<TrainCompanyDto>> GetAllDtoNoPagingAsync()
    {
        var trainCompanies = await _repository.GetAllAsync();
        return _mapper.Map<List<TrainCompanyDto>>(trainCompanies);
    }

    public async Task<TrainCompany> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<TrainCompanyDto> GetDtoByIdAsync(int id)
    {
        var trainCompany = await _repository.GetByIdAsync(id);
        return _mapper.Map<TrainCompanyDto>(trainCompany);
    }

    public async Task AddAsync(TrainCompany trainCompany)
    {

        if (NameExists(_repository, trainCompany.Name))
        {
            throw new BadRequestException(400, "Name already exists");
        }

        await _repository.Add(trainCompany);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(TrainCompany trainCompany)
    {
        var trainCompanyInDb = await _repository.GetByIdAsync(trainCompany.Id);

        if (trainCompanyInDb == null) throw new NotFoundException(nameof(Passenger), trainCompany.Id);

        trainCompanyInDb.Name = trainCompany.Name;
        trainCompanyInDb.Logo = trainCompany.Logo;
        trainCompanyInDb.Status = trainCompany.Status;
        trainCompanyInDb.UpdatedAt = DateTime.Now;

        await _repository.Update(trainCompanyInDb);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(TrainCompany trainCompany)
    {
        await _repository.Delete(trainCompany);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(TrainCompany trainCompany)
    {
        await _repository.SoftDelete(trainCompany);
        await _unitOfWork.SaveChangesAsync();
    }

    #region Private Helper Methods

    private static bool NameExists(ITrainCompanyRepository repository, string name)
    {
        return repository.GetQueryAsync().Result.Any(t => t.Name == name);
    }

    #endregion
}