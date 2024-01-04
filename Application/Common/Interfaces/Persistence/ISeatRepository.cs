using Domain.Entities;

namespace Application.Common.Interfaces.Persistence;

public interface ISeatRepository
{
    Task<IQueryable<Seat>> GetQueryAsync();
    Task<IQueryable<Seat>> GetQueryWithSeatTypeAndCompartmentAsync();
    Task<Seat> GetByIdAsync(int id);
    void Add(Seat seat);
    void Update(Seat seat);
    void Delete(Seat seat);
    void SoftDelete(Seat seat);
}