namespace WebApi.Controllers;

public class SeatController : BaseApiController
{
    private readonly ISeatService _seatService;

    public SeatController(ISeatService seatService)
    {
        _seatService = seatService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SeatDto>>> GetSeats([FromQuery] SeatQueryParams queryParams)
    {
        var seatsDto = await _seatService.GetAllSeatDtoAsync(queryParams);

        var paginationHeader = new PaginationHeader(queryParams.PageNumber, queryParams.PageSize, 
        seatsDto.TotalCount, seatsDto.TotalPages);

        Response.AddPaginationHeader(paginationHeader);

        return Ok(seatsDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SeatDto>> GetSeat(int id)
    {
        var seatsDto = await _seatService.GetSeatDtoByIdAsync(id);

        if (seatsDto is null) return NotFound(new ErrorResponse(404));

        return Ok(seatsDto);
    }

    [HttpPost]
    public async Task<ActionResult> PostSeat([FromBody] Seat seat)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            await _seatService.AddSeatAsync(seat);
        }
        catch (BadRequestException ex)
        {
            var errorResponse = new ValidateInputError(400, new List<string> { ex.Message });
            return BadRequest(errorResponse);
        }

        return CreatedAtAction("GetSeat", new { id = seat.Id }, seat);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutSeat(int id, [FromBody] Seat seat)
    {
        if (id != seat.Id) return BadRequest(new ErrorResponse(400));

        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _seatService.UpdateSeatAsync(seat);
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
    public async Task<IActionResult> SoftDeleteSeat(int id)
    {
        var seat = await _seatService.GetSeatByIdAsync(id);
        if(seat == null)
        {
            return NotFound(new ErrorResponse(404));
        }

        await _seatService.SoftDeleteSeatAsync(seat);

        return NoContent();
    }
}
