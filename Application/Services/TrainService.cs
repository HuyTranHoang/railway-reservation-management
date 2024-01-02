using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    public class TrainService : ITrainService
    {

        private readonly ITrainRepository _reponsitory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainService(ITrainRepository repositoy, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _reponsitory = repositoy;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddTrainAsync(Train train)
        {
            _reponsitory.Add(train);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteTrainAsync(Train train)
        {
            _reponsitory.Delete(train);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<TrainDto>> GetAllTrainDtoAsync()
        {
            var trainDto =  await _reponsitory.GetAllAsync();
            return _mapper.Map<List<TrainDto>>(trainDto);
        }

        public async Task<Train> GetTrainByIdAsync(int id)
        {
            return await _reponsitory.GetByIdAsync(id);
        }

        public async Task<TrainDto> GetTrainDtoByIdAsync(int id)
        {
            var trainDto = await _reponsitory.GetByIdAsync(id);
            return _mapper.Map<TrainDto>(trainDto);
        }

        public async Task SoftDeleteTrainAsync(Train train)
        {
            _reponsitory.SoftDelete(train);
             await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateTrainAsync(Train train)
        {
            _reponsitory.Update(train);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}