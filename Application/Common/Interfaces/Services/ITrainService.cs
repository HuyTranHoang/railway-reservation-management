namespace Application.Common.Interfaces.Services;

public interface ITrainService : IService<Train>
{
    Task<PagedList<TrainDto>> GetAllDtoAsync(TrainQueryParams queryParams);
    Task<TrainDto> GetDtoByIdAsync(int id);
}