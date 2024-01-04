namespace Application.Common.Interfaces.Services;

public interface IPassengerService : IService<Passenger>
{
    Task<PagedList<PassengerDto>> GetAllDtoAsync(QueryParams queryParams);
    Task<PassengerDto> GetDtoByIdAsync(int id);
}