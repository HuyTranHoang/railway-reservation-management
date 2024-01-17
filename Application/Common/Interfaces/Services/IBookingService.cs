namespace Application.Common.Interfaces.Services
{
    public interface IBookingService
    {
        Task<PagedList<ScheduleDto>> GetBookingInfoWithScheduleAsync(BookingQueryParams queryParams);
        Task<ScheduleDto> GetBookingInfoWithScheduleIdAsync(int scheduleId);
        Task<List<CarriageTypeDto>> GetAllCarriageTypeDtoAsync();
        Task<TrainDto> GetTrainInfoWithTrainIdAsync(int trainId);
        Task<List<CarriageDto>> GetCarriagesWithTrainIdAsync(int trainId);
        Task<List<CompartmentDto>> GetCompartmentsWithCarriageIdAsync(int carriageId);
        Task<List<SeatDto>> GetSeatsWithCompartmentIdAsync(int compartmentId);

    }
}