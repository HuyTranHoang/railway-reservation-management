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

    public void Add(Train train)
    {
        _context.Trains.Add(train);
    }

    public void Delete(Train train)
    {
        _context.Trains.Remove(train);
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
            .Include(t => t.TrainCompany)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public void SoftDelete(Train train)
    {
        train.IsDeleted = true;
        _context.Entry(train).State = EntityState.Modified;
    }

    public void Update(Train train)
    {
        _context.Entry(train).State = EntityState.Modified;
    }

    public Task<List<Train>> GetAllAsync()
    {
        return _context.Trains.ToListAsync();
    }
}