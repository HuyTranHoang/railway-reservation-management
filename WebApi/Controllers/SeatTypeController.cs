namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeatTypeController : ControllerBase

{
    private readonly ISeatTypeService _seatTypeService;

    public SeatTypeController(ISeatTypeService seatTypeService)
    {
        _seatTypeService = seatTypeService;
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
        var seatTypesDto = await _seatTypeService.GetDtoByIdAsync(id);

        if (seatTypesDto == null) return NotFound(new ErrorResponse(404));

        return Ok(seatTypesDto);
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

    [HttpPatch("{id}")]
    public async Task<IActionResult> SoftDeleteSeatType(int id)
    {
        var seatType = await _seatTypeService.GetByIdAsync(id);

        if (seatType == null) return NotFound();

        await _seatTypeService.SoftDeleteAsync(seatType);

        return Ok();
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