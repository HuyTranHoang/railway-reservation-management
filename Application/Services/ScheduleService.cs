using Domain.Exceptions;

namespace Application.Services;

public class ScheduleService : IScheduleService
{
    private readonly IScheduleRepository _repository;

    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public ScheduleService(IScheduleRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task AddAsync(Schedule schedule)
    {

        if (NameExists(_repository, schedule.Name, schedule.Id))
        {
            throw new BadRequestException(400, "Name already exists");
        }

        if (TrungLichTrinhHoacTrongThoiGianTauDangChay(_repository, schedule))
        {
            throw new BadRequestException(400, "Trùng Lịch Trình Hoặc Thời Gian Chỉ Định Đã Có!!");
        }

        _repository.Add(schedule);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Schedule schedule)
    {
        _repository.Delete(schedule);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PagedList<ScheduleDto>> GetAllDtoAsync(ScheduleQueryParams queryParams)
    {
        var query = await _repository.GetQueryWithTrainAndStationAsync();

        if (queryParams.TrainId != 0)
        {
            query = query.Where(t => t.TrainId == queryParams.TrainId)
            ;
        }

        if (!string.IsNullOrEmpty(queryParams.SearchTerm))
        {
            query = query.Where(p => p.Name.Contains(queryParams.SearchTerm));
        }

        query = queryParams.Sort switch
        {
            "nameScheduleAsc" => query.OrderBy(p => p.Name),
            "nameScheduleDesc" => query.OrderByDescending(p => p.Name),
            "departureTimeAsc" => query.OrderBy(p => p.DepartureTime),
            "departureTimeDesc" => query.OrderByDescending(p => p.DepartureTime),
            _ => query.OrderBy(p => p.CreatedAt)
        };

        var scheduleDtoQuery = query.Select(s => _mapper.Map<ScheduleDto>(s));

        return await PagedList<ScheduleDto>.CreateAsync(scheduleDtoQuery, queryParams.PageNumber, queryParams.PageSize);
    }

    public Task<Schedule> GetByIdAsync(int id)
    {
        return _repository.GetByIdAsync(id);
    }

    public Task<int> GetCompartmentsBelongToCarriageCountAsync(int scheduleId)
    {
        throw new NotImplementedException();
    }

    public async Task<ScheduleDto> GetDtoByIdAsync(int id)
    {
        var scheduleDto = await _repository.GetByIdAsync(id);
        return _mapper.Map<ScheduleDto>(scheduleDto);
    }

    public async Task SoftDeleteAsync(Schedule schedule)
    {
        _repository.SoftDelete(schedule);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(Schedule schedule)
    {
        var scheduleInDb = await _repository.GetByIdAsync(schedule.Id);

        if (scheduleInDb == null) throw new NotFoundException(nameof(Schedule), schedule.Id);

        if (NameExists(_repository, schedule.Name, schedule.Id))
        {
            throw new BadRequestException(400, "Name already exists");
        }

        if (TrungLichTrinhHoacTrongThoiGianTauDangChay(_repository, schedule))
        {
            //Đặt lỗi english giúp emm ;.; dịch ngu quá
            throw new BadRequestException(400, "Trùng Lịch Trình Hoặc Thời Gian Chỉ Định Đã Có!!");
        }

        scheduleInDb.Name = schedule.Name;
        scheduleInDb.TrainId = schedule.TrainId;
        scheduleInDb.DepartureStationId = schedule.DepartureStationId;
        scheduleInDb.ArrivalStationId = schedule.ArrivalStationId;
        scheduleInDb.DepartureTime = schedule.DepartureTime;
        scheduleInDb.Duration = schedule.Duration;
        scheduleInDb.Status = schedule.Status;
        scheduleInDb.UpdatedAt = DateTime.Now;

        await _unitOfWork.SaveChangesAsync();
    }

    private static bool NameExists(IScheduleRepository repository, string name, int scheduleId)
    {
        return repository.GetQueryAsync().Result.Any(t => t.Name == name && t.Id != scheduleId);
    }


    //Đặt tên english giúp em ;.;
    private static bool TrungLichTrinhHoacTrongThoiGianTauDangChay(IScheduleRepository repository, Schedule schedule)
    {
        return repository.GetQueryAsync().Result.Any(s =>
                    s.TrainId != schedule.TrainId &&
                    s.DepartureStationId == schedule.DepartureStationId ||
                    s.ArrivalStationId == schedule.ArrivalStationId &&
                        (
                        (s.DepartureDate <= schedule.DepartureDate && s.ArrivalDate >= schedule.DepartureDate) ||
                        (s.DepartureDate <= schedule.ArrivalDate && s.ArrivalDate >= schedule.ArrivalDate) ||
                        (s.DepartureDate >= schedule.DepartureDate && s.ArrivalDate <= schedule.ArrivalDate)
            )
        );
    }

}
