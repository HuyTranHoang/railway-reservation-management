namespace Application.Common.Interfaces.Persistence;

public interface ICompartmentRepository : IRepository<Compartment>
{
    Task<IQueryable<Compartment>> GetQueryWithCarriageAsync();
    Task<Compartment> GetByIdWithSeatsAsync(int id);
    Task<List<Compartment>> GetCompartmentsByCarriageIdAsync(int carriageId);
    Task<List<Compartment>> GetAllNoPagingAsync();
}