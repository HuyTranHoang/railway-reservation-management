using Application.Common.Models;
using Application.Common.Models.Pagination;
using Application.Common.Models.QueryParams;
using Domain.Entities;

namespace Application.Common.Interfaces.Services;

public interface ICarriageTypeService : IService<CarriageType>
{
    Task<PagedList<CarriageTypeDto>> GetAllDtoAsync(QueryParams queryParams);
    Task<CarriageTypeDto> GetDtoByIdAsync(int id);
}