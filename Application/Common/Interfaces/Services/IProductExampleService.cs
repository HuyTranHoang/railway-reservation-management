using Domain.Entities;

namespace Application.Common.Interfaces.Services;

public interface IProductExampleService
{
    Task<List<ProductExample>> GetAllAsync();
    Task<ProductExample> GetByIdAsync(int id);
    Task AddAsync(ProductExample productExample);
    Task UpdateAsync(ProductExample productExample);
    Task DeleteAsync(ProductExample productExample);
}