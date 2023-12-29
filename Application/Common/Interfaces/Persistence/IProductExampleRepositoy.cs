using Domain.Entities;

namespace Application.Common.Interfaces.Persistence;

public interface IProductExampleRepositoy
{
    Task<List<ProductExample>> GetAllAsync();
    Task<ProductExample> GetByIdAsync(int id);
    void Add(ProductExample productExample);
}