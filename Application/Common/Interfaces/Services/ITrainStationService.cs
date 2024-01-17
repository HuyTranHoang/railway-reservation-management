namespace Application.Common.Interfaces.Services
{
    public interface ITrainStationService : IService<TrainStation>
    {
        Task<PagedList<TrainStationDto>> GetAllDtoAsync(QueryParams queryParams);

        Task<List<TrainStationDto>> GetAllDtoNoPagingAsync();

        Task<TrainStationDto> GetDtoByIdAsync(int id);
    }
}