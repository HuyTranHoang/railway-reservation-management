namespace Application.Common.Interfaces.Services
{
    public interface ITicketService : IService<Ticket>
    {
        Task<PagedList<TicketDto>> GetAllDtoAsync(TicketQueryParams queryParams);
        Task<TicketDto> GetDtoByIdAsync(int id);
        Task<List<TicketDto>> GetAllDtoNoPagingAsync();

        Task AddRoundTripAsync(Ticket ticket);
    }
}