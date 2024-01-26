using Application.Common.Models.Payments;
using Application.Services;
using Domain.Exceptions;
using Sek.Module.Payment.VnPay;
using Sek.Module.Payment.VnPay.AspNetCore;
using Sek.Module.Payment.VnPay.Common;

namespace WebApi.Controllers;

public class PaymentsController : BaseApiController
{
    private readonly IPaymentService _paymentService;
    private readonly IConfiguration _configuration;
    private readonly IVnPayService _vnPayService;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(IPaymentService paymentService,
                            IConfiguration configuration,
                            IVnPayService vnPayService,
                            ILogger<PaymentsController> logger)
    {
        _paymentService = paymentService;
        _configuration = configuration;
        _vnPayService = vnPayService;
        _logger = logger;
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
    public async Task<ActionResult> PostPayment([FromBody] Payment payment)
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
    public async Task<ActionResult> PutPayment(int id, [FromBody] Payment payment)
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

    [HttpPost("createUrlVnPay")]
    public IActionResult CreateUrlVnPay([FromBody] PaymentInformationModel model)
    {
        try
        {
            var paymentUrl = _vnPayService.CreatePaymentUrl(model, HttpContext);
            return Ok(new { PaymentUrl = paymentUrl });
        }
        catch (Exception ex)
        {
            // Ghi log chi tiết về ngoại lệ
        _logger.LogError(ex, "Đã xảy ra lỗi trong hành động CreateUrlVnPay.");
        
            // Log or handle the exception as needed
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
        }
    }

    [HttpPost("callback")]
    public IActionResult PaymentCallback()
    {
        try
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response.Success)
            {
                return Ok(new JsonResult(new { message = "Payment successful" }));
            }
            else
            {
                return BadRequest(new { Message = "Payment failed" });
            }
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
        }
    }
}
