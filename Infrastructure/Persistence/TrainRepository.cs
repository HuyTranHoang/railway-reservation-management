using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class TrainRepository : ITrainRepository
    {
        private readonly ApplicationDbContext _context;

        public TrainRepository(ApplicationDbContext context){

            _context = context;

        }
        public void Add(Train train)
        {
            _context.Trains.Add(train);
        }

        public async Task<List<Train>> GetAllAsync()
        {
            return await _context.Trains
                .Where(x => x.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<Train> GetByIdAsync(int id)
        {
            return await _context.Trains
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);

        }
    }
}