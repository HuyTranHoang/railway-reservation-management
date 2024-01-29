namespace Application.Common.Interfaces.Services;

public interface IBookingHistoryService
{
    Task<BookingHistoryDto> GetBookingHistoryDtoAsync(string Id);

    Task CanncleTicket(Cancellation cancellation);
}
