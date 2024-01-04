namespace Application.Common.Interfaces.Services;

public interface ISeatTypeService : IService<SeatType>
{
    Task<PagedList<SeatTypeDto>> GetAllDtoAsync(QueryParams queryParams);
    Task<SeatTypeDto> GetDtoByIdAsync(int id);
}