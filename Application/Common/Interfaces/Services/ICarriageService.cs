using Application.Common.Models;
using Application.Common.Models.Pagination;
using Application.Common.Models.QueryParams;
using Domain.Entities;

namespace Application.Common.Interfaces.Services;

public interface ICarriageService : IService<Carriage>
{
    Task<PagedList<CarriageDto>> GetAllDtoAsync(CarriageQueryParams queryParams);
    Task<CarriageDto> GetDtoByIdAsync(int id);
}
