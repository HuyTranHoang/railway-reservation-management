using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Domain.Entities;

namespace Application.Services;

public class ProductExampleService : IProductExampleService
{
    private readonly IProductExampleRepositoy _repositoy;
    private readonly IUnitOfWork _unitOfWork;

    public ProductExampleService(IProductExampleRepositoy repositoy, IUnitOfWork unitOfWork)
    {
        _repositoy = repositoy;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<ProductExample>> GetAllAsync()
    {
        return await _repositoy.GetAllAsync();
    }

    public async Task<ProductExample> GetByIdAsync(int id)
    {
        return await _repositoy.GetByIdAsync(id);
    }

    public async Task AddAsync(ProductExample productExample)
    {
        _repositoy.Add(productExample);
        await _unitOfWork.SaveChangesAsync();
    }

    public Task UpdateAsync(ProductExample productExample)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(ProductExample productExample)
    {
        throw new NotImplementedException();
    }
}