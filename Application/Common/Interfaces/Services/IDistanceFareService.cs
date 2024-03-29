namespace Application.Common.Interfaces.Services;
public interface IDistanceFareService : IService<DistanceFare>
{
    Task<PagedList<DistanceFareDto>> GetAllDtoAsync(DistanceFareQueryParams queryParams);
    Task<DistanceFareDto> GetDtoByIdAsync(int id);
    Task<DistanceFareDto> GetDtoByDistanceAsync(int distance, int trainCompanyId);
    Task<double> TestDistance(int distance);
}