namespace Application.Common.Interfaces.Persistence;

public interface ICompartmentRepository : IRepository<Compartment>
{
    Task<IQueryable<Compartment>> GetQueryWithCarriageAsync();

    Task<Compartment> GetByIdWithSeatsAsync(int id);

    Task<List<Compartment>> GetAllNoPagingAsync();

    Task<List<Compartment>> GetAllAsync();

}