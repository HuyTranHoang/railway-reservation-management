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
    public class TrainCompanyRepository : ITrainCompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public TrainCompanyRepository(ApplicationDbContext context)
        {

            _context = context;

        }
        public void Add(TrainCompany trainCompany)
        {
            _context.TrainCompanies.Add(trainCompany);
        }

        public void Delete(TrainCompany trainCompany)
        {
            trainCompany.IsDeleted = true;
            _context.SaveChanges();
        }

        public async Task<IEnumerable<TrainCompany>> GetAllAsync()
        {
            return await _context.TrainCompanies.ToListAsync();
        }

        public async Task<TrainCompany> GetByIdAsync(int id)
        {
            return await _context.TrainCompanies
                            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(TrainCompany trainCompany)
        {
            _context.Entry(trainCompany).State = EntityState.Modified;
        }
        
        public void SoftDelete(TrainCompany trainCompany)
        {
            trainCompany.IsDeleted = true;
            _context.Entry(trainCompany).State = EntityState.Modified;
        }

        public DateTime GetOldCreatedDate(int id)
        {
            var trainCompany = _context.TrainCompanies
            .Where(p => p.Id == id)
            .Select(p => new { p.CreatedAt })
            .FirstOrDefault();

            return trainCompany?.CreatedAt ?? DateTime.Now;
        }
    }
}