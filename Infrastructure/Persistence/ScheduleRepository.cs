using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public void Add(Schedule schedule)
    {
        _context.Schedules.Add(schedule);
    }

    public void Delete(Schedule schedule)
    {
        _context.Schedules.Remove(schedule);
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

    public void SoftDelete(Schedule schedule)
    {
        schedule.IsDeleted = true;
        _context.Entry(schedule).State = EntityState.Modified;
    }

    public void Update(Schedule schedule)
    {
        _context.Entry(schedule).State = EntityState.Modified;
    }
};
