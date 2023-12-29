using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class ProductExampleController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductExampleRepositoy _repositoy;

    public ProductExampleController(IUnitOfWork unitOfWork, IProductExampleRepositoy repositoy)
    {
        _unitOfWork = unitOfWork;
        _repositoy = repositoy;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductExample>>> GetAll()
    {
        return await _repositoy.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductExample>> GetById(int id)
    {
        return await _repositoy.GetByIdAsync(id);
    }

    [HttpPost]
    public async Task<ActionResult> Create(ProductExample productExample)
    {
        _repositoy.Add(productExample);
        await _unitOfWork.SaveChangesAsync();
        return Ok();
    }
    
}