using Domain.Entities;

namespace Application.Common.Interfaces.Persistence;

public interface ITrainRepository
{
    Task<IEnumerable<Train>> GetAllAsync();
    Task<Train> GetByIdAsync(int id);
    void Add(Train train);
    void Update(Train train);
    void Delete(Train train);
    void SoftDelete(Train train);
    DateTime GetOldCreatedDate(int id);
}