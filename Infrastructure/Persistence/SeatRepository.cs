using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence;
public class SeatRepository : ISeatRepository
{
    private readonly ApplicationDbContext _context;

    public SeatRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(Seat seat)
    {
        _context.Seats.Add(seat);
    }

    public void Delete(Seat seat)
    {
        _context.Seats.Remove(seat);
    }

    public async Task<Seat> GetByIdAsync(int id)
    {
        return await _context.Seats
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IQueryable<Seat>> GetQueryAsync()
    {
        return await Task.FromResult(_context.Seats
            .AsQueryable());
    }

    public async Task<IQueryable<Seat>> GetQueryWithSeatTypeAndCompartmentAsync()
    {
        return await Task.FromResult(
        _context.Seats
            .Include(c => c.Compartment)
            .Include(s => s.SeatType)
            .AsQueryable());
    }

    public void SoftDelete(Seat seat)
    {
        seat.IsDeleted = true;
        _context.Entry(seat).State = EntityState.Modified;
    }

    public void Update(Seat seat)
    {
        _context.Entry(seat).State = EntityState.Modified;
    }
}