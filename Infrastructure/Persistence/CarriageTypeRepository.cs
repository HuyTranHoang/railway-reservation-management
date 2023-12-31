﻿using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class CarriageTypeRepository : ICarriageTypeRepository
{
    private readonly ApplicationDbContext _context;

    public CarriageTypeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IQueryable<CarriageType>> GetQueryAsync()
    {
        return await Task.FromResult(_context.CarriageTypes.AsQueryable());
    }

    public async Task<CarriageType> GetByIdAsync(int id)
    {
        return await _context.CarriageTypes.FindAsync(id);
    }

    public void Add(CarriageType carriageType)
    {
        _context.CarriageTypes.Add(carriageType);
    }

    public void Update(CarriageType carriageType)
    {
        _context.Entry(carriageType).State = EntityState.Modified;
    }

    public void Delete(CarriageType carriageType)
    {
        _context.CarriageTypes.Remove(carriageType);
    }

    public void SoftDelete(CarriageType carriageType)
    {
        carriageType.IsDeleted = true;
        _context.CarriageTypes.Update(carriageType);
    }
}