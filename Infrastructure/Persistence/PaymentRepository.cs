
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

    public void SoftDelete(Payment payment)
    {
        payment.IsDeleted = true;
        _context.Entry(payment).State = EntityState.Modified;
    }

    public void Update(Payment payment)
    {
        _context.Entry(payment).State = EntityState.Modified;
    }
}
