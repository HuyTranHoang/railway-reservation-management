
using Domain.Exceptions;

namespace Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TicketService(ITicketRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task AddAsync(Ticket ticket)
        {
            ticket.Code = GenerateUniqueCode();

            ticket.Price = CalculatePrice(ticket);



            _repository.Add(ticket);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Ticket ticket)
        {
            _repository.Delete(ticket);
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
            _repository.SoftDelete(ticket);
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
            ticketInDb.Price = CalculatePrice(ticket);
            ticketInDb.Status = ticket.Status;
            ticketInDb.UpdatedAt = DateTime.Now;

            _repository.Update(ticketInDb);

            await _unitOfWork.SaveChangesAsync();
        }

        private static string GenerateUniqueCode()
        {
            Guid guid = Guid.NewGuid();
            DateTime currentDate = DateTime.Now;

            string datePart = currentDate.ToString("ddMMyy");
            string code = datePart + guid.ToString("N").Substring(0, 10 - datePart.Length);

            return code;
        }

        private double CalculatePrice (Ticket ticket)
        {
            double ticketAmount = ticket.DistanceFare.Price + ticket.Carriage.CarriageType.ServiceCharge + ticket.Seat.SeatType.ServiceCharge;

            return ticketAmount;
        }

    }
}