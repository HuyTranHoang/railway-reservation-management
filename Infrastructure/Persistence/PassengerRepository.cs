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

    public void Add(Passenger passenger)
    {
        _context.Passengers.Add(passenger);
    }

    public void Update(Passenger passenger)
    {
        _context.Entry(passenger).State = EntityState.Modified;
    }

    public void Delete(Passenger passenger)
    {
        _context.Passengers.Remove(passenger);
    }

    public void SoftDelete(Passenger passenger)
    {
        passenger.IsDeleted = true;
        _context.Entry(passenger).State = EntityState.Modified;
    }
}