using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Models;
using Domain.Entities;

namespace Application.Common.Interfaces.Services
{
    public interface ITrainService
    {
        Task<List<TrainDto>> GetAllTrainDtoAsync();
        Task<Train> GetTrainByIdAsync(int id);
        Task<TrainDto> GetTrainDtoByIdAsync(int id);
        Task AddTrainAsync(Train train);
        Task UpdateTrainAsync(Train train);
        Task DeleteTrainAsync(Train train);
        Task SoftDeleteTrainAsync(Train train);
    }
}