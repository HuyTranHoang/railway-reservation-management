using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Application.Common.Models;
using Application.Common.Models.Pagination;
using Application.Common.Models.QueryParams;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi.Exceptions;
using WebApi.Extensions;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeatTypeController : ControllerBase

{
    private readonly ISeatTypeService _seatTypeService;
    private readonly IUnitOfWork _unitofWork;

    public SeatTypeController(ISeatTypeService seatTypeService, IUnitOfWork unitofWork)
    {
        _seatTypeService = seatTypeService;
        _unitofWork = unitofWork;
    }

    [HttpGet]
    public async Task<ActionResult<List<SeatTypeDto>>> GetAll([FromQuery] QueryParams queryParams)
    {
        var seatTypesDto = await _seatTypeService.GetAllDtoAsync(queryParams);

        var paginationHeader = new PaginationHeader(queryParams.PageNumber, queryParams.PageSize,
            seatTypesDto.TotalCount, seatTypesDto.TotalPages);

        Response.AddPaginationHeader(paginationHeader);

        return Ok(seatTypesDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SeatTypeDto>> GetById(int id)
    {
        var seatTypes = await _seatTypeService.GetByIdDtoAsync(id);

        if (seatTypes == null) return NotFound(new ErrorResponse(404));

        return Ok(seatTypes);
    }

    [HttpPost]
    public async Task<ActionResult> Create(SeatType seatType)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        await _seatTypeService.AddAsync(seatType);
        return CreatedAtAction("GetById", new { id = seatType.Id }, seatType);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var seatType = await _seatTypeService.GetByIdAsync(id);
        if (seatType == null) return NotFound();
        await _seatTypeService.DeleteAsync(seatType);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] SeatType seatType)
    {
        if (id != seatType.Id) return BadRequest(new ErrorResponse(400));

        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            await _seatTypeService.UpdateAsync(seatType);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, ex.Message));
        }

        return NoContent();
    }
}