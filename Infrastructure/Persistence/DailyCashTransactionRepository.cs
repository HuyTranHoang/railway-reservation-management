using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class DailyCashTransactionRepository : IDailyCashTransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public DailyCashTransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async  Task Add(DailyCashTransaction dailyCashTransaction)
        {
            await _context.DailyCashTransactions.AddAsync(dailyCashTransaction);
        }

        public async  Task Delete(DailyCashTransaction dailyCashTransaction)
        {
            _context.DailyCashTransactions.Remove(dailyCashTransaction);
            await Task.CompletedTask;
        }

        public async Task<DailyCashTransaction> GetByIdAsync(int id)
        {
            return await _context.DailyCashTransactions
            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IQueryable<DailyCashTransaction>> GetQueryAsync()
        {
            return await Task.FromResult(_context.DailyCashTransactions
            .AsQueryable());
        }

        public async Task SaveDailyCashTransaction(double totalReceived, double totalRefunded)
        {
            var dailyCashTransaction = new DailyCashTransaction
            {   
                Date = DateTime.Now,
                TotalReceived = totalReceived,
                TotalRefunded = totalRefunded
            };

            _context.DailyCashTransactions.Add(dailyCashTransaction);
            await _context.SaveChangesAsync();
        }

        public async  Task SoftDelete(DailyCashTransaction dailyCashTransaction)
        {
            dailyCashTransaction.IsDeleted = true;
            _context.Entry(dailyCashTransaction).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        public async  Task Update(DailyCashTransaction dailyCashTransaction)
        {
            _context.Entry(dailyCashTransaction).State = EntityState.Modified;
            await Task.CompletedTask;
        }
    }
}