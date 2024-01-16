using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class CarriageTypeRepository : ICarriageTypeRepository
{
    private readonly ApplicationDbContext _context;

    public CarriageTypeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IQueryable<CarriageType>> GetQueryAsync()
    {
        return await Task.FromResult(_context.CarriageTypes.AsQueryable());
    }

    public async Task<CarriageType> GetByIdAsync(int id)
    {
        return await _context.CarriageTypes.FindAsync(id);
    }

    public async Task Add(CarriageType carriageType)
    {
        _context.CarriageTypes.Add(carriageType);
        await Task.CompletedTask;
    }

    public async Task Update(CarriageType carriageType)
    {
        _context.Entry(carriageType).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task Delete(CarriageType carriageType)
    {
        _context.CarriageTypes.Remove(carriageType);
        await Task.CompletedTask;
    }

    public async Task SoftDelete(CarriageType carriageType)
    {
        carriageType.IsDeleted = true;
        _context.CarriageTypes.Update(carriageType);
        await Task.CompletedTask;
    }

    public async Task<List<CarriageType>> GetAllNoPagingAsync()
    {
        return await _context.CarriageTypes.ToListAsync();
    }
}