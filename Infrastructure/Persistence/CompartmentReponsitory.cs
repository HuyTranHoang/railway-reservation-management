using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class CompartmentReponsitory : ICompartmentReponsitory
{
    private readonly ApplicationDbContext _context;

    public CompartmentReponsitory(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(Compartment compartment)
    {
        _context.Compartments.Add(compartment);
    }

    public void Delete(Compartment compartment)
    {
        _context.Compartments.Remove(compartment);
    }

    public async Task<Compartment> GetByIdAsync(int id)
    {
        return await _context.Compartments.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IQueryable<Compartment>> GetQueryAsync()
    {
        return await Task.FromResult(_context.Compartments
        .AsQueryable());
    }

    public async Task<IQueryable<Compartment>> GetQueryWithCarriageAsync()
    {
        return await Task.FromResult(
        _context.Compartments
         .Include(c => c.Carriage)
         .AsQueryable());
    }

    public void SoftDelete(Compartment compartment)
    {
        compartment.IsDeleted = true;
        _context.Entry(compartment).State = EntityState.Modified;
    }

    public void Update(Compartment compartment)
    {
        _context.Entry(compartment).State = EntityState.Modified;
    }
}
