namespace Application.Common.Interfaces.Services;

public interface IScheduleService : IService<Schedule>
{
    Task<PagedList<ScheduleDto>> GetAllDtoAsync(ScheduleQueryParams queryParams);
    Task<ScheduleDto> GetDtoByIdAsync(int id);
}
