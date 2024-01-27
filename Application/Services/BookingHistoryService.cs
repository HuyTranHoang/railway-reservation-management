
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

        var tickets = await _ticketRepository.GetByIdWithPaymentAsync(payments.Id);

        var bookingHistoryDto = new BookingHistoryDto
        {
            UpcomingTrips = new List<TicketDto>(),
            PastTrips = new List<TicketDto>(),
            Cancellations = new List<TicketDto>()
        };

        var currentDate = DateTime.Now;

        foreach (var ticket in tickets)
        {
            var ticketDto = _mapper.Map<TicketDto>(ticket);
            //vé tàu đang khởi hành
            if (ticket.Schedule.DepartureTime > currentDate)
            {
                bookingHistoryDto.UpcomingTrips.Add(ticketDto);
            }
            // vé tàu chuẩn bị khởi hành 10p
            else if (ticket.Schedule.DepartureTime > currentDate.AddMinutes(10))
            {
                bookingHistoryDto.UpcomingTrips.Add(ticketDto);
            }
            // vé đã chạy xong
            else if (ticket.Schedule.ArrivalTime < currentDate)
            {
                bookingHistoryDto.PastTrips.Add(ticketDto);
            }
            // vé đã huỷ (chưa xongg)
            if (ticket.Cancellation != null)
            {
                bookingHistoryDto.Cancellations.Add(ticketDto);
            }
        }
        return bookingHistoryDto;
    }

}


