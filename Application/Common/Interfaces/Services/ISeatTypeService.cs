using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Models;
using Domain.Entities;

namespace Application.Common.Interfaces.Services
{
    public interface ISeatTypeService 
    {
    Task<PagedList<SeatTypeDto>> GetAllAsync(QueryParams queryParams);
    Task<SeatTypeDto> GetByIdDtoAsync(int id);
    Task<SeatType> GetByIdAsync(int id);
    Task AddAsync(SeatType seatType);
    Task DeleteAsync(SeatType seatType);
    Task UpdateAsync(SeatType seatType);
    }
}