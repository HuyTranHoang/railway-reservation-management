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

    public async Task Add(Carriage carriage)
    {
        await _context.Carriages.AddAsync(carriage);
    }

    public async Task Delete(Carriage carriage)
    {
        _context.Carriages.Remove(carriage);
        await Task.CompletedTask;
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

    public async Task<IQueryable<Carriage>> GetQueryWithTrainAndTypeAsync()
    {
        var query = _context.Carriages
            .Include(c => c.Train)
            .Include(c => c.CarriageType)
            .AsQueryable();

        return await Task.FromResult(query);
    }


    public async Task<Carriage> GetByIdWithCompartmentsAsync(int id)
    {
        return await _context.Carriages
            .Include(c => c.Compartments)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Carriage> GetByIdWithCarriageTypeAsync(int id)
    {
        return await _context.Carriages
            .Include(c => c.CarriageType)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task SoftDelete(Carriage carriage)
    {
        carriage.IsDeleted = true;
        _context.Entry(carriage).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task Update(Carriage carriage)
    {
        _context.Entry(carriage).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task<double> GetServiceChargeByIdAsync(int id)
    {
        var carriage = await _context.Carriages
            .Include(c => c.CarriageType)
            .Select(c => new { c.Id, c.CarriageType.ServiceCharge })
            .FirstOrDefaultAsync(c => c.Id == id);

        return carriage.ServiceCharge;
    }

    public async Task<List<Carriage>> GetCarriagesByTrainIdAsync(int trainId)
    {
        return await _context.Carriages
            .Include(c => c.Train)
            .Include(c => c.CarriageType)
            .Where(c => c.TrainId == trainId)
            .ToListAsync();
    }

    public async Task<List<Carriage>> GetAllNoPagingAsync()
    {
        return await _context.Carriages.ToListAsync();
    }
}
