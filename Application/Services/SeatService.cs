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

        if (compartment.Seats.Count >= compartment.NumberOfSeats)
        {
            throw new BadRequestException(400, "The number of seats is full");
        }

        _repository.Add(seat);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Seat seat)
    {
        _repository.Delete(seat);
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
            query = query.Where(s => s.SeatType.Name.Contains(queryParams.SearchTerm));

        query = queryParams.Sort switch
        {
            "seatTypeNameAsc" => query.OrderBy(s => s.SeatType.Name),
            "seatTypeNameDesc" => query.OrderByDescending(s => s.SeatType.Name),
            "compartmentNameAsc" => query.OrderBy(s => s.Compartment.Name),
            "compartmentNameDesc" => query.OrderByDescending(s => s.Compartment.Name),
            _ => query.OrderBy(s => s.CreatedAt)
        };

        var seatDtoQuery = query.Select(s => _mapper.Map<SeatDto>(s));

        return await PagedList<SeatDto>.CreateAsync(seatDtoQuery, queryParams.PageNumber,
            queryParams.PageSize);
    }

    public async Task<Seat> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<SeatDto> GetDtoByIdAsync(int id)
    {
        var seatDto = await _repository.GetByIdAsync(id);
        return _mapper.Map<SeatDto>(seatDto);
    }

    public async Task SoftDeleteAsync(Seat seat)
    {
        _repository.SoftDelete(seat);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(Seat seat)
    {
        var seatInDb = await _repository.GetByIdAsync(seat.Id);

        if (seatInDb == null) throw new NotFoundException(nameof(Seat), seat.Id);

        seatInDb.SeatTypeId = seat.SeatTypeId;
        seatInDb.CompartmentId = seat.CompartmentId;
        seatInDb.Status = seat.Status;
        seatInDb.UpdatedAt = DateTime.Now;

        await _unitOfWork.SaveChangesAsync();
    }
}
