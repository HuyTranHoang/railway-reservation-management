using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class ProductExampleService : IProductExampleService
{
    private readonly IProductExampleRepositoy _repositoy;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductExampleService(IProductExampleRepositoy repositoy, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repositoy = repositoy;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<ProductExampleDto>> GetAllAsync()
    {
        var productExamples = await _repositoy.GetAllAsync();
        return _mapper.Map<List<ProductExampleDto>>(productExamples);
    }

    public async Task<ProductExampleDto> GetByIdAsync(int id)
    {
        var productExample = await _repositoy.GetByIdAsync(id);
        return _mapper.Map<ProductExampleDto>(productExample);
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