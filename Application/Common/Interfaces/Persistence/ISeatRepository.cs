namespace Application.Common.Interfaces.Persistence;

public interface ISeatRepository : IRepository<Seat>
{
    Task<IQueryable<Seat>> GetQueryWithSeatTypeAndCompartmentAsync();
}