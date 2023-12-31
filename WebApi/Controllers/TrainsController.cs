using Domain.Exceptions;

namespace WebApi.Controllers;

public class TrainsController : BaseApiController
{
    private readonly ITrainService _trainService;

    public TrainsController(ITrainService trainService)
    {
        _trainService = trainService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrainDto>>> GetTrains([FromQuery] TrainQueryParams queryParams)
    {
        var trainsDto = await _trainService.GetAllDtoAsync(queryParams);

        var paginationHeader = new PaginationHeader(queryParams.PageNumber, queryParams.PageSize,
            trainsDto.TotalCount, trainsDto.TotalPages);

        Response.AddPaginationHeader(paginationHeader);

        return Ok(trainsDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TrainDto>> GetTrain(int id)
    {
        var trains = await _trainService.GetDtoByIdAsync(id);

        if (trains is null) return NotFound(new ErrorResponse(404));

        return Ok(trains);
    }

    [HttpPost]
    public async Task<IActionResult> PostTrain([FromBody] Train train)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _trainService.AddAsync(train);
        }
        catch (BadRequestException ex)
        {
            var errorResponse = new ValidateInputError(400, new List<string> { ex.Message });
            return BadRequest(errorResponse);
        }

        return CreatedAtAction("GetTrain", new { id = train.Id }, train);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTrainCompany(int id, [FromBody] Train train)
    {
        if (id != train.Id) return BadRequest(new ErrorResponse(400));

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _trainService.UpdateAsync(train);
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
    public async Task<IActionResult> SoftDeleteTrain(int id)
    {
        var train = await _trainService.GetByIdAsync(id);
        if (train is null) return NotFound(new ErrorResponse(404));

        await _trainService.SoftDeleteAsync(train);

        return NoContent();
    }
}