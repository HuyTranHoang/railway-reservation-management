using Domain.Exceptions;

namespace Application.Services;

public class SeatTypeService : ISeatTypeService
{
    private readonly IMapper _mapper;
    private readonly ISeatTypeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public SeatTypeService(ISeatTypeRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedList<SeatTypeDto>> GetAllDtoAsync(QueryParams queryParams)
    {
        var query = await _repository.GetQueryAsync();

        if (!string.IsNullOrEmpty(queryParams.SearchTerm))
            query = query.Where(st => st.Name.Contains(queryParams.SearchTerm.Trim()));

        query = queryParams.Sort switch
        {
            "nameAsc" => query.OrderBy(st => st.Name),
            "nameDesc" => query.OrderByDescending(st => st.Name),
            "serviceChargeAsc" => query.OrderBy(st => st.ServiceCharge),
            "serviceChargeDesc" => query.OrderByDescending(st => st.ServiceCharge),
            "createdAtDesc" => query.OrderByDescending(st => st.CreatedAt),
            _ => query.OrderBy(st => st.CreatedAt)
        };

        var seatTypeDtoQuery = query.Select(p => _mapper.Map<SeatTypeDto>(p));

        return await PagedList<SeatTypeDto>.CreateAsync(seatTypeDtoQuery, queryParams.PageNumber,
            queryParams.PageSize);
    }

    public async Task<SeatTypeDto> GetDtoByIdAsync(int id)
    {
        var seatType = await _repository.GetByIdAsync(id);
        return _mapper.Map<SeatTypeDto>(seatType);
    }

    public async Task<SeatType> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task AddAsync(SeatType seatType)
    {
        await _repository.Add(seatType);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(SeatType seatType)
    {
        var seatTypeInDb = await _repository.GetByIdAsync(seatType.Id);

        if (seatTypeInDb == null) throw new NotFoundException(nameof(Passenger), seatType.Id);

        seatTypeInDb.Name = seatType.Name;
        seatTypeInDb.ServiceCharge = seatType.ServiceCharge;
        seatTypeInDb.Description = seatType.Description;
        seatTypeInDb.Status = seatType.Status;
        seatTypeInDb.UpdatedAt = DateTime.Now;

        await _repository.Update(seatTypeInDb);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(SeatType seatType)
    {
        await _repository.Delete(seatType);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(SeatType seatType)
    {
        await _repository.SoftDelete(seatType);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<List<SeatTypeDto>> GetAllDtoNoPagingAsync()
    {
        var seatType = await _repository.GetAllNoPagingAsync();
        return _mapper.Map<List<SeatTypeDto>>(seatType);
    }

    public async Task<SeatTypeDto> GetDtoByNameAsync(string name)
    {
        var seatType = await _repository.GetSeatTypeByNameAsync(name);
        var seatTypeDto = _mapper.Map<SeatTypeDto>(seatType);

        return seatTypeDto;
    }
}
