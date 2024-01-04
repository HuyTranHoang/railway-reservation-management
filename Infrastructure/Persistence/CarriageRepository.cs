using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class CarriageRepository : ICarriageRepository
{
    private readonly ApplicationDbContext _context;

    public CarriageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(Carriage carriage)
    {
        _context.Carriages.Add(carriage);
    }

    public void Delete(Carriage carriage)
    {
        _context.Carriages.Remove(carriage);
    }

    public async Task<Carriage> GetByIdAsync(int id)
    {
        return await _context.Carriages
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IQueryable<Carriage>> GetQueryAsync()
    {
        return await Task.FromResult(_context.Carriages
            .AsQueryable());
    }

    public async Task<IQueryable<Carriage>> GetQueryWithTrainAsync()
    {
        return await Task.FromResult(
        _context.Carriages
            .Include(t => t.Train)
            .AsQueryable());
    }

    public void SoftDelete(Carriage carriage)
    {
        carriage.IsDeleted = true;
        _context.Entry(carriage).State = EntityState.Modified;
    }

    public void Update(Carriage carriage)
    {
        _context.Entry(carriage).State = EntityState.Modified;
    }

}
