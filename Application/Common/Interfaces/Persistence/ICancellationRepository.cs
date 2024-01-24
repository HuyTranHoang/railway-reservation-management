namespace Application.Common.Interfaces.Persistence
{
    public interface ICancellationRepository : IRepository<Cancellation>
    {
         Task<IQueryable<Cancellation>> GetQueryWithCancellationRuleAndTicketAsync();
         Task<Cancellation> GetByTicketIdAsync(int ticketId);
    }
}