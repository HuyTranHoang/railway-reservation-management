namespace Application.Common.Interfaces.Persistence;

public interface ICarriageRepository : IRepository<Carriage>
{
    Task<IQueryable<Carriage>> GetQueryWithTrainAndTypeAsync();
    Task<Carriage> GetByIdWithCompartmentsAsync(int id);
}