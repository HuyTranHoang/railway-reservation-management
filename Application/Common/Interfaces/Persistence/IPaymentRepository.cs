namespace Application.Common.Interfaces.Persistence;

public interface IPaymentRepository : IRepository<Payment>
{
    Task<IQueryable<Payment>> GetQueryWithAspNetUserAsync();
}
