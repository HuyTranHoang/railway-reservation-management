namespace WebApi.Controllers;

public class TrainCompaniesController : BaseApiController
{
    private readonly ITrainCompanyService _trainCompanySerivce;

    public TrainCompaniesController(ITrainCompanyService trainCompanySerivce)
    {
        _trainCompanySerivce = trainCompanySerivce;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrainCompanyDto>>> GetTrainCompanies(
        [FromQuery] QueryParams queryParams)
    {
        var trainCompaniesDto = await _trainCompanySerivce.GetAllCompanyDtoAsync(queryParams);

        var paginationHeader = new PaginationHeader(
            trainCompaniesDto.CurrentPage, trainCompaniesDto.PageSize,
            trainCompaniesDto.TotalCount, trainCompaniesDto.TotalPages);

        Response.AddPaginationHeader(paginationHeader);

        return Ok(trainCompaniesDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TrainCompanyDto>> GetTrainCompany(int id)
    {
        var trainCompanies = await _trainCompanySerivce.GetCompanyDtoByIdAsync(id);

        if (trainCompanies is null)
            return NotFound(new ErrorResponse(404));

        return Ok(trainCompanies);
    }

    [HttpPost]
    public async Task<IActionResult> PostTrainCompany([FromBody] TrainCompany trainCompany)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            await _trainCompanySerivce.AddCompanyAsync(trainCompany);
        }
        catch (BadRequestException ex)
        {
            var errorResponse = new ValidateInputError(400, new List<string> { ex.Message });
            return BadRequest(errorResponse);
        }

        return CreatedAtAction("GetTrainCompany", new { id = trainCompany.Id }, trainCompany);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTrainCompany(int id, [FromBody] TrainCompany trainCompany)
    {
        if (id != trainCompany.Id) return BadRequest(new ErrorResponse(400));

        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            await _trainCompanySerivce.UpdateCompanyAsync(trainCompany);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, ex.Message));
        }

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> SoftDeleteTrainCompany(int id)
    {
        var trainCompany = await _trainCompanySerivce.GetCompanyByIdAsync(id);

        if (trainCompany is null) return NotFound(new ErrorResponse(404));

        await _trainCompanySerivce.SoftDeleteCompanyAsync(trainCompany);

        return NoContent();
    }
}