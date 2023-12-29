using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ProductExampleRepositoy : IProductExampleRepositoy
{
    private readonly ApplicationDbContext _context;

    public ProductExampleRepositoy(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductExample>> GetAllAsync()
    {
        return await _context.ProductExamples.ToListAsync();
    }

    public async Task<ProductExample> GetByIdAsync(int id)
    {
        return await _context.ProductExamples.FindAsync(id);
    }

    public void Add(ProductExample productExample)
    {
        _context.ProductExamples.Add(productExample);
    }
}