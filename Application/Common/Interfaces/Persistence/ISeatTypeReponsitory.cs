using Domain.Entities;

namespace Application.Common.Interfaces.Persistence;

public interface ISeatTypeRepository
{
    Task<IQueryable<SeatType>> GetQueryAsync();
    Task<SeatType> GetByIdAsync(int id);
    void Add(SeatType seatType);
    void Update(SeatType seatType);
    void Delete(SeatType seatType);
    void SoftDelete(SeatType seatType);
}