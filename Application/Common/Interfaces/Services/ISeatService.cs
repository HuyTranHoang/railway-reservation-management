namespace Application.Common.Interfaces.Services;

public interface ISeatService : IService<Seat>
{
    Task<PagedList<SeatDto>> GetAllDtoAsync(SeatQueryParams queryParams);

    Task<List<SeatDto>> GetAllDtoNoPagingAsync();

    Task<SeatDto> GetDtoByIdAsync(int id);
}