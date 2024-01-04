namespace Application.Common.Interfaces.Persistence;

public interface ISeatRepository : IReponsitory<Seat>
{
    Task<IQueryable<Seat>> GetQueryWithSeatTypeAndCompartmentAsync();
}