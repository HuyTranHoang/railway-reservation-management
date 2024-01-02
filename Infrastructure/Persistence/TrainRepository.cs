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

        public void Delete(Train train)
        {
            _context.Trains.Remove(train);
        }

        public async Task<IEnumerable<Train>> GetAllAsync()
        {
           return await _context.Trains
                 .ToListAsync();
        }

        public async Task<Train> GetByIdAsync(int id)
        {
            return await _context.Trains
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public DateTime GetOldCreatedDate(int id)
        {
            var train = _context.TrainCompanies
            .Where(p => p.Id == id)
            .Select(p => new { p.CreatedAt })
            .FirstOrDefault();

            return train?.CreatedAt ?? DateTime.Now;
        }

        public void SoftDelete(Train train)
        {
            train.IsDeleted = true;
            _context.Entry(train).State = EntityState.Modified;
        }

        public void Update(Train train)
        {
            _context.Entry(train).State = EntityState.Modified;
        }
    }
}