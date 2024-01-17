
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
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
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPassengerRepository _passengerRepository;
        private readonly ITicketRepository _ticketRepository;

        public BookingService(
                                IUnitOfWork unitOfWork,
                                IMapper mapper,
                                ITrainCompanyRepository trainCompanyRepository,
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
            _trainCompanyRepository = trainCompanyRepository;
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
        public async Task AddPassengerAsync(Passenger passenger)
        {
            _passengerRepository.Add(passenger);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddPaymentAsync(Payment payment)
        {
            _paymentRepository.Add(payment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddTicketAsync(Ticket ticket)
        {
            _ticketRepository.Add(ticket);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}