using Domain.Exceptions;
using Serilog;

namespace Application.Services;

public class ScheduleService : IScheduleService
{
    private readonly IScheduleRepository _repository;
    private readonly IDistanceFareRepository _distanceFareRepository;
    private readonly ITrainStationRepository _trainStationRepository;
    private readonly ITrainRepository _trainRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public ScheduleService(IScheduleRepository repository,
                            IDistanceFareRepository distanceFareRepository,
                            ITrainStationRepository trainStationRepository,
                            ITrainRepository trainRepository,
                            IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _distanceFareRepository = distanceFareRepository;
        _trainStationRepository = trainStationRepository;
        _trainRepository = trainRepository;
    }

    public async Task AddAsync(Schedule schedule)
    {

        if (NameExists(_repository, schedule.Name, schedule.Id))
        {
            throw new BadRequestException(400, "Name already exists");
        }

        if (ScheduleConflictsOrDeparture(_repository, schedule))
        {
            throw new BadRequestException(400, "Trùng Lịch Trình Hoặc Thời Gian Chỉ Định Đã Có!!");
        }

        schedule.Price = await CalculatePrice(schedule);
        
        int durationInMinutes = await CalculateDurationInMinutes(schedule);
        schedule.Duration = durationInMinutes;

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
            query = query.Where(t => t.TrainId == queryParams.TrainId);
        }

        if (queryParams.DepartureStationId != 0)
        {
            query = query.Where(t => t.DepartureStationId == queryParams.DepartureStationId);
        }

        if (queryParams.ArrivalStationId != 0)
        {
            query = query.Where(t => t.ArrivalStationId == queryParams.ArrivalStationId);
        }

        if (!string.IsNullOrEmpty(queryParams.SearchTerm))
        {
            query = query.Where(p => p.Name.Contains(queryParams.SearchTerm.Trim()));
        }

        query = queryParams.Sort switch
        {
            "scheduleNameAsc" => query.OrderBy(p => p.Name),
            "scheduleNameDesc" => query.OrderByDescending(p => p.Name),
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

        if (ScheduleConflictsOrDeparture(_repository, schedule))
        {
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
    private static bool ScheduleConflictsOrDeparture(IScheduleRepository repository, Schedule schedule)
    {
        return repository
            .GetQueryAsync()
            .Result
            .Any(s =>
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

    private async Task<double> CalculatePrice(Schedule schedule)
    {
        var departure = await _trainStationRepository.GetByIdAsync(schedule.DepartureStationId);
        if (departure == null)
        {
            Console.WriteLine($"Không tìm thấy TrainStation với Id = {schedule.DepartureStationId}");
        }
        var arrival = await _trainStationRepository.GetByIdAsync(schedule.ArrivalStationId);
        if (departure == null)
        {
            Console.WriteLine($"Không tìm thấy TrainStation với Id = {schedule.ArrivalStationId}");
        }
        int distance = arrival.CoordinateValue - departure.CoordinateValue;
        var train = await _trainRepository.GetByIdAsync(schedule.TrainId);

        var distanceFare = await _distanceFareRepository.GetByDistanceAsync(distance, train.TrainCompanyId);
        return (double)distanceFare;
    }

    private async Task<int> CalculateDurationInMinutes(Schedule schedule)
    {
        var departureStation = await _trainStationRepository.GetByIdAsync(schedule.DepartureStationId);
        var arrivalStation = await _trainStationRepository.GetByIdAsync(schedule.ArrivalStationId);

        if (departureStation == null || arrivalStation == null)
        {
            throw new Exception("Departure or arrival station not found.");
        }

        int distance = arrivalStation.CoordinateValue - departureStation.CoordinateValue;

        double estimatedSpeed = 80.0;
        
        double travelTimeHours = distance / estimatedSpeed;

        int travelTimeMinutes = (int)Math.Round(travelTimeHours * 60);

        return travelTimeMinutes;
    }
    private async Task<int> CalculateDurationInMinutesChild(int departureStationId, int arrivalStationId)
    {
        var departureStation = await _trainStationRepository.GetByIdAsync(departureStationId);
        var arrivalStation = await _trainStationRepository.GetByIdAsync(arrivalStationId);

        if (departureStation == null || arrivalStation == null)
        {
            throw new Exception("Departure or arrival station not found.");
        }

        int distance = arrivalStation.CoordinateValue - departureStation.CoordinateValue;

        double estimatedSpeed = 80.0;
        
        double travelTimeHours = distance / estimatedSpeed;

        int travelTimeMinutes = (int)Math.Round(travelTimeHours * 60);

        return travelTimeMinutes;
    }

    public async Task CreateSchedulesForTrainPassingAsync(Schedule largeSchedule)
    {
        var departureStation = await _trainStationRepository.GetByIdAsync(largeSchedule.DepartureStationId);
        var arrivalStation = await _trainStationRepository.GetByIdAsync(largeSchedule.ArrivalStationId);

        if (departureStation == null || arrivalStation == null)
        {
            throw new Exception("Departure or arrival station not found.");
        }

        var intermediateStations = await _trainStationRepository.GetStationsFromToAsync(departureStation.CoordinateValue, arrivalStation.CoordinateValue);

        var departureDate = largeSchedule.DepartureDate;
        var arrivalDate = largeSchedule.ArrivalDate;
        var departureTime = largeSchedule.DepartureTime;

        for (int i = 0; i < intermediateStations.Count - 1; i++)
        {
            for (int j = i + 1; j < intermediateStations.Count; j++)
            {
                Console.WriteLine(intermediateStations[i] + " " + intermediateStations[j]);

                var existingSchedule = await _repository.GetScheduleByStationsAsync(largeSchedule.TrainId, intermediateStations[i].Id, intermediateStations[j].Id);

                if (existingSchedule == null)
                {
                    if (i == 0 && j > 1)
                    {
                        var schedule = new Schedule
                        {
                            Name = $"{largeSchedule.Name}-{i+1}",
                            TrainId = largeSchedule.TrainId,
                            DepartureStationId = intermediateStations[i].Id,
                            ArrivalStationId = intermediateStations[j].Id,
                            DepartureDate = departureDate.AddMinutes(await CalculateDurationInMinutesChild(intermediateStations[i].Id, intermediateStations[j].Id)),
                            ArrivalDate = arrivalDate.AddMinutes(await CalculateDurationInMinutesChild(intermediateStations[i].Id, intermediateStations[j].Id)),
                            DepartureTime = departureTime.AddMinutes(await CalculateDurationInMinutesChild(intermediateStations[i].Id, intermediateStations[j].Id)),
                            Duration = await CalculateDurationInMinutesChild(intermediateStations[i].Id, intermediateStations[j].Id),
                            Status = "Active",
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };
                        schedule.Price = await CalculatePrice(schedule);

                        _repository.Add(schedule);
                    } else
                    {
                        // var previousDuration = await CalculateDurationInMinutesChild(intermediateStations[0].Id, intermediateStations[j-1].Id);
                        var schedule = new Schedule
                        {
                            Name = $"{largeSchedule.Name}-{i+1}",
                            TrainId = largeSchedule.TrainId,
                            DepartureStationId = intermediateStations[i].Id,
                            ArrivalStationId = intermediateStations[j].Id,
                            DepartureDate = departureDate.AddMinutes(await CalculateDurationInMinutesChild(intermediateStations[0].Id, intermediateStations[i].Id)),
                            ArrivalDate = arrivalDate.AddMinutes(await CalculateDurationInMinutesChild(intermediateStations[0].Id, intermediateStations[i].Id)),
                            DepartureTime = departureTime.AddMinutes(await CalculateDurationInMinutesChild(intermediateStations[0].Id, intermediateStations[i].Id)),
                            Duration = await CalculateDurationInMinutesChild(intermediateStations[i].Id, intermediateStations[j].Id),
                            Status = "Active",
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };
                        schedule.Price = await CalculatePrice(schedule);

                        _repository.Add(schedule);
                    }
                    

                    
                }
                
            }
        }

        await _unitOfWork.SaveChangesAsync();
}

}
