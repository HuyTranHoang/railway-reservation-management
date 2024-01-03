using Application.Common.Exceptions;
using Application.Common.Interfaces.Services;
using Application.Common.Models;
using Application.Common.Models.Pagination;
using Application.Common.Models.QueryParams;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi.Exceptions;
using WebApi.Extensions;

namespace WebApi.Controllers;

public class CarriagesController : BaseApiController
{
    private readonly ICarriageService _carriageService;

    public CarriagesController(ICarriageService carriageService)
    {
        _carriageService = carriageService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarriageDto>>> GetCarriages([FromQuery] CarriageQueryParams queryParams)
    {
        var carriagesDto = await _carriageService.GetAllCarriageDtoAsync(queryParams);

        var paginationHeader = new PaginationHeader(queryParams.PageNumber, queryParams.PageSize, 
        carriagesDto.TotalCount, carriagesDto.TotalPages);

        Response.AddPaginationHeader(paginationHeader);

        return Ok(carriagesDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CarriageDto>> GetCarriage(int id)
    {
        var carriagesDto = await _carriageService.GetCarriageDtoByIdAsync(id);

        if (carriagesDto is null) return NotFound(new ErrorResponse(404));

        return Ok(carriagesDto);
    }

    [HttpPost]
    public async Task<ActionResult> PostCarriage([FromBody] Carriage carriage)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            await _carriageService.AddCarriageAsync(carriage);
        }
        catch (BadRequestException ex)
        {
            var errorResponse = new ValidateInputError(400, new List<string> { ex.Message });
            return BadRequest(errorResponse);
        }

        return CreatedAtAction("GetCarriage", new { id = carriage.Id }, carriage);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCarriage(int id, [FromBody] Carriage carriage)
    {
        if (id != carriage.Id) return BadRequest(new ErrorResponse(400));

        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _carriageService.UpdateCarriageAsync(carriage);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, ex.Message));
        }
        catch (BadRequestException ex)
        {
            var errorResponse = new ValidateInputError(400, new List<string> { ex.Message });
            BadRequest(errorResponse);
        }

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> SoftDeleteCarriage(int id)
    {
        var carriage = await _carriageService.GetCarriageByIdAsync(id);
        if(carriage == null)
        {
            return NotFound(new ErrorResponse(404));
        }

        await _carriageService.SoftDeleteCarriageAsync(carriage);

        return NoContent();
    }
}
