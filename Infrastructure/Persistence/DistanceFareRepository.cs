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

    public void Add(DistanceFare distanceFare)
    {
        _context.DistanceFares.Add(distanceFare);
    }

    public void Delete(DistanceFare distanceFare)
    {
        _context.DistanceFares.Remove(distanceFare);
    }

    public async Task<DistanceFare> GetByIdAsync(int id)
    {
                return await _context.DistanceFares
            .FirstOrDefaultAsync(x => x.Id == id);
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

    public void SoftDelete(DistanceFare distanceFare)
    {
        distanceFare.IsDeleted = true;
        _context.Entry(distanceFare).State = EntityState.Modified;
    }

    public void Update(DistanceFare distanceFare)
    {
        _context.Entry(distanceFare).State = EntityState.Modified;
    }
}