using Domain.Entities;

namespace Application.Common.Interfaces.Persistence;

public interface ICarriageReponsitory : IReponsitory<Carriage>
{
    Task<IQueryable<Carriage>> GetQueryWithTrainAsync();
}
