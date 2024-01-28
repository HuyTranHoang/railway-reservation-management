namespace Application.Common.Interfaces.Persistence;

public interface ICarriageRepository : IRepository<Carriage>
{
    Task<IQueryable<Carriage>> GetQueryWithTrainAndTypeAsync();
    Task<Carriage> GetByIdWithCompartmentsAsync(int id);
    Task<Carriage> GetByIdWithCarriageTypeAsync(int id);
    Task<double> GetServiceChargeByIdAsync(int id);
    Task<List<Carriage>> GetCarriagesByTrainIdAsync(int trainId);

    Task<List<Carriage>> GetAllNoPagingAsync();
}
