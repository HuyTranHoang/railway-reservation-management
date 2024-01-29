namespace Application.Common.Interfaces.Persistence
{
    public interface ICancellationRepository : IRepository<Cancellation>
    {
         Task<IQueryable<Cancellation>> GetQueryWithCancellationRuleAndTicketAsync();
         Task<Cancellation> GetByTicketIdAsync(int ticketId);
         Task<bool> IsTicketCancelledAsync(int ticketId);
        //  Task<double> GetTicketPriceCancelTodayAsync();
         Task<double> GetTicketPriceCancelByDateAsync(DateTime dateTime);
    }
}