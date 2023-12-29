using Application.Common.Interfaces.Persistence;
using Infrastructure.Data;

namespace Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private static ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}