using Application.Common.Models;
using Application.Common.Models.Pagination;
using Application.Common.Models.QueryParams;
using Domain.Entities;

namespace Application.Common.Interfaces.Services;

public interface ICompartmentService
{
    Task<PagedList<CompartmentDto>> GetAllCompartmentDtoAsync(CompartmentQueryParams queryParams);
    Task<CompartmentDto> GetCompartmentDtoByIdAsync(int id);
    Task<Compartment> GetCompartmentByIdAsync(int id);
    Task AddCompartmentAsync(Compartment compartment);
    Task UpdateCompartmentAsync(Compartment compartment);
    Task DeleteCompartmentAsync(Compartment compartment);
    Task SoftDeleteCompartmentAsync(Compartment compartment);
}
