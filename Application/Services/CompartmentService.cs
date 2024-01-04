namespace Application.Services;

public class CompartmentService : ICompartmentService
{
    private readonly ICompartmentRepository _repository;

    private readonly IMapper _mapper;

    private readonly IUnitOfWork _unitOfWork;
    public CompartmentService(ICompartmentRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task AddAsync(Compartment compartment)
    {
        if (NameExists(_repository, compartment.Name, compartment.Id))
        {
            throw new BadRequestException(400, "Name already exists");
        }

        _repository.Add(compartment);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Compartment compartment)
    {

        _repository.Delete(compartment);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PagedList<CompartmentDto>> GetAllDtoAsync(CompartmentQueryParams queryParams)
    {
        var query = await _repository.GetQueryWithCarriageAsync();

        if (queryParams.CarriageId != 0)
        {
            query = query.Where(c => c.CarriageId == queryParams.CarriageId);
        }

        if (!string.IsNullOrEmpty(queryParams.SearchTerm))
        {
            query = query.Where(p => p.Name.Contains(queryParams.SearchTerm));
        }

        query = queryParams.Sort switch
        {
            "nameAsc" => query.OrderBy(p => p.Name),
            "nameDesc" => query.OrderByDescending(p => p.Name),
            "numberOfSeatsAsc" => query.OrderBy(p => p.NumberOfSeats),
            "numberOfSeatsDesc" => query.OrderByDescending(p => p.NumberOfSeats),
            _ => query.OrderBy(p => p.CreatedAt)
        };

        var compartmentDtoQuery = query.Select(p => _mapper.Map<CompartmentDto>(p));

        return await PagedList<CompartmentDto>.CreateAsync(compartmentDtoQuery, queryParams.PageNumber,
        queryParams.PageSize);
    }

    public async Task<Compartment> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<CompartmentDto> GetDtoByIdAsync(int id)
    {
        var compartmentDto = await _repository.GetByIdAsync(id);
        return _mapper.Map<CompartmentDto>(compartmentDto);
    }

    public async Task SoftDeleteAsync(Compartment compartment)
    {
        _repository.SoftDelete(compartment);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(Compartment compartment)
    {
        var compartmentInDb = await _repository.GetByIdAsync(compartment.Id);

        if (compartmentInDb is null) throw new NotFoundException(nameof(Passenger), compartment.Id);

        if (NameExists(_repository, compartment.Name, compartment.Id))
        {
            throw new BadRequestException(400, "Name already exists");
        }

        compartmentInDb.Name = compartment.Name;
        compartmentInDb.CarriageId = compartment.CarriageId;
        compartmentInDb.NumberOfSeats = compartment.NumberOfSeats;
        compartmentInDb.Status = compartment.Status;
        compartmentInDb.UpdatedAt = compartment.UpdatedAt;

        _repository.Update(compartmentInDb);

        await _unitOfWork.SaveChangesAsync();
    }


    private static bool NameExists(ICompartmentRepository repository, string name, int compartmentId)
    {
        return repository.GetQueryAsync().Result.Any(t => t.Name == name);
    }
}
