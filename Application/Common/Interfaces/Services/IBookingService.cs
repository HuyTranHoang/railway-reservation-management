namespace Application.Common.Interfaces.Services
{
    public interface IBookingService
    {
        Task<List<ScheduleDto>> GetBookingInfoWithScheduleAsync(BookingQueryParams queryParams);
        Task<ScheduleDto> GetBookingInfoWithScheduleIdAsync(int scheduleId);
        Task<List<CarriageTypeDto>> GetAllCarriageTypeDtoAsync();
        Task<List<CarriageTypeDto>> GetCarriageTypesByTrainIdAsync(int trainId);
        Task<TrainDetailsDto> GetTrainDetailsWithTrainIdAsync(int trainId);
        Task AddPassengerAsync(Passenger passenger);
        Task AddPaymentAsync(Payment payment);
        Task AddTicketAsync(Ticket ticket);

    }
}