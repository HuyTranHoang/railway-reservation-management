using Microsoft.Extensions.Hosting;

namespace Application.Services
{
    public class DailyCashTransactionService : IHostedService, IDisposable
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
        private readonly IUnitOfWork _unitOfWork;
        private Timer _timer;

        public DailyCashTransactionService(IDailyCashTransactionRepository repository,
                                            IUnitOfWork unitOfWork,
                                            Timer timer,
                                            IPaymentService paymentService,
                                            ITicketService ticketService,
                                            ITrainStationService trainStationService, 
                                            IDistanceFareService distanceFareService, 
                                            ICarriageService carriageService, 
                                            ICarriageTypeService carriageTypeService,
                                            ISeatService seatService,
                                            ISeatTypeService seatTypeService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _timer = timer;
            _paymentService = paymentService;
            _ticketService = ticketService;
            _trainStationService = trainStationService;
            _distanceFareService = distanceFareService;
            _carriageService = carriageService;
            _carriageTypeService = carriageTypeService;
            _seatService = seatService;
            _seatTypeService = seatTypeService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            RecordDailyCashTransaction();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private async void RecordDailyCashTransaction()
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
                var distance = await _distanceFareService.GetDtoByDistanceAsync(ticket.DistanceFareId);
                var distanceFare = distance.Price;

                //Đối chiếu phí theo Carriage
                var carriage = await  _carriageService.GetDtoByIdAsync(ticket.CarriageIdId);
                var carriageType = await _carriageTypeService.GetDtoByIdAsync(carriage.CarriageTypeId);
                var carriageFare = carriageType.ServiceCharge;

                //Đối chiếu phí theo Seat
                var seat = await _seatService.GetDtoByIdAsync(ticket.SeatId);
                var seatType = await _seatTypeService.GetDtoByIdAsync(seat.SeatTypeId);
                var seatFare = seatType.ServiceCharge;

                //Đối chiếu phí Cancellation
                var cancellation = await 

                //totalAmount
                totalReceived = distanceFare + carriageFare + seatFare;

            }
        
        }

    }
}