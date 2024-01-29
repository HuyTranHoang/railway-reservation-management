

namespace Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IMapper _mapper;
        private readonly ITicketRepository _ticketRepository;
        private readonly ICancellationRepository _cancellationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork,
                                IMapper mapper,
                                ITicketRepository ticketRepository,
                                ICancellationRepository cancellationRepository,
                                IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _ticketRepository = ticketRepository;
            _cancellationRepository = cancellationRepository;
            _userRepository = userRepository;
        }

        public async Task<int> GetTicketCountTodayAsync()
        {
            var ticketCount = await _ticketRepository.GetTicketCountTodayAsync();
            return ticketCount;
        }

        public async Task<double> GetTicketPriceTodayAsync()
        {
            var ticketPrice = await _ticketRepository.GetTicketPriceTodayAsync();
            return ticketPrice;
        }

        public async Task<double> GetTicketPriceCancelTodayAsync()
        {
            var ticketPriceCancel = await _cancellationRepository.GetTicketPriceCancelTodayAsync();
            return ticketPriceCancel;
        }

        public async Task<int> GetUserCountTodayAsync()
        {
            var userCount = await _userRepository.GetUserCountTodayAsync();
            return userCount;
        }
    }
}