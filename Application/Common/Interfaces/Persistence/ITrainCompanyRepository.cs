using Domain.Entities;

namespace Application.Common.Interfaces.Persistence;

public interface ITrainCompanyRepository
{
    Task<IQueryable<TrainCompany>> GetQueryAsync();
    Task<TrainCompany> GetByIdAsync(int id);
    void Add(TrainCompany trainCompany);
    void Update(TrainCompany trainCompany);
    void Delete(TrainCompany trainCompany);
    void SoftDelete(TrainCompany trainCompany);
}