using Domain.Exceptions;


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

        if (schedule.DepartureTime >= schedule.ArrivalTime || schedule.ArrivalTime <= schedule.DepartureTime)
        {
            throw new BadRequestException(400, "Departure time must be before arrival time");
        }
        
        if (ScheduleConflictsOrDeparture(_repository, schedule))
        {
            throw new BadRequestException(400, "Schedule conflicts or departure time already exists");
        }

        schedule.Price = await CalculatePrice(schedule);

        int durationInMinutes = await CalculateDurationInMinutes(schedule);
        schedule.Duration = durationInMinutes;

        await _repository.Add(schedule);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Schedule schedule)
    {
        await _repository.Delete(schedule);
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

        if (queryParams.DepartureTime != null)
        {
            query = query.Where(t => t.DepartureTime.Date == queryParams.DepartureTime.Value.Date);
        }

        if (queryParams.ArrivalTime != null)
        {
            query = query.Where(t => t.ArrivalTime.Date == queryParams.ArrivalTime.Value.Date);
        }

        if (!string.IsNullOrEmpty(queryParams.SearchTerm))
        {
            query = query.Where(p => p.Name.Contains(queryParams.SearchTerm.Trim()) ||
                                     p.Train.Name.Contains(queryParams.SearchTerm.Trim()) ||
                                     p.DepartureStation.Name.Contains(queryParams.SearchTerm.Trim()) ||
                                     p.ArrivalStation.Name.Contains(queryParams.SearchTerm.Trim()));
        }

        query = queryParams.Sort switch
        {
            "scheduleNameAsc" => query.OrderBy(p => p.Name),
            "scheduleNameDesc" => query.OrderByDescending(p => p.Name),
            "trainNameAsc" => query.OrderBy(p => p.Train.Name),
            "trainNameDesc" => query.OrderByDescending(p => p.Train.Name),
            "departureStationNameAsc" => query.OrderBy(p => p.DepartureStation.Name),
            "departureStationNameDesc" => query.OrderByDescending(p => p.DepartureStation.Name),
            "arrivalStationNameAsc" => query.OrderBy(p => p.ArrivalStation.Name),
            "arrivalStationNameDesc" => query.OrderByDescending(p => p.ArrivalStation.Name),
            "departureTimeAsc" => query.OrderBy(p => p.DepartureTime),
            "departureTimeDesc" => query.OrderByDescending(p => p.DepartureTime),
            "arrivalTimeAsc" => query.OrderBy(p => p.ArrivalTime),
            "arrivalTimeDesc" => query.OrderByDescending(p => p.ArrivalTime),
            "createdAtDesc" => query.OrderByDescending(p => p.CreatedAt),
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
        await _repository.SoftDelete(schedule);
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

        if (schedule.DepartureTime >= schedule.ArrivalTime || schedule.ArrivalTime <= schedule.DepartureTime)
        {
            throw new BadRequestException(400, "Departure time must be before arrival time");
        }

        if (ScheduleConflictsOrDeparture(_repository, schedule))
        {
            throw new BadRequestException(400, "Schedule conflicts or departure time already exists");
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


    private static bool ScheduleConflictsOrDeparture(IScheduleRepository repository, Schedule schedule)
    {
        return repository
            .GetQueryAsync()
            .Result
            .Any(s =>
                s.Id != schedule.Id && // Thêm điều kiện này để bỏ qua chính nó khi update
                (
                    s.TrainId != schedule.TrainId &&
                    s.DepartureStationId == schedule.DepartureStationId ||
                    s.ArrivalStationId == schedule.ArrivalStationId &&
                    (
                        (s.DepartureTime <= schedule.DepartureTime && s.ArrivalTime >= schedule.DepartureTime) ||
                        (s.DepartureTime <= schedule.ArrivalTime && s.ArrivalTime >= schedule.ArrivalTime) ||
                        (s.DepartureTime >= schedule.DepartureTime && s.DepartureTime <= schedule.ArrivalTime)
                    )
                )
            );
    }


    private async Task<double> CalculatePrice(Schedule schedule)
    {
        var departure = await _trainStationRepository.GetByIdAsync(schedule.DepartureStationId);
        var arrival = await _trainStationRepository.GetByIdAsync(schedule.ArrivalStationId);

        int distance = arrival.CoordinateValue - departure.CoordinateValue;
        var train = await _trainRepository.GetByIdAsync(schedule.TrainId);

        var distanceFare = await _distanceFareRepository.GetByDistanceAsync(distance, train.TrainCompanyId);
        if (distanceFare != null) return (double)distanceFare;

        throw new BadRequestException(400, "Distance fare not found.");
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

    private async Task<bool> IsTrainOccupied(int trainId, DateTime newDepartureTime)
    {
        var schedules = await _repository.GetSchedulesByTrainAsync(trainId);
        return schedules.Any(s => s.DepartureTime <= newDepartureTime && s.ArrivalTime >= newDepartureTime);
    }


    public async Task CreateSchedulesForTrainPassingAsync(Schedule largeSchedule)
    {
        if (await IsTrainOccupied(largeSchedule.TrainId, largeSchedule.DepartureTime))
        {
            // Kiểm tra xem tàu đã hoàn thành lịch trình hay chưa, nếu chưa thì không cho tạo lịch trình mới
            throw new BadRequestException(400, "Train is already occupied for another schedule.");
        }

        // Get departure and arrival stations
        var departureStation = await _trainStationRepository.GetByIdAsync(largeSchedule.DepartureStationId);
        var arrivalStation = await _trainStationRepository.GetByIdAsync(largeSchedule.ArrivalStationId);

        if (departureStation == null || arrivalStation == null)
        {
            throw new BadRequestException(400, "Departure or arrival station not found.");
        }

        // Get intermediate stations
        var intermediateStations = await _trainStationRepository
            .GetStationsFromToAsync(departureStation.CoordinateValue, arrivalStation.CoordinateValue);

        DateTime currentDepartureTime = largeSchedule.DepartureTime;

        for (int i = 0; i < intermediateStations.Count; i++)
        {
            DateTime nextDepartureTime = currentDepartureTime;

            for (int j = i + 1; j < intermediateStations.Count; j++)
            {
                var departureStationId = intermediateStations[i].Id;
                var arrivalStationId = intermediateStations[j].Id;

                // Calculate duration and update arrival time
                var duration = Math.Abs(await CalculateDurationInMinutesChild(departureStationId, arrivalStationId));
                var arrivalTime = currentDepartureTime.AddMinutes(duration);

                if (j == i + 1)
                {
                    nextDepartureTime = arrivalTime; // Set next departure time based on the first pair
                }

                // Check if a schedule already exists
                var existingSchedule =
                    await _repository.GetScheduleByStationsAsync(largeSchedule.TrainId, departureStationId,
                        arrivalStationId);

                if (existingSchedule == null || existingSchedule.DepartureTime != currentDepartureTime)
                {
                    // Create a new schedule
                    var schedule = new Schedule
                    {
                        Name = $"{largeSchedule.Name}-{i}-{j}",
                        TrainId = largeSchedule.TrainId,
                        DepartureStationId = departureStationId,
                        ArrivalStationId = arrivalStationId,
                        DepartureTime = currentDepartureTime,
                        ArrivalTime = currentDepartureTime.AddMinutes(duration),
                        Duration = duration,
                        Status = "Active",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    schedule.Price = await CalculatePrice(schedule);
                    // Add the schedule to the repository
                    await _repository.Add(schedule);
                }
            }

            // Update the current departure time for the next departure station
            currentDepartureTime = nextDepartureTime;
        }

        await _unitOfWork.SaveChangesAsync();
    }



}