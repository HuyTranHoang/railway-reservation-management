namespace Application.Common.Interfaces.Persistence;

public interface IScheduleRepository : IRepository<Schedule>
{
    Task<IQueryable<Schedule>> GetQueryWithTrainAndStationAsync();

    Task<Schedule> GetScheduleByStationsAsync(int trainId, int departureStationId, int arrivalStationId);

    Task<List<Schedule>> GetSchedulesByTrainAsync(int trainId);
    Task<List<Schedule>> GetUpcomingSchedules(int numberOfSchedules);

}
