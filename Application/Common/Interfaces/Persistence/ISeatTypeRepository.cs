namespace Application.Common.Interfaces.Persistence;

public interface ISeatTypeRepository : IRepository<SeatType>
{
    Task<List<SeatType>> GetAllNoPagingAsync();
}