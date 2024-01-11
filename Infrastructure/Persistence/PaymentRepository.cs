
using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationDbContext _context;

    public PaymentRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public void Add(Payment payment)
    {
        _context.Payments.Add(payment);
    }

    public void Delete(Payment payment)
    {
        _context.Payments.Remove(payment);
    }

    public async Task<Payment> GetByIdAsync(int id)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IQueryable<Payment>> GetQueryAsync()
    {
        return await Task.FromResult(
            _context.Payments.AsQueryable());
    }

    public async Task<IQueryable<Payment>> GetQueryWithAspNetUserAsync()
    {
        return await Task.FromResult(_context.Payments
                                .Include(p => p.AspNetUser)
                                .AsQueryable());
    }

    public void SoftDelete(Payment payment)
    {
        payment.IsDeleted = true;
        _context.Entry(payment).State = EntityState.Modified;
    }

    public void Update(Payment payment)
    {
        _context.Entry(payment).State = EntityState.Modified;
    }

    public async Task<IEnumerable<Payment>> GetAllPaymentsForDateRange(DateTime startDate, DateTime endDate)
    {
        return await _context.Payments
            .Where(p => p.CreatedAt >= startDate && p.CreatedAt < endDate)
            .Include(p => p.Tickets) 
                .ThenInclude(t => t.DistanceFare)
            .Include(p => p.Tickets)
                .ThenInclude(t => t.Carriage)
                    .ThenInclude(c => c.CarriageType)
            .Include(p => p.Tickets)
                .ThenInclude(t => t.Seat)
                    .ThenInclude(s => s.SeatType)
            .Include(p => p.Tickets)
                .ThenInclude(t => t.Cancellation)
                    .ThenInclude(c => c.CancellationRule)
            .ToListAsync();
    }

}
