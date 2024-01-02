using Domain.Entities;

namespace Application.Common.Interfaces.Persistence;

public interface IPassengerReponsitory
{
    Task<IQueryable<Passenger>> GetQueryAsync();
    Task<Passenger> GetByIdAsync(int id);
    void Add(Passenger passenger);
    void Update(Passenger passenger);
    void Delete(Passenger passenger);
    void SoftDelete(Passenger passenger);
}