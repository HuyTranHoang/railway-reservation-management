using Application.Common.Models;
using Application.Common.Models.Pagination;
using Application.Common.Models.QueryParams;
using Domain.Entities;

namespace Application.Common.Interfaces.Services;

public interface ITrainService
{
    Task<PagedList<TrainDto>> GetAllTrainDtoAsync(TrainQueryParams queryParams);
    Task<Train> GetTrainByIdAsync(int id);
    Task<TrainDto> GetTrainDtoByIdAsync(int id);
    Task AddTrainAsync(Train train);
    Task UpdateTrainAsync(Train train);
    Task DeleteTrainAsync(Train train);
    Task SoftDeleteTrainAsync(Train train);
}