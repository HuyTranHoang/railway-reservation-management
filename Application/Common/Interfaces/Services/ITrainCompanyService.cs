using Application.Common.Models;
using Application.Common.Models.Pagination;
using Application.Common.Models.QueryParams;
using Domain.Entities;

namespace Application.Common.Interfaces.Services;

public interface ITrainCompanyService
{
    Task<PagedList<TrainCompanyDto>> GetAllCompanyDtoAsync(QueryParams queryParams);
    Task<TrainCompany> GetCompanyByIdAsync(int id);
    Task<TrainCompanyDto> GetCompanyDtoByIdAsync(int id);
    Task AddCompanyAsync(TrainCompany trainCompany);
    Task UpdateCompanyAsync(TrainCompany trainCompany);
    Task DeleteCompanyAsync(TrainCompany trainCompany);
    Task SoftDeleteCompanyAsync(TrainCompany trainCompany);
}