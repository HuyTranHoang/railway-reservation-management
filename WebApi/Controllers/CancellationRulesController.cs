using Domain.Exceptions;

namespace WebApi.Controllers;

public class CancellationRulesController : BaseApiController
{
    private readonly ICancellationRuleService _cancellationRuleService;

    public CancellationRulesController(ICancellationRuleService cancellationRuleService)
    {
        _cancellationRuleService = cancellationRuleService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CancellationRuleDto>>> GetCancellationRules([FromQuery] QueryParams queryParams)
    {
        var cancellationRulesDto = await _cancellationRuleService.GetAllDtoAsync(queryParams);

        var paginationHeader = new PaginationHeader(queryParams.PageNumber, queryParams.PageSize,
            cancellationRulesDto.TotalCount, cancellationRulesDto.TotalPages);

        Response.AddPaginationHeader(paginationHeader);

        return Ok(cancellationRulesDto);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<CancellationRuleDto>>> GetAllCancellationRules()
    {
        var cancellationRulesDto = await _cancellationRuleService.GetAllDtoNoPagingAsync();

        return Ok(cancellationRulesDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CancellationRuleDto>> GetCancellationRule(int id)
    {
        var cancellationRule = await _cancellationRuleService.GetDtoByIdAsync(id);

        if (cancellationRule is null) return NotFound(new ErrorResponse(404));

        return Ok(cancellationRule);
    }

    [HttpPost]
    public async Task<ActionResult> AddCancellationRule([FromBody] CancellationRule cancellationRule)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        await _cancellationRuleService.AddAsync(cancellationRule);
        return CreatedAtAction("GetCancellationRule", new { id = cancellationRule.Id }, cancellationRule);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCancellationRule(int id, [FromBody] CancellationRule cancellationRule)
    {
        if (id != cancellationRule.Id) return BadRequest(new ErrorResponse(400));

        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            await _cancellationRuleService.UpdateAsync(cancellationRule);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, ex.Message));
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCancellationRule(int id)
    {
        var cancellationRule = await _cancellationRuleService.GetByIdAsync(id);

        if (cancellationRule == null) return NotFound(new ErrorResponse(404));

        await _cancellationRuleService.DeleteAsync(cancellationRule);

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> SoftDeleteCancellationRule(int id)
    {
        var cancellationRule = await _cancellationRuleService.GetByIdAsync(id);

        if (cancellationRule is null) return NotFound(new ErrorResponse(404));

        await _cancellationRuleService.SoftDeleteAsync(cancellationRule);

        return Ok();
    }
}