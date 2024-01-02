using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Application.Common.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi.Exceptions;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatTypeController : ControllerBase
  
    {
    private readonly ISeatTypeService _seatTypeService;
      private readonly IUnitOfWork _unitofWork;
    public SeatTypeController(ISeatTypeService seatTypeService,IUnitOfWork unitofWork)
    {
            _seatTypeService = seatTypeService;
            _unitofWork = unitofWork;
    }
    [HttpGet]
    public async Task<ActionResult<List<SeatTypeDto>>> GetAll([FromQuery] QueryParams queryParams)
    {
        var seatTypes = await _seatTypeService.GetAllAsync(queryParams);
        return Ok(seatTypes);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<SeatTypeDto>> GetById(int id)
    {
        var seatTypes = await _seatTypeService.GetByIdDtoAsync(id);
        return Ok(seatTypes);
    }
    [HttpPost]
    public async Task<ActionResult> Create(SeatType seatType)
    {
        await _seatTypeService.AddAsync(seatType);
        return CreatedAtAction("GetById", new { id = seatType.Id }, seatType);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var seatType = await _seatTypeService.GetByIdAsync(id);
        if (seatType == null)
        {
            return NotFound();
        }
      await _seatTypeService.DeleteAsync(seatType);
        return NoContent();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] SeatType seatType)
    {
    if (id != seatType.Id)
        {
            return BadRequest(new ErrorResponse(400));
        }
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
}