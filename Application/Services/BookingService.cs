
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IMapper _mapper;
        private readonly ITrainCompanyRepository _trainCompanyRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IDistanceFareRepository _distanceFareRepository;
        private readonly IRoundTripRepository _roundTripRepository;
        private readonly ICarriageTypeRepository _carriageTypeRepository;
        private readonly ITrainRepository _trainRepository;
        private readonly ICarriageRepository _carriageRepository;
        private readonly ICompartmentRepository _compartmentRepository;
        private readonly ISeatRepository _seatRepository;

        public BookingService(
                                IMapper mapper,
                                ITrainCompanyRepository trainCompanyRepository,
                                IScheduleRepository scheduleRepository,
                                IDistanceFareRepository distanceFareRepository,
                                IRoundTripRepository roundTripRepository,
                                ICarriageTypeRepository carriageTypeRepository,
                                ITrainRepository trainRepository,
                                ICarriageRepository carriageRepository,
                                ICompartmentRepository compartmentRepository,
                                ISeatRepository seatRepository)
        {
            _mapper = mapper;
            _trainCompanyRepository = trainCompanyRepository;
            _scheduleRepository = scheduleRepository;
            _distanceFareRepository = distanceFareRepository;
            _roundTripRepository = roundTripRepository;
            _carriageTypeRepository = carriageTypeRepository;
            _trainRepository = trainRepository;
            _carriageRepository = carriageRepository;
            _compartmentRepository = compartmentRepository;
            _seatRepository = seatRepository;
        }

        //Lấy danh sách các lịch trình kèm filter
        public async Task<PagedList<ScheduleDto>> GetBookingInfoWithScheduleAsync(BookingQueryParams queryParams)
        {
           var query = await _scheduleRepository.GetQueryWithTrainAndStationAsync();

            query = query
                .Where(t =>
                    (queryParams.DepartureStationId == 0 || t.DepartureStationId == queryParams.DepartureStationId) &&
                    (queryParams.ArrivalStationId == 0 || t.ArrivalStationId == queryParams.ArrivalStationId) &&
                    (!queryParams.DepartureTime.HasValue || t.DepartureTime == queryParams.DepartureTime.Value) &&
                    (!queryParams.ArrivalTime.HasValue || t.ArrivalTime == queryParams.ArrivalTime.Value)
                );

            query = queryParams.Sort switch
            {
                "nameAsc" => query.OrderBy(t => t.Name),
                "nameDesc" => query.OrderByDescending(t => t.Name),
                "departureTimeAsc" => query.OrderBy(t => t.DepartureTime),
                "departureTimeDesc" => query.OrderByDescending(t => t.DepartureTime),
                "arrivalTimeAsc" => query.OrderBy(t => t.ArrivalTime),
                "arrivalTimeDesc" => query.OrderByDescending(t => t.ArrivalTime),
                "priceTimeAsc" => query.OrderBy(t => t.Price),
                "priceTimeDesc" => query.OrderByDescending(t => t.Price),
                _ => query.OrderBy(t => t.Train.TrainCompany.Name)
            };



            var bookingDtoQuery = query.Select(t => _mapper.Map<ScheduleDto>(t));
            return await PagedList<ScheduleDto>.CreateAsync(bookingDtoQuery, queryParams.PageNumber, queryParams.PageSize);
        
        }

        //Lấy lịch trình theo Id
        public async Task<ScheduleDto> GetBookingInfoWithScheduleIdAsync(int scheduleId)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(scheduleId);

            return _mapper.Map<ScheduleDto>(schedule);
        }

        //Lấy danh sách CarriageType và Service Charge tương ứng
        public async Task<List<CarriageTypeDto>> GetAllCarriageTypeDtoAsync()
        {
            var carriageTypes = await _carriageTypeRepository.GetQueryAsync();
            var carriageTypeDtos = _mapper.Map<List<CarriageTypeDto>>(carriageTypes);
            return carriageTypeDtos;
        }

        //Lấy thông tin Train từ Schedule, truyền train_id từ Schedule vô
        public async Task<TrainDto> GetTrainInfoWithTrainIdAsync(int trainId)
        {
            var train = await _trainRepository.GetByIdAsync(trainId);

            return _mapper.Map<TrainDto>(train);
        }

        //Lấy thông tin các Carriage từ Train, truyền carriage_id từ Train vô
        public async Task<List<CarriageDto>> GetCarriagesWithTrainIdAsync(int trainId)
        {
            var carriages = await _carriageRepository.GetCarriagesByTrainIdAsync(trainId);

            return _mapper.Map<List<CarriageDto>>(carriages);
        }

        //Lấy thông tin các Compartment từ Carriage, truyền compartment_id từ Carriage vô
        public async Task<List<CompartmentDto>> GetCompartmentsWithCarriageIdAsync(int carriageId)
        {
            var compartments = await _compartmentRepository.GetCompartmentsByCarriageIdAsync(carriageId);

            return _mapper.Map<List<CompartmentDto>>(compartments);
        }

        //Lấy thông tin các Seat từ Compartment, truyền seat_id từ Compartment vô
        public async Task<List<SeatDto>> GetSeatsWithCompartmentIdAsync(int compartmentId)
        {
            var seats = await _seatRepository.GetSeatsByCompartmentIdAsync(compartmentId);

            return _mapper.Map<List<SeatDto>>(seats);
        }

    }
}