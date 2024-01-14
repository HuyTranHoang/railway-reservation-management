namespace Application.Common.Interfaces.Persistence;

public interface ICarriageRepository : IRepository<Carriage>
{
    Task<IQueryable<Carriage>> GetQueryWithTrainAndTypeAsync();
    Task<Carriage> GetByIdWithCompartmentsAsync(int id);
    Task<Double> GetServiceChargeByIdAsync(int id);

    Task<List<Carriage>> GetAllAsync();
}