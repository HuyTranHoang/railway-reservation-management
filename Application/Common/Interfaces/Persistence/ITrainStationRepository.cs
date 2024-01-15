namespace Application.Common.Interfaces.Persistence
{
    public interface ITrainStationRepository : IRepository<TrainStation>
    {
         Task<List<TrainStation>> GetStationsFromToAsync(int startCoordinate, int endCoordinate);
    }
}