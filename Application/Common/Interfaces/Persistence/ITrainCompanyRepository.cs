using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Common.Interfaces.Persistence
{
    public interface ITrainCompanyRepository
    {
        Task<IEnumerable<TrainCompany>> GetAllAsync();
        Task<TrainCompany> GetByIdAsync(int id);
        void Add(TrainCompany trainCompany);
        void Update(TrainCompany trainCompany);
        void Delete(TrainCompany trainCompany);
        void SoftDelete(TrainCompany trainCompany);
        DateTime GetOldCreatedDate(int id);
    }
}