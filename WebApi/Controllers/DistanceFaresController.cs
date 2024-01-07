using Domain.Exceptions;

namespace WebApi.Controllers;

public class DistanceFaresController : BaseApiController
{
    private readonly IDistanceFareService _distanceFareService;

    public DistanceFaresController(IDistanceFareService distanceFareService)
    {
        _distanceFareService = distanceFareService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DistanceFareDto>>> GetDistanceFares([FromQuery] DistanceFareQueryParams queryParams)
    {
        var distanceFaresDto = await _distanceFareService.GetAllDtoAsync(queryParams);

        var paginationHeader = new PaginationHeader(queryParams.PageNumber, queryParams.PageSize,
            distanceFaresDto.TotalCount, distanceFaresDto.TotalPages);

        Response.AddPaginationHeader(paginationHeader);

        return Ok(distanceFaresDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DistanceFareDto>> GetDistanceFare(int id)
    {
        var distanceFares = await _distanceFareService.GetDtoByIdAsync(id);

        if (distanceFares is null) return NotFound(new ErrorResponse(404));

        return Ok(distanceFares);
    }

    [HttpPost]
    public async Task<IActionResult> PostDistanceFare([FromBody] DistanceFare distanceFare)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _distanceFareService.AddAsync(distanceFare);
        }
        catch (BadRequestException ex)
        {
            var errorResponse = new ValidateInputError(400, new List<string> { ex.Message });
            return BadRequest(errorResponse);
        }

        return CreatedAtAction("GetDistanceFare", new { id = distanceFare.Id }, distanceFare);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutDistanceFare(int id, [FromBody] DistanceFare distanceFare)
    {
        if (id != distanceFare.Id) return BadRequest(new ErrorResponse(400));

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _distanceFareService.UpdateAsync(distanceFare);
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
    public async Task<IActionResult> SoftDeleteDistanceFare(int id)
    {
        var distanceFare = await _distanceFareService.GetByIdAsync(id);
        if (distanceFare is null) return NotFound(new ErrorResponse(404));

        await _distanceFareService.SoftDeleteAsync(distanceFare);

        return NoContent();
    }
}