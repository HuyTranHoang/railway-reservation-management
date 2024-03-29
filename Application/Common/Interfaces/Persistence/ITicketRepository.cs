namespace Application.Common.Interfaces.Persistence
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<IQueryable<Ticket>> GetQueryWithRelationshipTableAsync();
        Task<Ticket> GetByCodeAndEmail(string code, string email);
        List<Ticket> GetAllTickets();
        Task<List<Ticket>> GetAllNoPagingAsync();
        Task<List<Ticket>> GetByIdWithPaymentAsync(int id);
        Task<int> GetTicketCountTodayAsync();
        // Task<double> GetTicketPriceTodayAsync();
        Task<double> GetTicketPriceSumByDateAsync(DateTime date);
        Task<int> GetSeatsBookedInSchedule(int scheduleId);
    }
}