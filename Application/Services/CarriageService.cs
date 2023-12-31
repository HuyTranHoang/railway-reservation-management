using Domain.Exceptions;

namespace Application.Services;

public class CarriageService : ICarriageService
{
    private readonly ICarriageRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CarriageService(ICarriageRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task AddAsync(Carriage carriage)
    {
        _repository.Add(carriage);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Carriage carriage)
    {
        _repository.Delete(carriage);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PagedList<CarriageDto>> GetAllDtoAsync(CarriageQueryParams queryParams)
    {
        var query = await _repository.GetQueryWithTrainAndTypeAsync();

        if (queryParams.TrainId != 0)
        {
            query = query.Where(t => t.TrainId == queryParams.TrainId);
        }

        if (queryParams.CarriageTypeId != 0)
        {
            query = query.Where(t => t.CarriageTypeId == queryParams.CarriageTypeId);
        }

        if (!string.IsNullOrEmpty(queryParams.SearchTerm))
            query = query.Where(p => p.Name.Contains(queryParams.SearchTerm));

        query = queryParams.Sort switch
        {
            "nameAsc" => query.OrderBy(p => p.Name),
            "nameDesc" => query.OrderByDescending(p => p.Name),
            "numberOfCompartmentAsc" => query.OrderBy(p => p.NumberOfCompartments),
            "numberOfCompartmentDesc" => query.OrderByDescending(p => p.NumberOfCompartments),
            _ => query.OrderBy(p => p.CreatedAt)
        };

        var carriageDtoQuery = query.Select(p => _mapper.Map<CarriageDto>(p));

        return await PagedList<CarriageDto>.CreateAsync(carriageDtoQuery, queryParams.PageNumber,
            queryParams.PageSize);
    }

    public async Task<Carriage> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<CarriageDto> GetDtoByIdAsync(int id)
    {
        var carriageDto = await _repository.GetByIdAsync(id);
        return _mapper.Map<CarriageDto>(carriageDto);
    }

    public async Task SoftDeleteAsync(Carriage carriage)
    {
        _repository.SoftDelete(carriage);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(Carriage carriage)
    {
        var carriageInDb = await _repository.GetByIdAsync(carriage.Id);

        if (carriageInDb == null) throw new NotFoundException(nameof(Passenger), carriage.Id);

        carriageInDb.Name = carriage.Name;
        carriageInDb.TrainId = carriage.TrainId;
        carriageInDb.CarriageTypeId = carriage.CarriageTypeId;
        carriageInDb.NumberOfCompartments = carriage.NumberOfCompartments;
        carriageInDb.Status = carriage.Status;
        carriageInDb.UpdatedAt = DateTime.Now;

        _repository.Update(carriageInDb);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<int> GetCompartmentsBelongToCarriageCountAsync(int carriageId)
    {
        var carriage = await _repository.GetByIdWithCompartmentsAsync(carriageId);

        if (carriage == null)
        {
            throw new NotFoundException(nameof(Carriage), carriageId);
        }

        return carriage.Compartments.Count;
    }

}
