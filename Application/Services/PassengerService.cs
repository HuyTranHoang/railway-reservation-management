namespace Application.Services;

public class PassengerService : IPassengerService
{
    private readonly IMapper _mapper;
    private readonly IPassengerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public PassengerService(IPassengerRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedList<PassengerDto>> GetAllDtoAsync(QueryParams queryParams)
    {
        var query = await _repository.GetQueryAsync();

        if (!string.IsNullOrEmpty(queryParams.SearchTerm))
            query = query.Where(p => p.FullName.Contains(queryParams.SearchTerm));

        query = queryParams.Sort switch
        {
            "ageAsc" => query.OrderBy(p => p.Age),
            "ageDesc" => query.OrderByDescending(p => p.Age),
            "nameAsc" => query.OrderBy(p => p.FullName),
            "nameDesc" => query.OrderByDescending(p => p.FullName),
            _ => query.OrderBy(p => p.CreatedAt)
        };

        var passengerDtoQuery = query.Select(p => _mapper.Map<PassengerDto>(p));

        return await PagedList<PassengerDto>.CreateAsync(passengerDtoQuery, queryParams.PageNumber,
            queryParams.PageSize);
    }

    public async Task<PassengerDto> GetDtoByIdAsync(int id)
    {
        var passenger = await _repository.GetByIdAsync(id);
        return _mapper.Map<PassengerDto>(passenger);
    }

    public async Task<Passenger> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task AddAsync(Passenger passenger)
    {
        _repository.Add(passenger);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(Passenger passenger)
    {
        var passengerInDb = await _repository.GetByIdAsync(passenger.Id);

        if (passengerInDb == null) throw new NotFoundException(nameof(Passenger), passenger.Id);

        passengerInDb.FullName = passenger.FullName;
        passengerInDb.CardId = passenger.CardId;
        passengerInDb.Age = passenger.Age;
        passengerInDb.Gender = passenger.Gender;
        passengerInDb.Phone = passenger.Phone;
        passengerInDb.Email = passenger.Email;
        passengerInDb.UpdatedAt = DateTime.Now;

        _repository.Update(passengerInDb);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Passenger passenger)
    {
        _repository.Delete(passenger);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(Passenger passenger)
    {
        _repository.SoftDelete(passenger);
        await _unitOfWork.SaveChangesAsync();
    }
}