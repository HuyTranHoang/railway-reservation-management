namespace Application.Common.Interfaces.Persistence;

public interface IPaymentRepository : IRepository<Payment>
{
    Task<IQueryable<Payment>> GetQueryWithAspNetUserAsync();
    Task<IEnumerable<Payment>> GetAllPaymentsForDateRange(DateTime startDate, DateTime endDate);
    Task<Payment> GetPaymentWithAspNetUserByIdAsync(int id);
    Task<List<Payment>> GetPaymentWithAspNetUserByIdStringAsync(string id);
}
