using Microsoft.Extensions.Hosting;

namespace Application.Services
{
    public class DailyCashTransactionService :  IDailyCashTransactionService
    {
        private readonly IDailyCashTransactionRepository _repository;
        private readonly IPaymentService _paymentService;
        private readonly ITicketService _ticketService;
        private readonly IScheduleService _scheduleService;
        private readonly ITrainStationService _trainStationService;
        private readonly IDistanceFareService _distanceFareService;
        private readonly ICarriageService _carriageService;
        private readonly ICarriageTypeService _carriageTypeService;
        private readonly ISeatService _seatService;
        private readonly ISeatTypeService _seatTypeService;
        private readonly ICancellationRuleService _cancellationRuleService;
        private readonly ICancellationService _cancellationService;
        private readonly IUnitOfWork _unitOfWork;
        private Timer _timer;

        public DailyCashTransactionService(IDailyCashTransactionRepository repository,
                                            IUnitOfWork unitOfWork,
                                            IPaymentService paymentService,
                                            ITicketService ticketService,
                                            ITrainStationService trainStationService, 
                                            IDistanceFareService distanceFareService, 
                                            ICarriageService carriageService, 
                                            ICarriageTypeService carriageTypeService,
                                            ISeatService seatService,
                                            ISeatTypeService seatTypeService,
                                            ICancellationRuleService cancellationRuleService,
                                            ICancellationService cancellationService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            _ticketService = ticketService;
            _trainStationService = trainStationService;
            _distanceFareService = distanceFareService;
            _carriageService = carriageService;
            _carriageTypeService = carriageTypeService;
            _seatService = seatService;
            _seatTypeService = seatTypeService;
            _cancellationRuleService = cancellationRuleService;
            _cancellationService = cancellationService;
        }

        public async Task<(double, double)> RecordDailyCashTransaction()
        {
            var currentDate = DateTime.Now;
            double totalReceived = 0;
            double totalRefunded = 0;

            var queryParams = new PaymentQueryParams
            {
                CreatedAt = currentDate
            };
            
            var payments = await _paymentService.GetAllDtoAsync(queryParams);
            foreach (var payment in payments)
            {
                var ticket = await _ticketService.GetDtoByIdAsync(payment.Id);

                // Tính trị giá khoảng cách theo lịch trình của vé
                var schedule = await _scheduleService.GetDtoByIdAsync(ticket.ScheduleId);
                var departureTrainStation = await _trainStationService.GetDtoByIdAsync(schedule.DepartureStationId);
                var arrivalTrainStation = await _trainStationService.GetDtoByIdAsync(schedule.ArrivalStationId);
                var distanceSchedule = Math.Abs(departureTrainStation.CoordinateValue - arrivalTrainStation.CoordinateValue);

                //Đối chiếu giá theo khoảng cách
                double distanceFare = await _distanceFareService.GetDtoByDistanceAsync(distanceSchedule);

                //Đối chiếu phí theo Carriage
                var carriage = await  _carriageService.GetDtoByIdAsync(ticket.CarriageIdId);
                var carriageType = await _carriageTypeService.GetDtoByIdAsync(carriage.CarriageTypeId);
                double carriageFare = carriageType.ServiceCharge;

                //Đối chiếu phí theo Seat
                var seat = await _seatService.GetDtoByIdAsync(ticket.SeatId);
                var seatType = await _seatTypeService.GetDtoByIdAsync(seat.SeatTypeId);
                double seatFare = seatType.ServiceCharge;

                //Đối chiếu phí Cancellation
                var cancellation = await _cancellationService.GetDtoByIdAsync(ticket.Id);
                var cancellationRule = await _cancellationRuleService.GetDtoByIdAsync(cancellation.CancellationRuleId);
                double cancellationFee = cancellationRule.Fee;

                //Tính TotalAmount của 1 Ticket
                double totalAmount = distanceFare + carriageFare + seatFare;

                //Tính totalReceived
                totalReceived += totalAmount;

                //Tính totalRefunded
                totalRefunded += cancellationFee;
            }

            return (totalReceived, totalRefunded);
        
        }

        public Task<PagedList<DailyCashTransactionDto>> GetAllDtoAsync(QueryParams queryParams)
        {
            throw new NotImplementedException();
        }

        public Task<DailyCashTransaction> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(DailyCashTransaction t)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(DailyCashTransaction t)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(DailyCashTransaction t)
        {
            throw new NotImplementedException();
        }

        public Task SoftDeleteAsync(DailyCashTransaction t)
        {
            throw new NotImplementedException();
        }
    }
}