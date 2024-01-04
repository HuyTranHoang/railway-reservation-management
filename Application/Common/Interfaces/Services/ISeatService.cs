using Application.Common.Models;
using Application.Common.Models.Pagination;
using Application.Common.Models.QueryParams;
using Domain.Entities;

namespace Application.Common.Interfaces.Services;

public interface ISeatService
{
    Task<PagedList<SeatDto>> GetAllSeatDtoAsync(SeatQueryParams queryParams);
    Task<SeatDto> GetSeatDtoByIdAsync(int id);
    Task<Seat> GetSeatByIdAsync(int id);
    Task AddSeatAsync(Seat seat);
    Task UpdateSeatAsync(Seat seat);
    Task DeleteSeatAsync(Seat seat);
    Task SoftDeleteSeatAsync(Seat seat);
}