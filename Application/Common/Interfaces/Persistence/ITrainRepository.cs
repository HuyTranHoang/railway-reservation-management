using Domain.Entities;

namespace Application.Common.Interfaces.Persistence;

public interface ITrainRepository
{
    Task<IQueryable<Train>> GetQueryAsync();
    Task<IQueryable<Train>> GetQueryWithTrainCompanyAsync();
    Task<Train> GetByIdAsync(int id);
    void Add(Train train);
    void Update(Train train);
    void Delete(Train train);
    void SoftDelete(Train train);
}