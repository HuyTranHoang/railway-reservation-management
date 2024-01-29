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

    public async Task Add(Seat seat)
    {
        await _context.Seats.AddAsync(seat);
    }

    public async Task Delete(Seat seat)
    {
        _context.Seats.Remove(seat);
        await Task.CompletedTask;
    }

    public Task<List<Seat>> GetAllNoPagingAsync()
    {
        return _context.Seats.ToListAsync();
    }

    public async Task<Seat> GetByIdWithCompartment(int seatId)
    {
        return await _context.Seats
            .Include(s => s.Compartment)
            .FirstOrDefaultAsync(s => s.Id == seatId);
    }

    public async Task<Seat> GetByIdAsync(int id)
    {
        return await _context.Seats
            .FirstOrDefaultAsync(s => s.Id == id);
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
            .Include(s => s.Compartment)
            .Include(s => s.SeatType)
            .AsQueryable());
    }

    public async Task<double> GetServiceChargeByIdAsync(int seatId)
    {
        var seat = await _context.Seats
            .Include(s => s.SeatType)
            .Select(s => new { s.Id, s.SeatType.ServiceCharge })
            .FirstOrDefaultAsync(s => s.Id == seatId);

        return seat.ServiceCharge;
    }

    public async Task SoftDelete(Seat seat)
    {
        seat.IsDeleted = true;
        _context.Entry(seat).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task Update(Seat seat)
    {
        _context.Entry(seat).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task<List<Seat>> GetSeatsByCompartmentIdAsync(int compartmentId)
    {
        return await _context.Seats
            .Include(c => c.SeatType)
            .Include(c => c.Compartment)
            .Where(c => c.CompartmentId == compartmentId)
            .ToListAsync();
    }

    public async Task AddRangeAsync(List<Seat> seats)
    {
        await _context.Seats.AddRangeAsync(seats);
    }

    public async Task<int> GetTotalNumberOfSeatsInTrain(int trainId)
    {
        return await _context.Seats
        .Include(s => s.Compartment)
            .ThenInclude(c => c.Carriage)
                .ThenInclude(c => c.Train)
        .Where(s => s.Compartment.Carriage.Train.Id == trainId)
        .CountAsync();
    }
}