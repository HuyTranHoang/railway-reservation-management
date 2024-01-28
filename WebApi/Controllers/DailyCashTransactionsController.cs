using Domain.Exceptions;

namespace WebApi.Controllers;

public class DailyCashTransactionsController : BaseApiController
{
    private readonly IDailyCashTransactionService _dailyCashTransactionService;


    public DailyCashTransactionsController(IDailyCashTransactionService dailyCashTransactionService)
    {
        _dailyCashTransactionService = dailyCashTransactionService;
    }

    [HttpGet]
    public async Task<ActionResult<List<DailyCashTransactionDto>>> GetDailyCashTransactions([FromQuery] QueryParams queryParams)
    {
        var dailyCashTransactionDto = await _dailyCashTransactionService.GetAllDtoAsync(queryParams);

        var paginationHeader = new PaginationHeader(queryParams.PageNumber, queryParams.PageSize,
            dailyCashTransactionDto.TotalCount, dailyCashTransactionDto.TotalPages);

        Response.AddPaginationHeader(paginationHeader);

        return Ok(dailyCashTransactionDto);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<DailyCashTransactionDto>>> GetAllDailyCashTransaction()
    {
        var dailyCashTransactionDto = await _dailyCashTransactionService.GetAllDtoNoPagingAsync();

        return Ok(dailyCashTransactionDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DailyCashTransactionDto>> GetDailyCashTransaction(int id)
    {
        var dailyCashTransactionDto = await _dailyCashTransactionService.GetDtoByIdAsync(id);

        if (dailyCashTransactionDto is null) return NotFound(new ErrorResponse(404));

        return Ok(dailyCashTransactionDto);
    }
}