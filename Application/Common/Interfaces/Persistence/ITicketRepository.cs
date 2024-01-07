namespace Application.Common.Interfaces.Persistence
{
    public interface ITicketRepository : IRepository<Ticket>
    {
         Task<IQueryable<Ticket>> GetQueryWithRelationshipTableAsync();
    }
}