﻿using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class CancellationRuleRepository : ICancellationRuleRepository
{
    private readonly ApplicationDbContext _context;

    public CancellationRuleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IQueryable<CancellationRule>> GetQueryAsync()
    {
        return await Task.FromResult(_context.CancellationRules.AsQueryable());
    }

    public async Task<CancellationRule> GetByIdAsync(int id)
    {
        return await _context.CancellationRules.FindAsync(id);
    }

    public void Add(CancellationRule cancellationRule)
    {
        _context.CancellationRules.Add(cancellationRule);
    }

    public void Update(CancellationRule cancellationRule)
    {
        _context.Entry(cancellationRule).State = EntityState.Modified;
    }

    public void Delete(CancellationRule cancellationRule)
    {
        _context.CancellationRules.Remove(cancellationRule);
    }

    public void SoftDelete(CancellationRule cancellationRule)
    {
        cancellationRule.IsDeleted = true;
        _context.Entry(cancellationRule).State = EntityState.Modified;
    }
}