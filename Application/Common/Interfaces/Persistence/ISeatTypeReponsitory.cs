using Application.Common.Models;
using Domain.Entities;

namespace Application.Common.Interfaces.Persistence;

public interface ISeatTypeRepository
{
    Task<IQueryable<SeatType>> GetQueryAsync();
    Task<SeatType> GetByIdAsync(int id);
    Task<SeatTypeDto> GetByIdDtoAsync(int id);
    void Add(SeatType seatType);
    void Remove(SeatType seatType);
    void Update(SeatType seatType);
    void RemoveById(SeatType seatType);
}