using Domain.Entities;

namespace Application.Common.Interfaces.Persistence;

public interface ITrainRepository : IReponsitory<Train>
{
    Task<IQueryable<Train>> GetQueryWithTrainCompanyAsync();
}