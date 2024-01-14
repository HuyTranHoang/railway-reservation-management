namespace WebApi.Controllers
{
    public class BookingController : BaseApiController
    {
        private readonly IBookingService _bookingService;
        
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("booking-schedule")]
        public async Task<IActionResult> GetBookingSchedule([FromQuery] BookingQueryParams queryParams)
        {
            var schedulesDto = await _bookingService.GetBookingInfoAsync(queryParams);

            var paginationHeader = new PaginationHeader(queryParams.PageNumber, queryParams.PageSize,
            schedulesDto.TotalCount, schedulesDto.TotalPages);

            Response.AddPaginationHeader(paginationHeader);

            return Ok(schedulesDto);
        }
    }
}