using Domain.Exceptions;

namespace Application.Services;

public class CompartmentService : ICompartmentService
{
    private readonly ICompartmentRepository _repository;
    private readonly ICarriageRepository _carriageRepository;
    private readonly IMapper _mapper;

    private readonly IUnitOfWork _unitOfWork;
    public CompartmentService(ICompartmentRepository repository,
        ICarriageRepository carriageRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _carriageRepository = carriageRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Compartment> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task AddAsync(Compartment compartment)
    {
        var carriage = await _carriageRepository.GetByIdWithCompartmentsAsync(compartment.CarriageId);

        if (carriage == null)
        {
            throw new NotFoundException(nameof(Carriage), compartment.CarriageId);
        }

        if (carriage.Compartments.Count >= carriage.NumberOfCompartments)
        {
            throw new BadRequestException(400, "The number of compartments is full");
        }

        _repository.Add(compartment);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(Compartment compartment)
    {
        var compartmentInDb = await _repository.GetByIdAsync(compartment.Id);

        if (compartmentInDb == null)
        {
            throw new NotFoundException(nameof(Compartment), compartment.Id);
        }

        compartmentInDb.Name = compartment.Name;
        compartmentInDb.CarriageId = compartment.CarriageId;
        compartmentInDb.NumberOfSeats = compartment.NumberOfSeats;
        compartmentInDb.Status = compartment.Status;
        compartmentInDb.UpdatedAt = DateTime.Now;

        _repository.Update(compartmentInDb);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Compartment compartment)
    {
        _repository.Delete(compartment);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(Compartment compartment)
    {
        _repository.SoftDelete(compartment);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PagedList<CompartmentDto>> GetAllDtoAsync(CompartmentQueryParams queryParams)
    {
        var query = await _repository.GetQueryWithCarriageAsync();

        if (!string.IsNullOrEmpty(queryParams.SearchTerm))
            query = query.Where(p => p.Name.Contains(queryParams.SearchTerm.Trim()));

        query = queryParams.Sort switch
        {
            "numberOfSeatsAsc" => query.OrderBy(p => p.NumberOfSeats),
            "numberOfSeatsDesc" => query.OrderByDescending(p => p.NumberOfSeats),
            "nameAsc" => query.OrderBy(p => p.Name),
            "nameDesc" => query.OrderByDescending(p => p.Name),
            _ => query.OrderBy(p => p.CreatedAt)
        };

        var compartmentDtoQuery = query.Select(p => _mapper.Map<CompartmentDto>(p));

        return await PagedList<CompartmentDto>.CreateAsync(compartmentDtoQuery, queryParams.PageNumber,
            queryParams.PageSize);
    }

    public async Task<CompartmentDto> GetDtoByIdAsync(int id)
    {
        var compartment = await _repository.GetByIdAsync(id);
        return _mapper.Map<CompartmentDto>(compartment);
    }

    public async Task<int> GetSeatsBelongToCompartmentCountAsync(int compartmentId)
    {
        var compartment = await _repository.GetByIdWithSeatsAsync(compartmentId);

        if (compartment == null)
        {
            throw new NotFoundException(nameof(Carriage), compartmentId);
        }

        return compartment.Seats.Count;
    }
}
