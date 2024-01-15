namespace Application.Common.Interfaces.Persistence;

public interface ICarriageTypeRepository : IRepository<CarriageType>
{
    Task<List<CarriageType>> GetAllNoPagingAsync();
}