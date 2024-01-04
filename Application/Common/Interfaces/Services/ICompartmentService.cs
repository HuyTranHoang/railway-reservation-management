using Application.Common.Models;
using Application.Common.Models.Pagination;
using Application.Common.Models.QueryParams;
using Domain.Entities;

namespace Application.Common.Interfaces.Services;

public interface ICompartmentService : IService<Compartment>
{
    Task<PagedList<CompartmentDto>> GetAllDtoAsync(CompartmentQueryParams queryParams);
    Task<CompartmentDto> GetDtoByIdAsync(int id);
}
