namespace Application.Common.Interfaces.Persistence
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<IQueryable<Ticket>> GetQueryWithRelationshipTableAsync();
        Task<Ticket> GetByCodeAndEmail(string code, string email);
        List<Ticket> GetAllTickets();
    }
}