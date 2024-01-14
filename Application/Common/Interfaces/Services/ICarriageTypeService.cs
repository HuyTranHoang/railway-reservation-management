namespace Application.Common.Interfaces.Services;

public interface ICarriageTypeService : IService<CarriageType>
{
    Task<PagedList<CarriageTypeDto>> GetAllDtoAsync(QueryParams queryParams);
    Task<CarriageTypeDto> GetDtoByIdAsync(int id);
    Task<List<CarriageTypeDto>> GetAllDtoNoPagingAsync();
}