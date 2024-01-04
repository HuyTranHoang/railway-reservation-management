using Application.Common.Models;
using Application.Common.Models.Pagination;
using Application.Common.Models.QueryParams;
using Domain.Entities;

namespace Application.Common.Interfaces.Services;

public interface ITrainCompanyService : IService<TrainCompany>
{
    Task<PagedList<TrainCompanyDto>> GetAllDtoAsync(QueryParams queryParams);
    Task<TrainCompanyDto> GetDtoByIdAsync(int id);
}