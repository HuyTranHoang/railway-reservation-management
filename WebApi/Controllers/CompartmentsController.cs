using Domain.Exceptions;

namespace WebApi.Controllers;

public class CompartmentsController : BaseApiController
{
    private readonly ICompartmentService _compartmentService;

    public CompartmentsController(ICompartmentService compartmentService)
    {
        _compartmentService = compartmentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompartmentDto>>> GetCompartments(
        [FromQuery] CompartmentQueryParams queryParams)
    {
        var compartmentDto = await _compartmentService.GetAllDtoAsync(queryParams);

        var paginationHeader = new PaginationHeader(queryParams.PageNumber, queryParams.PageSize, compartmentDto.TotalCount, compartmentDto.TotalPages);

        Response.AddPaginationHeader(paginationHeader);

        return Ok(compartmentDto);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<CompartmentDto>>> GetAllSeats()
    {
        var seatsDto = await _compartmentService.GetAllDtoNoPagingAsync();

        return Ok(seatsDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CompartmentDto>> GetCompartment(int id)
    {
        var compartmentDto = await _compartmentService.GetDtoByIdAsync(id);

        if (compartmentDto is null) return NotFound(new ErrorResponse(404));

        return Ok(compartmentDto);
    }

    [HttpGet("{id}/seats")]
    public async Task<ActionResult<object>> GetNumberSeatsBelongToCompartment(int id)
    {
        var count = await _compartmentService.GetSeatsBelongToCompartmentCountAsync(id);

        return Ok(new
        {
            Id = id,
            NumberOfCompartments = count
        });
    }

    [HttpPost]
    public async Task<ActionResult> PostCompartment([FromBody] Compartment compartment)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            await _compartmentService.AddAsync(compartment);
        }
        catch (BadRequestException ex)
        {
            var errorResponse = new ValidateInputError(400, new List<string> { ex.Message });
            return BadRequest(errorResponse);
        }

        var compartmentDto = await _compartmentService.GetDtoByIdAsync(compartment.Id);

        return CreatedAtAction("GetCompartment", new { id = compartment.Id }, compartmentDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCompartment(int id, [FromBody] Compartment compartment)
    {
        if (id != compartment.Id) return BadRequest(new ErrorResponse(400));

        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _compartmentService.UpdateAsync(compartment);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, ex.Message));
        }
        catch (BadRequestException ex)
        {
            var errorResponse = new ValidateInputError(400, new List<string> { ex.Message });
            return BadRequest(errorResponse);
        }

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> SoftDeleteCompartment(int id)
    {
        var compartment = await _compartmentService.GetByIdAsync(id);

        if (compartment is null) return NotFound(new ErrorResponse(404));

        await _compartmentService.SoftDeleteAsync(compartment);

        return NoContent();
    }
}
