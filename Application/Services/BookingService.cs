
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

        public BookingService(
                                IMapper mapper,
                                ITrainCompanyRepository trainCompanyRepository,
                                IScheduleRepository scheduleRepository,
                                IDistanceFareRepository distanceFareRepository,
                                IRoundTripRepository roundTripRepository,
                                ICarriageTypeRepository carriageTypeRepository)
        {
            _mapper = mapper;
            _trainCompanyRepository = trainCompanyRepository;
            _scheduleRepository = scheduleRepository;
            _distanceFareRepository = distanceFareRepository;
            _roundTripRepository = roundTripRepository;
            _carriageTypeRepository = carriageTypeRepository;
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
            var scheduleDto = await _scheduleRepository.GetByIdAsync(scheduleId);

            return _mapper.Map<ScheduleDto>(scheduleDto);
        }

        //Lấy danh sách CarriageType và Service Charge tương ứng
        public async Task<List<CarriageTypeDto>> GetAllCarriageTypeDtoAsync()
        {
            var carriageTypes = await _carriageTypeRepository.GetQueryAsync();
            var carriageTypeDtos = _mapper.Map<List<CarriageTypeDto>>(carriageTypes);
            return carriageTypeDtos;
        }
    }
}