namespace Application.Common.Interfaces.Services;

public interface ICarriageService : IService<Carriage>
{
    Task<PagedList<CarriageDto>> GetAllDtoAsync(CarriageQueryParams queryParams);
    Task<CarriageDto> GetDtoByIdAsync(int id);
    Task<int> GetCompartmentsBelongToCarriageCountAsync(int carriageId);
}