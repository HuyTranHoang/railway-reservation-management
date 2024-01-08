using Application.Services;
using Domain.Exceptions;

namespace WebApi.Controllers;

public class PaymentsController : BaseApiController
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPayments([FromQuery] PaymentQueryParams queryParams)
    {
        var paymentsDto = await _paymentService.GetAllDtoAsync(queryParams);

        var paginationHeader = new PaginationHeader(queryParams.PageNumber, queryParams.PageSize,
            paymentsDto.TotalCount, paymentsDto.TotalPages);

        Response.AddPaginationHeader(paginationHeader);

        return Ok(paymentsDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PaymentDto>> GetPayment(int id)
    {
        var payments = await _paymentService.GetDtoByIdAsync(id);

        if (payments is null) return NotFound(new ErrorResponse(404));

        return Ok(payments);
    }

    [HttpPost]
    public async Task<IActionResult> PostPayment([FromBody] Payment payment)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _paymentService.AddAsync(payment);
        }
        catch (BadRequestException ex)
        {
            var errorResponse = new ValidateInputError(400, new List<string> { ex.Message });
            return BadRequest(errorResponse);
        }

        return CreatedAtAction("GetPayment", new { id = payment.Id }, payment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPayment(int id, [FromBody] Payment payment)
    {
        if (id != payment.Id) return BadRequest(new ErrorResponse(400));

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _paymentService.UpdateAsync(payment);
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
    public async Task<IActionResult> SoftDeletePayment(int id)
    {
        var payment = await _paymentService.GetByIdAsync(id);
        if (payment is null) return NotFound(new ErrorResponse(404));

        await _paymentService.SoftDeleteAsync(payment);

        return NoContent();
    }
}
