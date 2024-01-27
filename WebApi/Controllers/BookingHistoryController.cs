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
            try
            {
                var bookingHistory = await _bookingHistoryService.GetBookingHistoryDtoAsync(id);
                return Ok(bookingHistory);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}