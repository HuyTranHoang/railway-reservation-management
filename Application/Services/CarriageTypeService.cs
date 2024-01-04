namespace Application.Services;

public class CarriageTypeService : ICarriageTypeService
{
    private readonly ICarriageTypeReponsitory _reponsitory;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CarriageTypeService(ICarriageTypeReponsitory reponsitory, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _reponsitory = reponsitory;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedList<CarriageTypeDto>> GetAllDtoAsync(QueryParams queryParams)
    {
        throw new NotImplementedException();
    }

    public async Task<CarriageTypeDto> GetDtoByIdAsync(int id)
    {
        throw new NotImplementedException();
    }


    public async Task<CarriageType> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(CarriageType t)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(CarriageType t)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(CarriageType t)
    {
        throw new NotImplementedException();
    }

    public async Task SoftDeleteAsync(CarriageType t)
    {
        throw new NotImplementedException();
    }

}