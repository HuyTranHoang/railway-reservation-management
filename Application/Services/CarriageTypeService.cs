using Domain.Exceptions;

namespace Application.Services;

public class CarriageTypeService : ICarriageTypeService
{
    private readonly ICarriageTypeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CarriageTypeService(ICarriageTypeRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedList<CarriageTypeDto>> GetAllDtoAsync(QueryParams queryParams)
    {
        var query = await _repository.GetQueryAsync();

        if (!string.IsNullOrEmpty(queryParams.SearchTerm))
            query = query.Where(ct => ct.Name.Contains(queryParams.SearchTerm.Trim()));

        query = queryParams.Sort switch
        {
            "nameAsc" => query.OrderBy(ct => ct.Name),
            "nameDesc" => query.OrderByDescending(ct => ct.Name),
            "numberOfCompartmentAsc" => query.OrderBy(ct => ct.NumberOfCompartments),
            "numberOfCompartmentDesc" => query.OrderByDescending(ct => ct.NumberOfCompartments),
            "numberOfSeatsAsc" => query.OrderBy(ct => ct.NumberOfSeats),
            "numberOfSeatsDesc" => query.OrderByDescending(ct => ct.NumberOfSeats),
            "numberOfSeatTypesAsc" => query.OrderBy(ct => ct.NumberOfSeatTypes),
            "numberOfSeatTypesDesc" => query.OrderByDescending(ct => ct.NumberOfSeatTypes),
            "serviceChargeAsc" => query.OrderBy(ct => ct.ServiceCharge),
            "serviceChargeDesc" => query.OrderByDescending(ct => ct.ServiceCharge),
            "createdAtDesc" => query.OrderByDescending(ct => ct.CreatedAt),
            _ => query.OrderBy(ct => ct.CreatedAt)
        };

        var carriageTypeDtoQuery = query.Select(ct => _mapper.Map<CarriageTypeDto>(ct));

        return await PagedList<CarriageTypeDto>.CreateAsync(carriageTypeDtoQuery, queryParams.PageNumber,
            queryParams.PageSize);
    }

    public async Task<CarriageTypeDto> GetDtoByIdAsync(int id)
    {
        var carriageType = await _repository.GetByIdAsync(id);
        return _mapper.Map<CarriageTypeDto>(carriageType);
    }


    public async Task<CarriageType> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task AddAsync(CarriageType carriageType)
    {
        await _repository.Add(carriageType);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(CarriageType carriageType)
    {
        var carriageTypeInDb = await _repository.GetByIdAsync(carriageType.Id);

        if (carriageTypeInDb == null) throw new NotFoundException(nameof(CarriageType), carriageType.Id);

        carriageTypeInDb.Name = carriageType.Name;
        carriageTypeInDb.ServiceCharge = carriageType.ServiceCharge;
        carriageTypeInDb.NumberOfCompartments = carriageType.NumberOfCompartments;
        carriageTypeInDb.NumberOfSeats = carriageType.NumberOfSeats;
        carriageTypeInDb.NumberOfSeatTypes = carriageType.NumberOfSeatTypes;
        carriageTypeInDb.Description = carriageType.Description;
        carriageTypeInDb.Status = carriageType.Status;
        carriageTypeInDb.UpdatedAt = DateTime.Now;

        await _repository.Update(carriageTypeInDb);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(CarriageType carriageType)
    {
        await _repository.Delete(carriageType);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(CarriageType carriageType)
    {
        await _repository.SoftDelete(carriageType);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<List<CarriageTypeDto>> GetAllDtoNoPagingAsync()
    {
        var carriageTypes = await _repository.GetAllNoPagingAsync();
        return _mapper.Map<List<CarriageTypeDto>>(carriageTypes);
    }
}