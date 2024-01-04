using Application.Common.Models;
using Application.Common.Models.Pagination;
using Application.Common.Models.QueryParams;
using Domain.Entities;

namespace Application.Common.Interfaces.Services;

public interface ISeatTypeService
{
    Task<PagedList<SeatTypeDto>> GetAllSeatTypeDtoAsync(QueryParams queryParams);
    Task<SeatTypeDto> GetSeatTypeDtoByIdAsync(int id);
    Task<SeatType> GetSeatTypeByIdAsync(int id);
    Task AddSeatTypeAsync(SeatType seatType);
    Task UpdateSeatTypeAsync(SeatType seatType);
    Task DeleteSeatTypeAsync(SeatType seatType);
    Task SoftDeleteSeatTypeAsync(SeatType seatType);
}