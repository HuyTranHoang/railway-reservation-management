using Application.Common.Models;
using Domain.Entities;

namespace Application.Common.Interfaces.Services;

public interface IProductExampleService
{
    Task<List<ProductExampleDto>> GetAllAsync();
    Task<ProductExampleDto> GetByIdAsync(int id);
    Task AddAsync(ProductExample productExample);
    Task UpdateAsync(ProductExample productExample);
    Task DeleteAsync(ProductExample productExample);
}