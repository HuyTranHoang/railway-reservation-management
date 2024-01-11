namespace Application.Common.Interfaces.Services
{
    public interface ICancellationService : IService<Cancellation>
    {
        Task<PagedList<CancellationDto>> GetAllDtoAsync(CancellationQueryParams queryParams);
        Task<CancellationDto> GetDtoByIdAsync(int id);
    }
}