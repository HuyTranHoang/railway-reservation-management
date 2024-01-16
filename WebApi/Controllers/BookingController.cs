using Domain.Exceptions;

namespace WebApi.Controllers
{
    public class BookingController : BaseApiController
    {
        private readonly IBookingService _bookingService;
        
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("schedule")]
        public async Task<ActionResult> GetBookingSchedule([FromQuery] BookingQueryParams queryParams)
        {
            var schedulesDto = await _bookingService.GetBookingInfoWithScheduleAsync(queryParams);

            var paginationHeader = new PaginationHeader(queryParams.PageNumber, queryParams.PageSize,
            schedulesDto.TotalCount, schedulesDto.TotalPages);

            Response.AddPaginationHeader(paginationHeader);

            return Ok(schedulesDto);
        }

        [HttpGet("schedule/{id}")]
        public async Task<ActionResult<object[]>> GetScheduleById(int id)
        {
            var schedules = await _bookingService.GetBookingInfoWithScheduleIdAsync(id);
            var carriageTypes = await _bookingService.GetAllCarriageTypeDtoAsync();

            if (schedules == null || carriageTypes == null)
            {
                return NotFound(new ErrorResponse(404));
            }

            var result = new object[] { schedules, carriageTypes };

            return Ok(result);
        }

        [HttpGet("carriageTypes")]
        public async Task<ActionResult> GetCarriageTypes()
        {
            var carriageTypes = await _bookingService.GetAllCarriageTypeDtoAsync();

            if (carriageTypes is null) return NotFound(new ErrorResponse(404));

            return Ok(carriageTypes);
        }

    }
}