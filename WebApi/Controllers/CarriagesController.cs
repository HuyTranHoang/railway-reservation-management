using Domain.Exceptions;

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
        var carriagesDto = await _carriageService.GetAllDtoAsync(queryParams);

        var paginationHeader = new PaginationHeader(queryParams.PageNumber, queryParams.PageSize, 
        carriagesDto.TotalCount, carriagesDto.TotalPages);

        Response.AddPaginationHeader(paginationHeader);

        return Ok(carriagesDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CarriageDto>> GetCarriage(int id)
    {
        var carriagesDto = await _carriageService.GetDtoByIdAsync(id);

        if (carriagesDto is null) return NotFound(new ErrorResponse(404));

        return Ok(carriagesDto);
    }

    [HttpPost]
    public async Task<ActionResult> PostCarriage([FromBody] Carriage carriage)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        
        try
        {
            await _carriageService.AddAsync(carriage);
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

        if(!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            await _carriageService.UpdateAsync(carriage);
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
        var carriage = await _carriageService.GetByIdAsync(id);

        if(carriage == null) return NotFound(new ErrorResponse(404));

        await _carriageService.SoftDeleteAsync(carriage);

        return NoContent();
    }
}
