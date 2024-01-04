﻿using Application.Common.Models;
using Application.Common.Models.Pagination;
using Application.Common.Models.QueryParams;
using Domain.Entities;

namespace Application.Common.Interfaces.Services;

public interface IPassengerService : IService<Passenger>
{
    Task<PagedList<PassengerDto>> GetAllDtoAsync(QueryParams queryParams);
    Task<PassengerDto> GetDtoByIdAsync(int id);
}