using Application.Common.Models;
using Application.Common.Models.Pagination;
using Application.Common.Models.QueryParams;
using Domain.Entities;

namespace Application.Common.Interfaces.Services;

public interface ISeatService : IService<Seat>
{
    Task<PagedList<SeatDto>> GetAllDtoAsync(SeatQueryParams queryParams);
    Task<SeatDto> GetDtoByIdAsync(int id);
}