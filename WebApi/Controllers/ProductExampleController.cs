using Application.Common.Interfaces.Services;
using Application.Common.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class ProductExampleController : BaseApiController
{
    private readonly IProductExampleService _productExampleService;


    public ProductExampleController(IProductExampleService productExampleService)
    {
        _productExampleService = productExampleService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductExampleDto>>> GetAll()
    {
        var productExamples = await _productExampleService.GetAllAsync();
        return Ok(productExamples);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductExampleDto>> GetById(int id)
    {
        var productExample = await _productExampleService.GetByIdAsync(id);
        return Ok(productExample);
    }

    [HttpPost]
    public async Task<ActionResult> Create(ProductExample productExample)
    {
        await _productExampleService.AddAsync(productExample);

        return CreatedAtAction("GetById", new { id = productExample.Id }, productExample);
    }
}