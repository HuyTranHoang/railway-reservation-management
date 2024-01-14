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

    public void SoftDelete(Carriage carriage)
    {
        carriage.IsDeleted = true;
        _context.Entry(carriage).State = EntityState.Modified;
    }

    public void Update(Carriage carriage)
    {
        _context.Entry(carriage).State = EntityState.Modified;
    }

    public async Task<double> GetServiceChargeByIdAsync(int id)
    {
        var carriage = await _context.Carriages
            .Include(c => c.CarriageType)
            .Select(c => new { c.Id, c.CarriageType.ServiceCharge })
            .FirstOrDefaultAsync(c => c.Id == id);

        return carriage.ServiceCharge;
    }
}
