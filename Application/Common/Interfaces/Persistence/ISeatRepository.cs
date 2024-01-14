namespace Application.Common.Interfaces.Persistence;

public interface ISeatRepository : IRepository<Seat>
{
    Task<IQueryable<Seat>> GetQueryWithSeatTypeAndCompartmentAsync();

    Task<List<Seat>> GetAllNoPagingAsync();

    Task<double> GetServiceChargeByIdAsync(int seatId);
}