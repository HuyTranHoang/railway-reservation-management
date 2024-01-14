using Application.Common.Interfaces.Persistence;
using Application.Common.Models;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class SeatTypeRepository : ISeatTypeRepository
{
    private readonly ApplicationDbContext _context;

    public SeatTypeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IQueryable<SeatType>> GetQueryAsync()
    {
        return await Task.FromResult(_context.SeatTypes.AsQueryable());
    }

    public async Task<SeatType> GetByIdAsync(int id)
    {
        return await _context.SeatTypes.FindAsync(id);
    }

    public void Add(SeatType seatType)
    {
        _context.SeatTypes.Add(seatType);
    }

    public void Update(SeatType seatType)
    {
        _context.Entry(seatType).State = EntityState.Modified;
    }

    public void Delete(SeatType seatType)
    {
        _context.SeatTypes.Remove(seatType);
    }

    public void SoftDelete(SeatType seatType)
    {
        seatType.IsDeleted = true;
        _context.Entry(seatType).State = EntityState.Modified;
    }

    public Task<List<SeatType>> GetAllNoPagingAsync()
    {
        return _context.SeatTypes.ToListAsync();
    }
}