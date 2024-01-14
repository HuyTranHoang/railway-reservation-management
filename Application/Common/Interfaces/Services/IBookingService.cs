namespace Application.Common.Interfaces.Services
{
    public interface IBookingService
    {
        Task<PagedList<BookingDto>> GetBookingInfoAsync(BookingQueryParams queryParams);
    }
}