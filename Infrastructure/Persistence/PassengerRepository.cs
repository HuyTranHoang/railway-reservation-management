using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class PassengerRepository : IPassengerRepository
{
    private readonly ApplicationDbContext _context;

    public PassengerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IQueryable<Passenger>> GetQueryAsync()
    {
        return await Task.FromResult(_context.Passengers
            .AsQueryable());
    }

    public async Task<Passenger> GetByIdAsync(int id)
    {
        return await _context.Passengers
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task Add(Passenger passenger)
    {
        await _context.Passengers.AddAsync(passenger);
    }

    public async Task Update(Passenger passenger)
    {
        _context.Entry(passenger).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task Delete(Passenger passenger)
    {
        _context.Passengers.Remove(passenger);
        await Task.CompletedTask;
    }

    public async Task SoftDelete(Passenger passenger)
    {
        passenger.IsDeleted = true;
        _context.Entry(passenger).State = EntityState.Modified;
        await Task.CompletedTask;
    }
}