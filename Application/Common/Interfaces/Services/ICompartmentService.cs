namespace Application.Common.Interfaces.Services;

public interface ICompartmentService : IService<Compartment>
{
    Task<PagedList<CompartmentDto>> GetAllDtoAsync(CompartmentQueryParams queryParams);
    Task<CompartmentDto> GetDtoByIdAsync(int id);
    Task<int> GetSeatsBelongToCompartmentCountAsync(int compartmentId);
}