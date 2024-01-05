namespace Application.Common.Interfaces.Services
{
    public interface IRoundTripService : IService<RoundTrip>
    {
        Task<PagedList<RoundTripDto>> GetAllDtoAsync(RoundTripParams queryParams);
        Task<RoundTripDto> GetDtoByIdAsync(int id);
    }
}