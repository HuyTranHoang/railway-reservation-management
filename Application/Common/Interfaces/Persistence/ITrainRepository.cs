namespace Application.Common.Interfaces.Persistence;

public interface ITrainRepository : IRepository<Train>
{
    Task<IQueryable<Train>> GetQueryWithTrainCompanyAsync();
}