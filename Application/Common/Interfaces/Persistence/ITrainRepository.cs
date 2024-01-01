using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Common.Interfaces.Persistence
{
    public interface ITrainRepository
    {
        Task<List<Train>> GetAllAsync();
        Task<Train> GetByIdAsync(int id);
        void Add(Train train);
    
    }
}