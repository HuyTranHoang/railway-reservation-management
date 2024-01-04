using Domain.Entities;

namespace Application.Common.Interfaces.Persistence;

public interface ICompartmentReponsitory : IReponsitory<Compartment>
{
    Task<IQueryable<Compartment>> GetQueryWithCarriageAsync();
}
