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

    public async Task Add(SeatType seatType)
    {
        await _context.SeatTypes.AddAsync(seatType);
    }

    public async Task Update(SeatType seatType)
    {
        _context.Entry(seatType).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task Delete(SeatType seatType)
    {
        _context.SeatTypes.Remove(seatType);
        await Task.CompletedTask;
    }

    public async Task SoftDelete(SeatType seatType)
    {
        seatType.IsDeleted = true;
        _context.Entry(seatType).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public Task<List<SeatType>> GetAllNoPagingAsync()
    {
        return _context.SeatTypes.ToListAsync();
    }

    public async Task<SeatType> GetSeatTypeByNameAsync(string name)
    {
        return await _context.SeatTypes
            .FirstOrDefaultAsync(x => x.Name == name);
    }
}