using Domain.Entities;

namespace Application.Common.Interfaces.Persistence;

public interface ICarriageReponsitory
{
    Task<IQueryable<Carriage>> GetQueryAsync();
    Task<IQueryable<Carriage>> GetQueryWithTrainAsync();
    Task<Carriage> GetByIdAsync(int id);
    void Add(Carriage carriage);
    void Update(Carriage carriage);
    void Delete(Carriage carriage);
    void SoftDelete(Carriage carriage);
}
