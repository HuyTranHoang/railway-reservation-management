using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Models;
using Domain.Entities;

namespace Application.Common.Interfaces.Services
{
    public interface ITrainCompanyService
    {
        Task<List<TrainCompanyDto>> GetAllCompanyDtoAsync();
        Task<TrainCompany> GetCompanyByIdAsync(int id);
        Task<TrainCompanyDto> GetCompanyDtoByIdAsync(int id);
        Task AddCompanyAsync(TrainCompany trainCompany);
        Task UpdateCompanyAsync(TrainCompany trainCompany);
        Task DeleteCompanyAsync(TrainCompany trainCompany);
        Task SoftDeleteCompanyAsync(TrainCompany trainCompany);
    }
}