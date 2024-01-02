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

    public void Add(TrainCompany trainCompany)
    {
        _context.TrainCompanies.Add(trainCompany);
    }

    public void Delete(TrainCompany trainCompany)
    {
        trainCompany.IsDeleted = true;
        _context.SaveChanges();
    }

    public async Task<IQueryable<TrainCompany>> GetQueryAsync()
    {
        return await Task.FromResult(_context.TrainCompanies.AsQueryable());
    }

    public async Task<TrainCompany> GetByIdAsync(int id)
    {
        return await _context.TrainCompanies
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Update(TrainCompany trainCompany)
    {
        _context.Entry(trainCompany).State = EntityState.Modified;
    }

    public void SoftDelete(TrainCompany trainCompany)
    {
        trainCompany.IsDeleted = true;
        _context.Entry(trainCompany).State = EntityState.Modified;
    }
}