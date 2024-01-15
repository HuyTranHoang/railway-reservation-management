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

        public void Add(TrainStation trainStation)
        {
            _context.TrainStations.Add(trainStation);

        }

        public void Delete(TrainStation trainStation)
        {
            _context.TrainStations.Remove(trainStation);
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

        public void SoftDelete(TrainStation trainStation)
        {
            trainStation.IsDeleted = true;
            _context.Entry(trainStation).State = EntityState.Modified;
        }

        public void Update(TrainStation trainStation)
        {
            _context.Entry(trainStation).State = EntityState.Modified;
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