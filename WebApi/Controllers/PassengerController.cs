using Application.Common.Exceptions;
using Application.Common.Interfaces.Services;
using Application.Common.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi.Exceptions;

namespace WebApi.Controllers;

public class PassengerController : BaseApiController
{
    private readonly IPassengerService _passengerService;

    public PassengerController(IPassengerService passengerService)
    {
        _passengerService = passengerService;
    }

    [HttpGet]
    public async Task<ActionResult<List<PassengerDto>>> GetPassengers()
    {
        var passengersDto = await _passengerService.GetAllPassengerDtoAsync();
        return Ok(passengersDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PassengerDto>> GetPassenger(int id)
    {
        var passenger = await _passengerService.GetPassgenerDtoByIdAsync(id);
        if (passenger == null)
        {
            return NotFound(new ErrorResponse(404));
        }

        return Ok(passenger);
    }

    [HttpPost]
    public async Task<IActionResult> AddPassenger([FromBody] Passenger passenger)
    {
        await _passengerService.AddPassengerAsync(passenger);
        return CreatedAtAction("GetPassenger", new { id = passenger.Id }, passenger);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePassenger(int id, [FromBody] Passenger passenger)
    {
        if (id != passenger.Id)
        {
            return BadRequest(new ErrorResponse(400));
        }

        try
        {
            await _passengerService.UpdatePassengerAsync(passenger);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, ex.Message));
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePassenger(int id)
    {
        var passenger = await _passengerService.GetPassgenerByIdAsync(id);
        if (passenger == null)
        {
            return NotFound(new ErrorResponse(404));
        }

        await _passengerService.DeletePassengerAsync(passenger);

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> SoftDeletePassenger(int id)
    {
        var passenger = await _passengerService.GetPassgenerByIdAsync(id);
        if (passenger == null)
        {
            return NotFound(new ErrorResponse(404));
        }

        await _passengerService.SoftDeletePassengerAsync(passenger);

        return Ok();
    }
}