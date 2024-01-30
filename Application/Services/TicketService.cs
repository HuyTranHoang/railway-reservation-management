using Domain.Exceptions;

namespace Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IDistanceFareRepository _distanceFare;
        private readonly ICarriageRepository _carriage;
        private readonly ISeatRepository _seat;
        private readonly ICancellationRepository _cancellationRepository;
        private readonly ITrainStationRepository _trainStation;
        private readonly IScheduleRepository _schedule;
        private readonly IRoundTripRepository _roundTrip;
        private readonly ITrainRepository _train;

        public TicketService(ITicketRepository repository, IUnitOfWork unitOfWork, IMapper mapper,
            IDistanceFareRepository distanceFare,
            ICarriageRepository carriage,
            ISeatRepository seat,
            ICancellationRepository cancellationRepository,
            ITrainStationRepository trainStation,
            IScheduleRepository schedule,
            IRoundTripRepository roundTrip,
            ITrainRepository train)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _distanceFare = distanceFare;
            _carriage = carriage;
            _seat = seat;
            _cancellationRepository = cancellationRepository;
            _trainStation = trainStation;
            _schedule = schedule;
            _roundTrip = roundTrip;
            _train = train;
        }

        public async Task AddAsync(Ticket ticket)
        {
            var tickets = await _repository.GetAllNoPagingAsync();

            ticket.Code = GenerateUniqueCode(ticket);

            int distanceFareId = await CaculateDistanceFareId(ticket);
            ticket.DistanceFareId = distanceFareId;

            ticket.Price = await CalculatePrice(ticket);

            bool isSeatAndScheduleExistsInTickets =
                tickets.Any(t => t.SeatId == ticket.SeatId
                                 && t.ScheduleId == ticket.ScheduleId
                                 && !_cancellationRepository.IsTicketCancelledAsync(t.Id).Result);

            if (isSeatAndScheduleExistsInTickets)
            {
                throw new BadRequestException(400, "Seat has been booked");
            }

            await _repository.Add(ticket);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddRoundTripAsync(Ticket ticket)
        {
            var tickets = await _repository.GetAllNoPagingAsync();

            ticket.Code = GenerateUniqueCode(ticket);

            int distanceFareId = await CaculateDistanceFareId(ticket);
            ticket.DistanceFareId = distanceFareId;

            ticket.Price = await CalculatePrice(ticket, true);

            bool isSeatAndScheduleExistsInTickets =
                tickets.Any(t => t.SeatId == ticket.SeatId
                                 && t.ScheduleId == ticket.ScheduleId
                                 && !_cancellationRepository.IsTicketCancelledAsync(t.Id).Result);

            if (isSeatAndScheduleExistsInTickets)
            {
                throw new BadRequestException(400, "Seat has been booked");
            }

            await _repository.Add(ticket);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Ticket ticket)
        {
            await _repository.Delete(ticket);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PagedList<TicketDto>> GetAllDtoAsync(TicketQueryParams queryParams)
        {
            var query = await _repository.GetQueryWithRelationshipTableAsync();

            if (queryParams.CreatedAt != DateTime.MinValue)
            {
                query = query.Where(t => t.CreatedAt.Date == queryParams.CreatedAt.Date);
            }

            if (queryParams.PassengerId != 0)
            {
                query = query.Where(t => t.PassengerId == queryParams.PassengerId);
            }

            if (queryParams.TrainId != 0)
            {
                query = query.Where(t => t.TrainId == queryParams.TrainId);
            }

            if (queryParams.DistanceFareId != 0)
            {
                query = query.Where(t => t.DistanceFareId == queryParams.DistanceFareId);
            }

            if (queryParams.CarriageId != 0)
            {
                query = query.Where(t => t.CarriageId == queryParams.CarriageId);
            }

            if (queryParams.SeatId != 0)
            {
                query = query.Where(t => t.SeatId == queryParams.SeatId);
            }

            if (queryParams.ScheduleId != 0)
            {
                query = query.Where(t => t.ScheduleId == queryParams.ScheduleId);
            }

            if (queryParams.PaymentId != 0)
            {
                query = query.Where(t => t.PaymentId == queryParams.PaymentId);
            }

            if (!string.IsNullOrEmpty(queryParams.SearchTerm))
                query = query.Where(p => p.Passenger.FullName.Contains(queryParams.SearchTerm.Trim()));

            query = queryParams.Sort switch
            {
                "PassengerNameAsc" => query.OrderBy(p => p.Passenger.FullName),
                "PassengerNameDesc" => query.OrderByDescending(p => p.Passenger.FullName),
                "trainNameAsc" => query.OrderBy(p => p.Train.Name),
                "trainNameDesc" => query.OrderByDescending(p => p.Train.Name),
                "carriageNameAsc" => query.OrderBy(p => p.Carriage.Name),
                "carriageNameDesc" => query.OrderByDescending(p => p.Carriage.Name),
                "seatNameAsc" => query.OrderBy(p => p.Seat.Name),
                "seatNameDesc" => query.OrderByDescending(p => p.Seat.Name),
                "scheduleNameAsc" => query.OrderBy(p => p.Schedule.Name),
                "scheduleNameDesc" => query.OrderByDescending(p => p.Schedule.Name),
                _ => query.OrderBy(p => p.CreatedAt)
            };

            var ticketDtoQuery = query.Select(p => _mapper.Map<TicketDto>(p));

            return await PagedList<TicketDto>.CreateAsync(ticketDtoQuery, queryParams.PageNumber,
                queryParams.PageSize);
        }

        public async Task<Ticket> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<TicketDto> GetDtoByIdAsync(int id)
        {
            var ticket = await _repository.GetByIdAsync(id);
            return _mapper.Map<TicketDto>(ticket);
        }

        public async Task SoftDeleteAsync(Ticket ticket)
        {
            await _repository.SoftDelete(ticket);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Ticket ticket)
        {
            var ticketInDb = await _repository.GetByIdAsync(ticket.Id);

            if (ticketInDb == null) throw new NotFoundException(nameof(Ticket), ticket.Id);

            ticketInDb.PassengerId = ticket.PassengerId;
            ticketInDb.TrainId = ticket.TrainId;
            ticketInDb.CarriageId = ticket.CarriageId;
            ticketInDb.DistanceFareId = ticket.DistanceFareId;
            ticketInDb.SeatId = ticket.SeatId;
            ticketInDb.ScheduleId = ticket.ScheduleId;
            ticketInDb.PaymentId = ticket.PaymentId;
            ticketInDb.Price = await CalculatePrice(ticket);
            ticketInDb.Status = ticket.Status;
            ticketInDb.UpdatedAt = DateTime.Now;

            await _repository.Update(ticketInDb);

            await _unitOfWork.SaveChangesAsync();
        }

        private static string GenerateUniqueCode(Ticket ticket)
        {
            DateTime currentDate = DateTime.Now;
            string datePart = currentDate.ToString("ddMM");
            string timePart = currentDate.ToString("HHmmss");
            var code = $"{datePart}{timePart}{ticket.PassengerId}";
            return code;
        }

        private async Task<double> CalculatePrice(Ticket ticket, bool isRoundTrip = false)
        {
            var distanceFare = await _distanceFare.GetDistanceFareByIdAsync(ticket.DistanceFareId);
            var carriageServiceCharge = await _carriage.GetServiceChargeByIdAsync(ticket.CarriageId);
            var seatTypeServiceCharge = await _seat.GetServiceChargeByIdAsync(ticket.SeatId);

            double ticketAmount = distanceFare + carriageServiceCharge + seatTypeServiceCharge;

            if (isRoundTrip)
            {
                var trainCompany = await _train.GetByIdAsync(ticket.TrainId);
                var roundTrip = await _roundTrip.GetByTrainCompanyIdAsync(trainCompany.TrainCompanyId);
                var discountAmount = ticketAmount * roundTrip.Discount / 100;
                ticketAmount -= discountAmount;
            }
            return ticketAmount;
        }

        public async Task<List<TicketDto>> GetAllDtoNoPagingAsync()
        {
            var tickets = await _repository.GetAllNoPagingAsync();
            return _mapper.Map<List<TicketDto>>(tickets);
        }

        private async Task<int> CaculateDistanceFareId(Ticket ticket)
        {
            var schedule = await _schedule.GetByIdAsync(ticket.ScheduleId);
            var train = await _train.GetByIdAsync(schedule.TrainId);
            var trainCompanyId = train.TrainCompanyId;

            var departureStation = await _trainStation.GetByIdAsync(schedule.DepartureStationId);
            var departureValue = departureStation.CoordinateValue;
            var arrivalStation = await _trainStation.GetByIdAsync(schedule.ArrivalStationId);
            var arrivalValue = arrivalStation.CoordinateValue;
            var distance = Math.Abs(departureValue - arrivalValue);

            var distanceFareId = await _distanceFare.GetIdByDistanceAndTrainCompanyAsync(distance, trainCompanyId);

            return distanceFareId;
        }
    }
}