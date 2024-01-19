using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class CompartmentRepository : ICompartmentRepository
{
    private readonly ApplicationDbContext _context;

    public CompartmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IQueryable<Compartment>> GetQueryAsync()
    {
        return await Task.FromResult(_context.Compartments.AsQueryable());
    }

    public async Task<Compartment> GetByIdAsync(int id)
    {
        return await _context.Compartments.FindAsync(id);
    }

    public async Task Add(Compartment compartment)
    {
        await _context.Compartments.AddAsync(compartment);
    }

    public async Task Update(Compartment compartment)
    {
        _context.Entry(compartment).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task Delete(Compartment compartment)
    {
        _context.Compartments.Remove(compartment);
        await Task.CompletedTask;
    }

    public async Task SoftDelete(Compartment compartment)
    {
        compartment.IsDeleted = true;
        _context.Entry(compartment).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task<IQueryable<Compartment>> GetQueryWithCarriageAsync()
    {
        return await Task.FromResult(_context.Compartments.Include(c => c.Carriage).AsQueryable());
    }

    public async Task<IQueryable<Compartment>> GetQueryWithCarriageAndTrainAsync()
    {
        return await Task.FromResult(
            _context.Compartments
                .Include(c => c.Carriage)
                .ThenInclude(c => c.Train)
                .AsQueryable());
    }

    public async Task<Compartment> GetByIdWithSeatsAsync(int id)
    {
        return await _context.Compartments
            .Include(c => c.Seats)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Compartment>> GetCompartmentsByCarriageIdAsync(int carriageId)
    {
        return await _context.Compartments
            .Include(c => c.Carriage)
            .Where(c => c.CarriageId == carriageId)
            .ToListAsync();
    }

    public Task<List<Compartment>> GetAllNoPagingAsync()
    {
        return _context.Compartments.ToListAsync();
    }

    public async Task AddRangeAsync(List<Compartment> compartments)
    {
        await _context.Compartments.AddRangeAsync(compartments);
    }
}
