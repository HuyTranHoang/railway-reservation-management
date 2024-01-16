using Domain.Exceptions;

namespace WebApi.Controllers;

public class PassengersController : BaseApiController
{
    private readonly IPassengerService _passengerService;

    public PassengersController(IPassengerService passengerService)
    {
        _passengerService = passengerService;
    }

    [HttpGet]
    public async Task<ActionResult<List<PassengerDto>>> GetPassengers([FromQuery] QueryParams queryParams)
    {
        var passengersDto = await _passengerService.GetAllDtoAsync(queryParams);

        var paginationHeader = new PaginationHeader(queryParams.PageNumber, queryParams.PageSize,
            passengersDto.TotalCount, passengersDto.TotalPages);

        Response.AddPaginationHeader(paginationHeader);

        return Ok(passengersDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PassengerDto>> GetPassenger(int id)
    {
        var passenger = await _passengerService.GetDtoByIdAsync(id);

        if (passenger is null) return NotFound(new ErrorResponse(404));

        return Ok(passenger);
    }

    [HttpPost]
    public async Task<ActionResult> AddPassenger([FromBody] Passenger passenger)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        await _passengerService.AddAsync(passenger);
        return CreatedAtAction("GetPassenger", new { id = passenger.Id }, passenger);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePassenger(int id, [FromBody] Passenger passenger)
    {
        if (id != passenger.Id) return BadRequest(new ErrorResponse(400));

        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            await _passengerService.UpdateAsync(passenger);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, ex.Message));
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePassenger(int id)
    {
        var passenger = await _passengerService.GetByIdAsync(id);

        if (passenger == null) return NotFound(new ErrorResponse(404));

        await _passengerService.DeleteAsync(passenger);

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> SoftDeletePassenger(int id)
    {
        var passenger = await _passengerService.GetByIdAsync(id);

        if (passenger is null) return NotFound(new ErrorResponse(404));

        await _passengerService.SoftDeleteAsync(passenger);

        return Ok();
    }
}