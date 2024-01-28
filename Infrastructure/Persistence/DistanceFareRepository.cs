using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class DistanceFareRepository : IDistanceFareRepository
{
    private readonly ApplicationDbContext _context;

    public DistanceFareRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Add(DistanceFare distanceFare)
    {
        await _context.DistanceFares.AddAsync(distanceFare);
    }

    public async Task Delete(DistanceFare distanceFare)
    {
        _context.DistanceFares.Remove(distanceFare);
        await Task.CompletedTask;
    }

    public async Task<decimal?> GetByDistanceAsync(int distance, int trainCompanyId)
    {
        var fare = await _context.DistanceFares
                            .Where(d => d.Distance <= distance && d.TrainCompanyId == trainCompanyId)
                            .OrderByDescending(d => d.Distance)
                            .Select(d => d.Price)
                            .FirstOrDefaultAsync();

        return (decimal?)fare;
    }

    public async Task<DistanceFare> GetByIdAsync(int id)
    {
        return await _context.DistanceFares
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<double> GetDistanceFareByIdAsync(int id)
    {
        var distanceFare = await _context.DistanceFares
            .Where(d => d.Id == id)
            .Select(d => d.Price)
            .FirstOrDefaultAsync();

        return distanceFare;
    }

    public async Task<int> GetIdByDistanceAndTrainCompanyAsync(int distance, int trainCompanyId)
    {
        var distanceFareId = await _context.DistanceFares
                            .Where(d => d.Distance <= distance && d.TrainCompanyId == trainCompanyId)
                            .OrderByDescending(d => d.Distance)
                            .Select(d => d.Id)
                            .FirstOrDefaultAsync();

        return distanceFareId;
    }

    public async Task<IQueryable<DistanceFare>> GetQueryAsync()
    {
        return await Task.FromResult(
            _context.DistanceFares.AsQueryable());
    }

    public async Task<IQueryable<DistanceFare>> GetQueryWithTrainCompanyAsync()
    {
        return await Task.FromResult(
            _context.DistanceFares
                .Include(t => t.TrainCompany)
                .AsQueryable());
    }

    public async Task SoftDelete(DistanceFare distanceFare)
    {
        distanceFare.IsDeleted = true;
        _context.Entry(distanceFare).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task Update(DistanceFare distanceFare)
    {
        _context.Entry(distanceFare).State = EntityState.Modified;
        await Task.CompletedTask;
    }
}