namespace Application.Common.Interfaces.Services
{
    public interface ITrainStationService : IService<TrainStation>
    {
        Task<PagedList<TrainStationDto>> GetAllDtoAsync(QueryParams queryParams);

        Task<TrainStationDto> GetDtoByIdAsync(int id);
    }
}