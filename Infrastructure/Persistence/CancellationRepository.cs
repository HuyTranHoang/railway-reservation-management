using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class CancellationRepository : ICancellationRepository
    {
        private readonly ApplicationDbContext _context;

        public CancellationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Add(Cancellation cancellation)
        {
            await _context.Cancellations.AddAsync(cancellation);
        }

        public async Task Delete(Cancellation cancellation)
        {
            _context.Cancellations.Remove(cancellation);
            await Task.CompletedTask;
        }

        public async Task<Cancellation> GetByIdAsync(int id)
        {
            return await _context.Cancellations.FindAsync(id);
        }

        public async Task<IQueryable<Cancellation>> GetQueryAsync()
        {
            return await Task.FromResult(_context.Cancellations.AsQueryable());
        }

        public async Task<IQueryable<Cancellation>> GetQueryWithCancellationRuleAndTicketAsync()
        {
            return await Task.FromResult(
            _context.Cancellations
                .Include(t => t.Ticket)
                .Include(t => t.CancellationRule)
                .AsQueryable());
        }

        public async Task SoftDelete(Cancellation cancellation)
        {
            cancellation.IsDeleted = true;
            _context.Entry(cancellation).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        public async Task Update(Cancellation cancellation)
        {
            _context.Entry(cancellation).State = EntityState.Modified;
            await Task.CompletedTask;
        }
    }
}