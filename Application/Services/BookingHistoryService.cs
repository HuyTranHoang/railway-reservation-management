
using Domain.Exceptions;

namespace Application.Services;

public class BookingHistoryService : IBookingHistoryService
{
    private readonly IMapper _mapper;
    private readonly IPaymentRepository _paymentRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly ICancellationRepository _cancellationRepository;
    private readonly ICancellationRuleRepository _cancellationRuleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IScheduleRepository _scheduleRepository;

    public BookingHistoryService(
        IMapper mapper,
        IPaymentRepository paymentRepository,
        ITicketRepository ticketRepository,
        ICancellationRepository cancellationRepository,
        ICancellationRuleRepository cancellationRuleRepository,
        IUnitOfWork unitOfWork,
        IScheduleRepository scheduleRepository
    )
    {
        _mapper = mapper;
        _paymentRepository = paymentRepository;
        _ticketRepository = ticketRepository;
        _cancellationRepository = cancellationRepository;
        _cancellationRuleRepository = cancellationRuleRepository;
        _unitOfWork = unitOfWork;
        _scheduleRepository = scheduleRepository;

    }

    public async Task<BookingHistoryDto> GetBookingHistoryDtoAsync(string Id)
    {
        var payments = await _paymentRepository.GetPaymentWithAspNetUserByIdStringAsync(Id);

        if (payments is null)
        {
            return new BookingHistoryDto
            {
                UpcomingTrips = new List<TicketDto>(),
                PastTrips = new List<TicketDto>(),
                Cancellations = new List<TicketDto>()
            };
        }

        var bookingHistoryDto = new BookingHistoryDto
        {
            UpcomingTrips = new List<TicketDto>(),
            PastTrips = new List<TicketDto>(),
            Cancellations = new List<TicketDto>()
        };

        foreach (var payment in payments)
        {
            var tickets = await _ticketRepository.GetByIdWithPaymentAsync(payment.Id);

            foreach (var ticket in tickets)
            {
                var isComing = ticket.Schedule.DepartureTime > DateTime.Now;
                var isPastTrip = ticket.Schedule.ArrivalTime < DateTime.Now;
                var isCancelled = ticket.Cancellation != null;

                var ticketDto = _mapper.Map<TicketDto>(ticket);

                if (isCancelled)
                {
                    bookingHistoryDto.Cancellations.Add(ticketDto);
                }
                else if (isComing)
                {
                    bookingHistoryDto.UpcomingTrips.Add(ticketDto);
                }
                else if (isPastTrip)
                {
                    bookingHistoryDto.PastTrips.Add(ticketDto);
                }

            }
        }
        return bookingHistoryDto;
    }

    public async Task CanncleTicket(Cancellation cancellation)
    {
        var today = DateTime.UtcNow;

        var ticket = await _ticketRepository.GetByIdAsync(cancellation.TicketId);
        if (ticket == null) throw new NotFoundException(nameof(Ticket), cancellation.TicketId);

        var schedule = await _scheduleRepository.GetByIdAsync(ticket.ScheduleId);
        if (schedule == null) throw new NotFoundException(nameof(Schedule), ticket.ScheduleId);

        var isCancelled = await _cancellationRepository.GetByTicketIdAsync(cancellation.TicketId);
        if (isCancelled != null) throw new BadRequestException(400, "Ticket has been cancelled");

        if (schedule.DepartureTime < today)
        {
            throw new BadRequestException(400, "Cannot cancel ticket after departure time");
        }

        var dateDifference = (schedule.DepartureTime - today).Days;

        var cancellationRule = await _cancellationRuleRepository.GetByDifferenDateAsync(dateDifference);

        cancellation.CancellationRuleId = cancellationRule.Id;

        await _cancellationRepository.Add(cancellation);
        await _unitOfWork.SaveChangesAsync();
    }

}


