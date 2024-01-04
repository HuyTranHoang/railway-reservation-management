using Application.Common.Models;
using Application.Common.Models.Pagination;
using Application.Common.Models.QueryParams;
using Domain.Entities;

namespace Application.Common.Interfaces.Services;

public interface ISeatTypeService : IService<SeatType>
{
    Task<PagedList<SeatTypeDto>> GetAllDtoAsync(QueryParams queryParams);
    Task<SeatTypeDto> GetDtoByIdAsync(int id);
}