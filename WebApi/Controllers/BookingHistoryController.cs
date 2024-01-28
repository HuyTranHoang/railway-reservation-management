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

    }
}