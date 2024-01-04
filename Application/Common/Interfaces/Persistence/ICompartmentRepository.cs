namespace Application.Common.Interfaces.Persistence;

public interface ICompartmentRepository : IRepository<Compartment>
{
    Task<IQueryable<Compartment>> GetQueryWithCarriageAsync();
}