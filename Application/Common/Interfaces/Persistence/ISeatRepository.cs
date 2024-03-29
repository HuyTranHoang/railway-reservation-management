namespace Application.Common.Interfaces.Persistence;

public interface ISeatRepository : IRepository<Seat>
{
    Task<IQueryable<Seat>> GetQueryWithSeatTypeAndCompartmentAsync();
    Task<List<Seat>> GetAllNoPagingAsync();
    Task<Seat> GetByIdWithCompartment(int seatId);

    Task<double> GetServiceChargeByIdAsync(int seatId);
    Task<List<Seat>> GetSeatsByCompartmentIdAsync(int compartmentId);
    Task AddRangeAsync(List<Seat> seats);
    Task<int> GetTotalNumberOfSeatsInTrain(int trainId);
}