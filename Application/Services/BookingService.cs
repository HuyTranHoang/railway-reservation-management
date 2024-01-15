
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

        public BookingService(
                                IMapper mapper,
                                ITrainCompanyRepository trainCompanyRepository,
                                IScheduleRepository scheduleRepository,
                                IDistanceFareRepository distanceFareRepository,
                                IRoundTripRepository roundTripRepository)
        {
            _mapper = mapper;
            _trainCompanyRepository = trainCompanyRepository;
            _scheduleRepository = scheduleRepository;
            _distanceFareRepository = distanceFareRepository;
            _roundTripRepository = roundTripRepository;
        }
        public async Task<PagedList<BookingDto>> GetBookingInfoAsync(BookingQueryParams queryParams)
        {
           var query = await _scheduleRepository.GetQueryWithTrainAndStationAsync();

            if (queryParams.DepartureStationId != 0)
            {
                query = query.Where(t => t.DepartureStationId == queryParams.DepartureStationId);
            }

            if (queryParams.ArrivalStationId != 0)
            {
                query = query.Where(t => t.ArrivalStationId == queryParams.ArrivalStationId);
            }

            if (queryParams.DepartureTime.HasValue)
            {
                query = query.Where(t => t.DepartureTime == queryParams.DepartureTime);
            }

            if (queryParams.ArrivalTime.HasValue)
            {
                query = query.Where(t => t.ArrivalTime == queryParams.ArrivalTime);
            }

            query = queryParams.Sort switch
            {
                "nameAsc" => query.OrderBy(t => t.Name),
                "nameDesc" => query.OrderByDescending(t => t.Name),
                _ => query.OrderBy(t => t.Train.TrainCompany.Name)
            };



            var bookingDtoQuery = query.Select(t => _mapper.Map<BookingDto>(t));
            return await PagedList<BookingDto>.CreateAsync(bookingDtoQuery, queryParams.PageNumber, queryParams.PageSize);
        
        }
    }
}