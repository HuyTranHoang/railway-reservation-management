namespace Application.Common.Interfaces.Persistence;

public interface IDistanceFareRepository : IRepository<DistanceFare>
{
    Task<IQueryable<DistanceFare>> GetQueryWithTrainCompanyAsync();
}