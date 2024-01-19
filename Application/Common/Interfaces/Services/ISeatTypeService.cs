namespace Application.Common.Interfaces.Services;

public interface ISeatTypeService : IService<SeatType>
{
    Task<PagedList<SeatTypeDto>> GetAllDtoAsync(QueryParams queryParams);

    Task<List<SeatTypeDto>> GetAllDtoNoPagingAsync();
    Task<SeatTypeDto> GetDtoByIdAsync(int id);
    Task<SeatTypeDto> GetDtoByNameAsync(string name);
}