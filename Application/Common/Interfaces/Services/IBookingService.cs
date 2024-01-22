namespace Application.Common.Interfaces.Services
{
    public interface IBookingService
    {
        Task<List<BookingDto>> GetBookingInfoWithScheduleAsync(BookingQueryParams queryParams);
        Task<ScheduleDto> GetBookingInfoWithScheduleIdAsync(int scheduleId);
        Task<List<CarriageTypeDto>> GetAllCarriageTypeDtoAsync();
        Task<List<CarriageTypeDto>> GetCarriageTypesByTrainIdAsync(int trainId);
        Task<TrainDetailsDto> GetTrainDetailsWithTrainIdAsync(int trainId);
        Task<PassengerDto> AddPassengerAsync(Passenger passenger);
        Task<PaymentDto> AddPaymentAsync(Payment payment);
        Task<TicketDto> AddTicketAsync(Ticket ticket);
        Task<List<TicketDto>> AddTicketListAsync(List<Ticket> tickets);
    }
}