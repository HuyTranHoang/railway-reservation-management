using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class TrainCompanyRepository : ITrainCompanyRepository
{
    private readonly ApplicationDbContext _context;

    public TrainCompanyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Add(TrainCompany trainCompany)
    {
        _context.TrainCompanies.Add(trainCompany);
        await Task.CompletedTask;
    }

    public async Task Delete(TrainCompany trainCompany)
    {
        _context.TrainCompanies.Remove(trainCompany);
        await Task.CompletedTask;

    }

    public async Task<IQueryable<TrainCompany>> GetQueryAsync()
    {
        return await Task.FromResult(_context.TrainCompanies.AsQueryable());
    }

    public Task<List<TrainCompany>> GetAllAsync()
    {
        return _context.TrainCompanies.ToListAsync();
    }

    public async Task<TrainCompany> GetByIdAsync(int id)
    {
        return await _context.TrainCompanies
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task Update(TrainCompany trainCompany)
    {
        _context.Entry(trainCompany).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task SoftDelete(TrainCompany trainCompany)
    {
        trainCompany.IsDeleted = true;
        _context.Entry(trainCompany).State = EntityState.Modified;
        await Task.CompletedTask;
    }
}