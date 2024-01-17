namespace Application.Common.Interfaces.Persistence;

public interface ISeatRepository : IRepository<Seat>
{
    Task<IQueryable<Seat>> GetQueryWithSeatTypeAndCompartmentAsync();
    Task<double> GetServiceChargeByIdAsync(int seatId);
    Task<List<Seat>> GetSeatsByCompartmentIdAsync(int compartmentId);
}