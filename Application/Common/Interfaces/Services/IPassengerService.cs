using Application.Common.Models;
using Domain.Entities;

namespace Application.Common.Interfaces.Services;

public interface IPassengerService
{
    Task<PagedList<PassengerDto>> GetAllPassengerDtoAsync(QueryParams queryParams);
    Task<PassengerDto> GetPassgenerDtoByIdAsync(int id);
    Task<Passenger> GetPassgenerByIdAsync(int id);
    Task AddPassengerAsync(Passenger passenger);
    Task UpdatePassengerAsync(Passenger passenger);
    Task DeletePassengerAsync(Passenger passenger);
    Task SoftDeletePassengerAsync(Passenger passenger);
}