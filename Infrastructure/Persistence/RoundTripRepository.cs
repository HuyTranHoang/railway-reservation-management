using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class RoundTripRepository : IRoundTripRepository
    {
        private readonly ApplicationDbContext _context;

        public RoundTripRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Add(RoundTrip roundTrip)
        {
            await _context.RoundTrips.AddAsync(roundTrip);
        }

        public async Task Delete(RoundTrip roundTrip)
        {
            _context.RoundTrips.Remove(roundTrip);
            await Task.CompletedTask;
        }

        public async Task<RoundTrip> GetByIdAsync(int id)
        {
            return await _context.RoundTrips
            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<RoundTrip> GetByIdWithTrainCompanyAsync(int id)
        {
            return await _context.RoundTrips
                .Include(t => t.TrainCompany)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IQueryable<RoundTrip>> GetQueryAsync()
        {
            return await Task.FromResult(
            _context.RoundTrips.AsQueryable());
        }

        public async Task<IQueryable<RoundTrip>> GetQueryWithTrainCompanyAsync()
        {
            return await Task.FromResult(
                _context.RoundTrips
                    .Include(t => t.TrainCompany)
                    .AsQueryable());
        }

        public async Task SoftDelete(RoundTrip roundTrip)
        {
            roundTrip.IsDeleted = true;
            _context.Entry(roundTrip).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        public async Task Update(RoundTrip roundTrip)
        {
            _context.Entry(roundTrip).State = EntityState.Modified;
            await Task.CompletedTask;
        }
    }
}