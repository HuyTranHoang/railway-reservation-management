
using Domain.Exceptions;

namespace Application.Services;

public class BookingHistoryService : IBookingHistoryService
{
    private readonly IMapper _mapper;
    private readonly IPaymentRepository _paymentRepository;
    private readonly ITicketRepository _ticketRepository;

    public BookingHistoryService(
        IMapper mapper,
        IPaymentRepository paymentRepository,
        ITicketRepository ticketRepository
    )
    {
        _mapper = mapper;
        _paymentRepository = paymentRepository;
        _ticketRepository = ticketRepository;
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

}


