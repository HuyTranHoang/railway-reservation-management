namespace Application.Common.Interfaces.Persistence
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<IQueryable<Ticket>> GetQueryWithRelationshipTableAsync();
        Task<Ticket> GetByCodeAndPhone(string code, string phone);
        List<Ticket> GetAllTickets();
        Task<List<Ticket>> GetAllNoPagingAsync();
    }
}