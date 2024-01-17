using Domain.Exceptions;

namespace WebApi.Controllers;

public class CarriageTypesController : BaseApiController
{
    private readonly ICarriageTypeService _carriageTypeService;


    public CarriageTypesController(ICarriageTypeService carriageTypeService)
    {
        _carriageTypeService = carriageTypeService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CarriageTypeDto>>> GetCarriageTypes([FromQuery] QueryParams queryParams)
    {
        var carriageTypeDto = await _carriageTypeService.GetAllDtoAsync(queryParams);

        var paginationHeader = new PaginationHeader(queryParams.PageNumber, queryParams.PageSize,
            carriageTypeDto.TotalCount, carriageTypeDto.TotalPages);

        Response.AddPaginationHeader(paginationHeader);

        return Ok(carriageTypeDto);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<CarriageTypeDto>>> GetAllCarriageTypes()
    {
        var carriageTypeDto = await _carriageTypeService.GetAllDtoNoPagingAsync();

        return Ok(carriageTypeDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CarriageTypeDto>> GetCarriageType(int id)
    {
        var carriageTypeDto = await _carriageTypeService.GetDtoByIdAsync(id);

        if (carriageTypeDto is null) return NotFound(new ErrorResponse(404));

        return Ok(carriageTypeDto);
    }

    [HttpPost]
    public async Task<ActionResult> AddCarriageType([FromBody] CarriageType carriageType)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        await _carriageTypeService.AddAsync(carriageType);
        return CreatedAtAction("GetCarriageType", new { id = carriageType.Id }, carriageType);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCarriageType(int id, [FromBody] CarriageType carriageType)
    {
        if (id != carriageType.Id) return BadRequest(new ErrorResponse(400));

        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            await _carriageTypeService.UpdateAsync(carriageType);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, ex.Message));
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCarriageType(int id)
    {
        var carriageType = await _carriageTypeService.GetByIdAsync(id);

        if (carriageType == null) return NotFound(new ErrorResponse(404));

        await _carriageTypeService.DeleteAsync(carriageType);

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> SoftDeleteCarriageType(int id)
    {
        var carriageType = await _carriageTypeService.GetByIdAsync(id);

        if (carriageType is null) return NotFound(new ErrorResponse(404));

        await _carriageTypeService.SoftDeleteAsync(carriageType);

        return Ok();
    }
}