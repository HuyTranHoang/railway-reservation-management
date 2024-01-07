namespace Application.Common.Interfaces.Services;
public interface IDistanceFareService : IService<DistanceFare>
{
    Task<PagedList<DistanceFareDto>> GetAllDtoAsync(DistanceFareQueryParams queryParams);
    Task<DistanceFareDto> GetDtoByIdAsync(int id);
}