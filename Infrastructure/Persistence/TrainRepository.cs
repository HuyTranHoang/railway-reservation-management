using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class TrainRepository : ITrainRepository
{
    private readonly ApplicationDbContext _context;

    public TrainRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Add(Train train)
    {
        _context.Trains.Add(train);
        await Task.CompletedTask;
    }

    public async Task Delete(Train train)
    {
        _context.Trains.Remove(train);
        await Task.CompletedTask;
    }

    public async Task<IQueryable<Train>> GetQueryAsync()
    {
        return await Task.FromResult(
            _context.Trains.AsQueryable());
    }

    public async Task<IQueryable<Train>> GetQueryWithTrainCompanyAsync()
    {
        return await Task.FromResult(
            _context.Trains
                .Include(t => t.TrainCompany)
                .AsQueryable());
    }

    public async Task<Train> GetByIdAsync(int id)
    {
        return await _context.Trains
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task SoftDelete(Train train)
    {
        train.IsDeleted = true;
        _context.Entry(train).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task Update(Train train)
    {
        _context.Entry(train).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public Task<List<Train>> GetAllAsync()
    {
        return _context.Trains.ToListAsync();
    }
}