using System.Security.Claims;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Controllers;

public class PaymentsController : BaseApiController
{
    private readonly IPaymentService _paymentService;
    private readonly IConfiguration _configuration;
    private readonly IVnPayService _vnPayService;
    private readonly IHubContext<PaymentHub> _hubContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(IPaymentService paymentService,
        IConfiguration configuration,
        IVnPayService vnPayService,
        IHubContext<PaymentHub> hubContext,
        UserManager<ApplicationUser> userManager,
        ILogger<PaymentsController> logger)
    {
        _paymentService = paymentService;
        _configuration = configuration;
        _vnPayService = vnPayService;
        _hubContext = hubContext;
        _userManager = userManager;
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

    [HttpPost("addPaymentByEmail/{email}")]
    public async Task<ActionResult> PostPaymentByEmail(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) return NotFound(new ErrorResponse(404));

        var payment = new Payment
        {
            AspNetUserId = user.Id,
            Status = "Success"
        };

        try
        {
            await _paymentService.AddAsync(payment);
        }
        catch (BadRequestException ex)
        {
            var errorResponse = new ValidateInputError(400, new List<string> { ex.Message });
            return BadRequest(errorResponse);
        }

        return Ok(new { paymentId = payment.Id, message = "Add payment successful" });
        // return CreatedAtAction("GetPayment", new { id = payment.Id }, payment);
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

    [Authorize]
    [HttpPost("createUrlVnPay")]
    public async Task<IActionResult> CreateUrlVnPay([FromBody] PaymentInformationModel model)
    {
        var user = await _userManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Email));
        if (user == null) return Unauthorized("Invalid username or password");
        try
        {
            var paymentUrl = _vnPayService.CreatePaymentUrl(model, HttpContext, user);
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

    [HttpGet("callback")]
    public async Task<IActionResult> PaymentCallback()
    {
        try
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response.Success)
            {
                if (response.VnPayResponseCode == "00")
                {
                    // await _hubContext.Clients.All.SendAsync("PaymentStatus", "PaymentSuccess");
                    await _hubContext.Clients.Group(response.OrderDescription).SendAsync("PaymentStatus", "PaymentSuccess");

                    // await _hubContext.Clients.User(response.OrderDescription).SendAsync("PaymentStatus", "PaymentSuccess");
                    return Content("<html><head></head><body><script>window.close();</script></body></html>", "text/html");
                    // return Ok(new { message = "Payment success" });
                }

                if (response.VnPayResponseCode == "24")
                {
                    await _hubContext.Clients.All.SendAsync("PaymentStatus", "PaymentCancel");
                    return Content("<html><head></head><body><script>window.close();</script></body></html>", "text/html");
                }

                await _hubContext.Clients.All.SendAsync("PaymentStatus", "PaymentPending");
            }


            await _hubContext.Clients.All.SendAsync("PaymentStatus", "PaymentFailed");
            return BadRequest(new { Message = "Payment failed" });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
        }
    }
}