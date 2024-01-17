namespace Application.Common.Interfaces.Services
{
    public interface IBookingService
    {
        Task<PagedList<ScheduleDto>> GetBookingInfoWithScheduleAsync(BookingQueryParams queryParams);
        Task<ScheduleDto> GetBookingInfoWithScheduleIdAsync(int scheduleId);
        Task<List<CarriageTypeDto>> GetAllCarriageTypeDtoAsync();
        Task<TrainDetailsDto> GetTrainDetailsWithTrainIdAsync(int trainId);

    }
}