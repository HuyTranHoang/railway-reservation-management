namespace Application.Common.Interfaces.Persistence;

public interface ITrainCompanyRepository : IRepository<TrainCompany>
{
    Task<List<TrainCompany>> GetAllAsync();
}