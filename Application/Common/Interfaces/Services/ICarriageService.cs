using Application.Common.Models;
using Application.Common.Models.Pagination;
using Application.Common.Models.QueryParams;
using Domain.Entities;

namespace Application.Common.Interfaces.Services;

public interface ICarriageService
{
    Task<PagedList<CarriageDto>> GetAllCarriageDtoAsync(CarriageQueryParams queryParams);
    Task<CarriageDto> GetCarriageDtoByIdAsync(int id);
    Task<Carriage> GetCarriageByIdAsync(int id);
    Task AddCarriageAsync(Carriage carriage);
    Task UpdateCarriageAsync(Carriage carriage);
    Task DeleteCarriageAsync(Carriage carriage);
    Task SoftDeleteCarriageAsync(Carriage carriage);
}
