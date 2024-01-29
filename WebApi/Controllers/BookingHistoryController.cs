using Domain.Exceptions;

namespace WebApi.Controllers
{
    public class BookingHistoryController : BaseApiController
    {
        private readonly IBookingHistoryService _bookingHistoryService;

        public BookingHistoryController(IBookingHistoryService bookingHistoryService)
        {
            _bookingHistoryService = bookingHistoryService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<BookingHistoryDto>>> GetBookingHistoryById(string id)
        {
            var bookingHistory = await _bookingHistoryService.GetBookingHistoryDtoAsync(id);

            if (bookingHistory is null)
            {
                return NotFound(new ErrorResponse(404));
            }

            return Ok(bookingHistory);
        }

        [HttpPost]
        public async Task<ActionResult> CancelTicket([FromBody] Cancellation cancellation)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _bookingHistoryService.CanncleTicket(cancellation);
            }
            catch (BadRequestException ex)
            {
                var errorResponse = new ValidateInputError(400, new List<string> { ex.Message });
                return BadRequest(errorResponse);
            }

            return Ok(new JsonResult(new { message = "Cancellation added successfully" }));
        }
    }
}