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
        var compartmentDto = await _compartmentService.GetAllCompartmentDtoAsync(queryParams);

        var paginationHeader = new PaginationHeader(queryParams.PageNumber, queryParams.PageSize, compartmentDto.TotalCount, compartmentDto.TotalPages);

        Response.AddPaginationHeader(paginationHeader);

        return Ok(compartmentDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CompartmentDto>> GetCompartment(int id)
    {
        var compartmentDto = await _compartmentService.GetCompartmentDtoByIdAsync(id);

        if (compartmentDto is null) return NotFound(new ErrorResponse(404));

        return Ok(compartmentDto);
    }

    [HttpPost]
    public async Task<ActionResult> PostCompartment([FromBody] Compartment compartment)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            await _compartmentService.AddCompartmentAsync(compartment);
        }
        catch (BadRequestException ex)
        {
            var errorResponse = new ValidateInputError(400, new List<string> { ex.Message });
            return BadRequest(errorResponse);
        }

        return CreatedAtAction("GetCompartment", new { id = compartment.Id }, compartment);
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
            await _compartmentService.UpdateCompartmentAsync(compartment);
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
        var compartment = await _compartmentService.GetCompartmentByIdAsync(id);

        if (compartment is null) return NotFound(new ErrorResponse(404));

        await _compartmentService.SoftDeleteCompartmentAsync(compartment);

        return NoContent();
    }
}
