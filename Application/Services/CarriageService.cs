using Domain.Exceptions;

namespace Application.Services;

public class CarriageService : ICarriageService
{
    private readonly ICarriageRepository _repository;
    private readonly ICarriageTypeRepository _carriageTypeRepository;
    private readonly ICompartmentRepository _compartmentRepository;
    private readonly ISeatRepository _seatRepository;
    private readonly ISeatTypeRepository _seatTypeRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CarriageService(ICarriageRepository repository,
        ICarriageTypeRepository carriageTypeRepository,
        ICompartmentRepository compartmentRepository,
        ISeatRepository seatRepository,
        ISeatTypeRepository seatTypeRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _repository = repository;
        _carriageTypeRepository = carriageTypeRepository;
        _compartmentRepository = compartmentRepository;
        _seatRepository = seatRepository;
        _seatTypeRepository = seatTypeRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task AddAsync(Carriage carriage)
    {
        await _repository.Add(carriage);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Carriage carriage)
    {
        await _repository.Delete(carriage);
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
            query = query.Where(
                p => p.Name.Contains(queryParams.SearchTerm.Trim()) ||
                     p.Train.Name.Contains(queryParams.SearchTerm.Trim()) ||
                     p.CarriageType.Name.Contains(queryParams.SearchTerm.Trim()));

        query = queryParams.Sort switch
        {
            "nameAsc" => query.OrderBy(p => p.Name),
            "nameDesc" => query.OrderByDescending(p => p.Name),
            "trainNameAsc" => query.OrderBy(p => p.Train.Name),
            "trainNameDesc" => query.OrderByDescending(p => p.Train.Name),
            "typeNameAsc" => query.OrderBy(p => p.CarriageType.Name),
            "typeNameDesc" => query.OrderByDescending(p => p.CarriageType.Name),
            "createdAtDesc" => query.OrderByDescending(p => p.CreatedAt),
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
        await _repository.SoftDelete(carriage);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(Carriage carriage)
    {
        var carriageInDb = await _repository.GetByIdAsync(carriage.Id);

        if (carriageInDb == null) throw new NotFoundException(nameof(Passenger), carriage.Id);

        carriageInDb.Name = carriage.Name;
        carriageInDb.TrainId = carriage.TrainId;
        carriageInDb.CarriageTypeId = carriage.CarriageTypeId;
        carriageInDb.Status = carriage.Status;
        carriageInDb.UpdatedAt = DateTime.Now;

        await _repository.Update(carriageInDb);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<List<CarriageDto>> GetAllDtoNoPagingAsync()
    {
        var carriages = await _repository.GetAllNoPagingAsync();
        return _mapper.Map<List<CarriageDto>>(carriages);
    }

    public async Task AddWithCompartmentAndSeatAsync(Carriage carriage)
    {
        await _repository.Add(carriage);

        await _unitOfWork.SaveChangesAsync();

        var carriageType = await _carriageTypeRepository.GetByIdAsync(carriage.CarriageTypeId);
        if (carriageType == null)
        {
            throw new NotFoundException(nameof(CarriageType), carriage.CarriageTypeId);
        }

        var compartments = new List<Compartment>();
        var seats = new List<Seat>();

        for (var i = 1; i <= carriageType.NumberOfCompartments; i++)
        {
            var compartment = new Compartment
            {
                Name = carriageType.Name + '-' + i,
                CarriageId = carriage.Id,
                Status = "Active",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            compartments.Add(compartment);
        }
        
        await _compartmentRepository.AddRangeAsync(compartments);
        await _unitOfWork.SaveChangesAsync();

        var numberOfSeatsPerCompartment = carriageType.Name switch
        {
            "Ngồi mềm điều hòa" => 32,
            "Giường nằm khoang 4 điều hòa" => 4,
            _ => 6 // Giường nằm khoang 6 điều hòa
            // Có thể phải cần đem cái này vào database thay vì hardcode như này.
        };

        foreach (var compartment in compartments)
        {
            // var seatType = await _seatTypeRepository.GetByIdAsync(carriageType.SeatTypeId);
            // if (seatType == null)
            // {
            //     throw new NotFoundException(nameof(SeatType), carriageType.SeatTypeId);
            // }

            for (var i = 1; i <= numberOfSeatsPerCompartment; i++)
            {
                var seat = new Seat
                {
                    Name = compartment.Name + '-' + i,
                    CompartmentId = compartment.Id,
                    SeatTypeId = 1, //Phải set lại chỗ này
                    Status = "Active",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                seats.Add(seat);
            }
        }

        await _seatRepository.AddRangeAsync(seats);
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
