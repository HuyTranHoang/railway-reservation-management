using Domain.Entities;

namespace Application.Common.Interfaces.Persistence;

public interface ICompartmentReponsitory
{
    Task<IQueryable<Compartment>> GetQueryAsync();
    Task<IQueryable<Compartment>> GetQueryWithCarriageAsync();
    Task<Compartment> GetByIdAsync(int id);
    void Add(Compartment compartment);
    void Update(Compartment compartment);
    void Delete(Compartment compartment);
    void SoftDelete(Compartment compartment);
}
