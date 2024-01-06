namespace Application.Common.Interfaces.Persistence;

public interface IScheduleRepository : IRepository<Schedule>
{
    Task<IQueryable<Schedule>> GetQueryWithTrainAndStationAsync();

}
