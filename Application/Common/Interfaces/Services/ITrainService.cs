using Application.Common.Models;
using Application.Common.Models.Pagination;
using Application.Common.Models.QueryParams;
using Domain.Entities;

namespace Application.Common.Interfaces.Services;

public interface ITrainService : IService<Train>
{
    Task<PagedList<TrainDto>> GetAllDtoAsync(TrainQueryParams queryParams);
    Task<TrainDto> GetDtoByIdAsync(int id);

}