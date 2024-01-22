using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IDistanceFareRepository _distanceFareRepository;
        private readonly IRoundTripRepository _roundTripRepository;
        private readonly ICarriageTypeRepository _carriageTypeRepository;
        private readonly ITrainRepository _trainRepository;
        private readonly ICarriageRepository _carriageRepository;
        private readonly ICompartmentRepository _compartmentRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPassengerRepository _passengerRepository;
        private readonly ITicketRepository _ticketRepository;

        public BookingService(
                                IUnitOfWork unitOfWork,
                                IMapper mapper,
                                IScheduleRepository scheduleRepository,
                                IDistanceFareRepository distanceFareRepository,
                                IRoundTripRepository roundTripRepository,
                                ICarriageTypeRepository carriageTypeRepository,
                                ITrainRepository trainRepository,
                                ICarriageRepository carriageRepository,
                                ICompartmentRepository compartmentRepository,
                                ISeatRepository seatRepository,
                                IPaymentRepository paymentRepository,
                                IPassengerRepository passengerRepository,
                                ITicketRepository ticketRepository
                                )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _scheduleRepository = scheduleRepository;
            _distanceFareRepository = distanceFareRepository;
            _roundTripRepository = roundTripRepository;
            _carriageTypeRepository = carriageTypeRepository;
            _trainRepository = trainRepository;
            _carriageRepository = carriageRepository;
            _compartmentRepository = compartmentRepository;
            _seatRepository = seatRepository;
            _paymentRepository = paymentRepository;
            _passengerRepository = passengerRepository;
            _ticketRepository = ticketRepository;
        }

        //Lấy danh sách các lịch trình kèm filter
        public async Task<List<BookingDto>> GetBookingInfoWithScheduleAsync(BookingQueryParams queryParams)
        {
           var query = await _scheduleRepository.GetQueryWithTrainAndStationAsync();

            query = query
                .Where(t =>
                    (queryParams.DepartureStationId == 0 || t.DepartureStationId == queryParams.DepartureStationId) &&
                    (queryParams.ArrivalStationId == 0 || t.ArrivalStationId == queryParams.ArrivalStationId) &&
                    (!queryParams.DepartureTime.HasValue || t.DepartureTime.Date == queryParams.DepartureTime.Value.Date) &&
                    (!queryParams.ArrivalTime.HasValue || t.ArrivalTime.Date == queryParams.ArrivalTime.Value.Date)
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

            var bookingDtoQuery = await query.Select(t => new BookingDto
            {
                Id = t.Id,
                Name = t.Name,
                TrainId = t.TrainId,
                TrainName = t.Train.Name,
                TrainCompanyName = t.Train.TrainCompany.Name,
                TrainCompanyLogo = t.Train.TrainCompany.Logo,
                DepartureStationId = t.DepartureStationId,
                DepartureStationName = t.DepartureStation.Name,
                DepartureStationAddress = t.DepartureStation.Address,
                ArrivalStationId = t.ArrivalStationId,
                ArrivalStationName = t.ArrivalStation.Name,
                ArrivalStationAddress = t.ArrivalStation.Address,
                DepartureTime = t.DepartureTime,
                ArrivalTime = t.ArrivalTime,
                Duration = t.Duration,
                Price = t.Price,
                RoundTrip = queryParams.RoundTrip,
                Status = t.Status
            }).ToListAsync();

            foreach (var bookingDto in bookingDtoQuery)
            {
                bookingDto.CarriageTypes = await GetCarriageTypesByTrainIdAsync(bookingDto.TrainId);
            }

            return bookingDtoQuery;

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

        public async Task<TrainDetailsDto> GetTrainDetailsWithTrainIdAsync(int trainId)
        {
            var train = await _trainRepository.GetByIdAsync(trainId);
            var carriages = await _carriageRepository.GetCarriagesByTrainIdAsync(trainId);

            var trainDto = _mapper.Map<TrainDto>(train);
            var carriageDtos = _mapper.Map<List<CarriageDto>>(carriages);

            var trainDetailsDto = new TrainDetailsDto
            {
                Train = trainDto,
                Carriages = carriageDtos,
                Compartments = new List<CompartmentDto>(),
                Seats = new List<SeatDto>()
            };

            foreach (var carriage in carriages)
            {
                var compartments = await _compartmentRepository.GetCompartmentsByCarriageIdAsync(carriage.Id);
                var compartmentDtos = _mapper.Map<List<CompartmentDto>>(compartments);
                trainDetailsDto.Compartments.AddRange(compartmentDtos);

                foreach (var compartment in compartments)
                {
                    var seats = await _seatRepository.GetSeatsByCompartmentIdAsync(compartment.Id);
                    var seatDtos = _mapper.Map<List<SeatDto>>(seats);
                    trainDetailsDto.Seats.AddRange(seatDtos);
                }
            }

            return trainDetailsDto;
        }

        //Noted:
        //Sau khi chọn ghế sẽ chuyển sang step tiếp theo là nhập thông tin hành khách,
        //Bao nhiêu ghế được chọn sẽ hiện ra bây nhiêu form nhập thông tin
            //Sau khi nhập thông tin sẽ được lưu vào biến tạm
        //=> Chuyển sang bước thanh toán thì hiện thị lại thông tin đã được chọn và thông tin hành khách từ biến tạm
            //Thanh toán xong thì mới thực hiện hàm add Passenger, Payment, Ticket
        public async Task<PassengerDto> AddPassengerAsync(Passenger passenger)
        {
            await _passengerRepository.Add(passenger);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<PassengerDto>(passenger);
        }

        public async Task<PaymentDto> AddPaymentAsync(Payment payment)
        {
            await _paymentRepository.Add(payment);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<PaymentDto>(payment);

        }

        public async Task AddTicketAsync(Ticket ticket)
        {
            await _ticketRepository.Add(ticket);
            await _unitOfWork.SaveChangesAsync();
        }

    public async Task<List<CarriageTypeDto>> GetCarriageTypesByTrainIdAsync(int trainId)
    {
        var carriageTypes = await _carriageTypeRepository.GetCarriageTypeByTrainIdAsync(trainId);
        return _mapper.Map<List<CarriageTypeDto>>(carriageTypes);
    }

}