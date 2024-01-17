using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;


public class ScheduleRepository : IScheduleRepository
{
    private readonly ApplicationDbContext _context;

    public ScheduleRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Add(Schedule schedule)
    {
        _context.Schedules.Add(schedule);
        await Task.CompletedTask;
    }

    public async Task Delete(Schedule schedule)
    {
        _context.Schedules.Remove(schedule);
        await Task.CompletedTask;
    }

    public async Task<Schedule> GetByIdAsync(int id)
    {
        return await _context.Schedules.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IQueryable<Schedule>> GetQueryAsync()
    {
        return await Task.FromResult(_context.Schedules.AsQueryable());
    }

    public async Task<IQueryable<Schedule>> GetQueryWithTrainAndStationAsync()
    {
        return await Task.FromResult(_context
        .Schedules
            .Include(t => t.Train)
            .Include(s => s.ArrivalStation)
            .Include(s => s.DepartureStation)
            .AsQueryable());
    }

    public async Task SoftDelete(Schedule schedule)
    {
        schedule.IsDeleted = true;
        _context.Entry(schedule).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task Update(Schedule schedule)
    {
        _context.Entry(schedule).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task<Schedule> GetScheduleByStationsAsync(int trainId, int departureStationId, int arrivalStationId)
    {
        return await _context.Schedules
            .FirstOrDefaultAsync(schedule =>
                schedule.TrainId == trainId &&
                schedule.DepartureStationId == departureStationId &&
                schedule.ArrivalStationId == arrivalStationId);
    }

    public async Task<List<Schedule>> GetSchedulesByTrainAsync(int trainId)
    {
        return await _context.Schedules
            .Where(schedule => schedule.TrainId == trainId)
            .ToListAsync();
    }
};
