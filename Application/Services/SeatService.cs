using Domain.Exceptions;

namespace Application.Services;

public class SeatService : ISeatService
{
    private readonly ISeatRepository _repository;
    private readonly ICompartmentRepository _compartmentRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public SeatService(ISeatRepository repository,
        ICompartmentRepository compartmentRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _repository = repository;
        _compartmentRepository = compartmentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task AddAsync(Seat seat)
    {
        var compartment = await _compartmentRepository.GetByIdWithSeatsAsync(seat.CompartmentId);

        if (compartment == null)
        {
            throw new NotFoundException(nameof(Compartment), seat.CompartmentId);
        }

        // if (compartment.Seats.Count >= compartment.NumberOfSeats)
        // {
        //     throw new BadRequestException(400, "The number of seats is full");
        // }

        await _repository.Add(seat);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Seat seat)
    {
        await _repository.Delete(seat);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PagedList<SeatDto>> GetAllDtoAsync(SeatQueryParams queryParams)
    {
        var query = await _repository.GetQueryWithSeatTypeAndCompartmentAsync();

        if (queryParams.SeatTypeId != 0)
        {
            query = query.Where(s => s.SeatTypeId == queryParams.SeatTypeId);
        }

        if (queryParams.CompartmentId != 0)
        {
            query = query.Where(s => s.CompartmentId == queryParams.CompartmentId);
        }

        if (!string.IsNullOrEmpty(queryParams.SearchTerm))
            query = query.Where(s => s.Name.Contains(queryParams.SearchTerm.Trim()));

        query = queryParams.Sort switch
        {
            "nameAsc" => query.OrderBy(s => s.Name),
            "nameDesc" => query.OrderByDescending(s => s.Name),
            "seatTypeNameAsc" => query.OrderBy(s => s.SeatType.Name),
            "seatTypeNameDesc" => query.OrderByDescending(s => s.SeatType.Name),
            "compartmentNameAsc" => query.OrderBy(s => s.Compartment.Name),
            "compartmentNameDesc" => query.OrderByDescending(s => s.Compartment.Name),
            "createdAtAsc" => query.OrderBy(s => s.CreatedAt),
            "createdAtDesc" => query.OrderByDescending(s => s.CreatedAt),
            _ => query.OrderBy(s => s.CreatedAt)
        };

        var seatDtoQuery = query.Select(s => _mapper.Map<SeatDto>(s));

        return await PagedList<SeatDto>.CreateAsync(seatDtoQuery, queryParams.PageNumber,
            queryParams.PageSize);
    }

    public async Task<List<SeatDto>> GetAllDtoNoPagingAsync()
    {
        var seat = await _repository.GetAllNoPagingAsync();
        return _mapper.Map<List<SeatDto>>(seat);
    }

    public async Task<Seat> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<SeatDto> GetDtoByIdAsync(int id)
    {
        var seat = await _repository.GetByIdWithCompartment(id);
        return _mapper.Map<SeatDto>(seat);
    }

    public async Task SoftDeleteAsync(Seat seat)
    {
        await _repository.SoftDelete(seat);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(Seat seat)
    {
        var seatInDb = await _repository.GetByIdAsync(seat.Id);

        if (seatInDb == null) throw new NotFoundException(nameof(Seat), seat.Id);

        seatInDb.SeatTypeId = seat.SeatTypeId;
        seatInDb.Name = seat.Name;
        seatInDb.CompartmentId = seat.CompartmentId;
        seatInDb.Status = seat.Status;
        seatInDb.UpdatedAt = DateTime.Now;

        await _unitOfWork.SaveChangesAsync();
    }
}
