using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class TrainStationRepository : ITrainStationRepository
    {
        private readonly ApplicationDbContext _context;

        public TrainStationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(TrainStation trainStation)
        {
            await _context.TrainStations.AddAsync(trainStation);

        }

        public async Task Delete(TrainStation trainStation)
        {
            _context.TrainStations.Remove(trainStation);
            await Task.CompletedTask;
        }

        public Task<List<TrainStation>> GetAllNoPagingAsync()
        {
            return _context.TrainStations.ToListAsync();
        }

        public async Task<TrainStation> GetByIdAsync(int id)
        {
            return await _context.TrainStations
            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IQueryable<TrainStation>> GetQueryAsync()
        {
            return await Task.FromResult(_context.TrainStations.AsQueryable());
        }

        public async Task SoftDelete(TrainStation trainStation)
        {
            trainStation.IsDeleted = true;
            _context.Entry(trainStation).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        public async Task Update(TrainStation trainStation)
        {
            _context.Entry(trainStation).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        public async Task<List<TrainStation>> GetStationsFromToAsync(int startCoordinate, int endCoordinate)
        {


            List<TrainStation> stations;

            if (startCoordinate < endCoordinate)
            {
                stations = await _context.TrainStations
                    .AsQueryable()
                    .Where(station => station.CoordinateValue >= startCoordinate && station.CoordinateValue <= endCoordinate)
                    .ToListAsync();
            }
            else if (startCoordinate > endCoordinate)
            {
                (startCoordinate, endCoordinate) = (endCoordinate, startCoordinate);

                stations = await _context.TrainStations
                    .AsQueryable()
                    .Where(station => station.CoordinateValue >= startCoordinate && station.CoordinateValue <= endCoordinate)
                    .OrderByDescending(station => station.CoordinateValue)
                    .ToListAsync();
            }
            else
            {
                stations = new List<TrainStation>();
            }
            return stations;
        }
    }
}