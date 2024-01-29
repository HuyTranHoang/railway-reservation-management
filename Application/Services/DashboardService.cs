


namespace Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IMapper _mapper;
        private readonly ITicketRepository _ticketRepository;
        private readonly ICancellationRepository _cancellationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork,
                                IMapper mapper,
                                ITicketRepository ticketRepository,
                                ICancellationRepository cancellationRepository,
                                IUserRepository userRepository,
                                IScheduleRepository scheduleRepository,
                                ISeatRepository seatRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _ticketRepository = ticketRepository;
            _cancellationRepository = cancellationRepository;
            _userRepository = userRepository;
            _scheduleRepository = scheduleRepository;
            _seatRepository = seatRepository;
        }

        public async Task<int> GetTicketCountTodayAsync()
        {
            var ticketCount = await _ticketRepository.GetTicketCountTodayAsync();
            return ticketCount;
        }

        public async Task<double> GetTicketPriceTodayAsync()
        {
            DateTime today = DateTime.UtcNow.Date;
            var ticketPrice = await _ticketRepository.GetTicketPriceSumByDateAsync(today);
            return ticketPrice;
        }

        public async Task<double> GetTicketPriceCancelTodayAsync()
        {
            DateTime today = DateTime.UtcNow.Date;
            var ticketPriceCancel = await _cancellationRepository.GetTicketPriceCancelByDateAsync(today);
            return ticketPriceCancel;
        }

        public async Task<int> GetUserCountTodayAsync()
        {
            var userCount = await _userRepository.GetUserCountTodayAsync();
            return userCount;
        }

        public async Task<double[]> GetTicketPriceSumLast7DaysAsync()
        {
            double[] ticketPriceSum = new double[7];

            for (int i = 0; i < 7; i++)
            {
                DateTime currentDate = DateTime.Today.AddDays(-i);
                double priceSum = await _ticketRepository.GetTicketPriceSumByDateAsync(currentDate);
                ticketPriceSum[i] = priceSum;
            }

            return ticketPriceSum;
        }

        public async Task<double[]> GetTicketPriceCancelSumLast7DaysAsync()
        {
            double[] ticketPriceSum = new double[7];

            for (int i = 0; i < 7; i++)
            {
                DateTime currentDate = DateTime.Today.AddDays(-i);
                double priceSum = await _cancellationRepository.GetTicketPriceCancelByDateAsync(currentDate);
                ticketPriceSum[i] = priceSum;
            }

            return ticketPriceSum;
        }

        public async Task<List<UpcomingScheduleDto>> GetUpcomingSchedulesWithSeatInfoAsync(int count)
        {
            var upcomingSchedules = await _scheduleRepository.GetUpcomingSchedules(count);
            var upcomingScheduleDtos = new List<UpcomingScheduleDto>();

            foreach (var schedule in upcomingSchedules)
            {
                var totalSeats = await _seatRepository.GetTotalNumberOfSeatsInTrain(schedule.TrainId);

                var seatsBooked = await _ticketRepository.GetSeatsBookedInSchedule(schedule.Id);

                var upcomingScheduleDto = new UpcomingScheduleDto
                {
                    ScheduleId = schedule.Id,
                    ScheduleName = schedule.Name,
                    DepartureTime = schedule.DepartureTime,
                    TotalSeats = totalSeats,
                    SeatsBooked = seatsBooked
                };

                upcomingScheduleDtos.Add(upcomingScheduleDto);
            }

            return upcomingScheduleDtos;
        }
    }
}