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
        public async Task<IActionResult> GetBookingSchedule([FromQuery] BookingQueryParams queryParams)
        {
            var schedulesDto = await _bookingService.GetBookingInfoWithScheduleAsync(queryParams);

            var paginationHeader = new PaginationHeader(queryParams.PageNumber, queryParams.PageSize,
            schedulesDto.TotalCount, schedulesDto.TotalPages);

            Response.AddPaginationHeader(paginationHeader);

            return Ok(schedulesDto);
        }

        [HttpGet("schedule/{id}")]
        public async Task<ActionResult<List<object>>> GetScheduleById(int id)
        {
            var schedules = await _bookingService.GetBookingInfoWithScheduleIdAsync(id);
            var carriageTypes = await _bookingService.GetAllCarriageTypeDtoAsync();

            if (schedules == null || carriageTypes == null)
            {
                return NotFound(new ErrorResponse(404));
            }

            var result = new List<object> { schedules, carriageTypes };

            return Ok(result);
        }

        [HttpGet("carriageTypes")]
        public async Task<ActionResult> GetCarriageTypes()
        {
            var carriageTypes = await _bookingService.GetAllCarriageTypeDtoAsync();

            if (carriageTypes is null) return NotFound(new ErrorResponse(404));

            return Ok(carriageTypes);
        }

        [HttpGet("train/{id}")]
        public async Task<ActionResult<List<object>>> GetTrainByScheduleId(int id)
        {
            var train = await _bookingService.GetTrainInfoWithTrainIdAsync(id);

            if (train == null)
            {
                return NotFound(new ErrorResponse(404));
            }

            var result = new List<object> { train };

            return Ok(result);
        }

        [HttpGet("carriage/{id}")]
        public async Task<ActionResult> GetCarriagesByTrainId(int id)
        {
            var carriages = await _bookingService.GetCarriagesWithTrainIdAsync(id);

            if (carriages == null)
            {
                return NotFound(new ErrorResponse(404));
            }

            return Ok(carriages);
        }

        [HttpGet("compartment/{id}")]
        public async Task<ActionResult> GetCompartmentsByCarriageId(int id)
        {
            var compartments = await _bookingService.GetCompartmentsWithCarriageIdAsync(id);

            if (compartments == null)
            {
                return NotFound(new ErrorResponse(404));
            }

            return Ok(compartments);
        }

        [HttpGet("seat/{id}")]
        public async Task<ActionResult> GetSeatsByCompartmentId(int id)
        {
            var seats = await _bookingService.GetSeatsWithCompartmentIdAsync(id);

            if (seats == null)
            {
                return NotFound(new ErrorResponse(404));
            }

            return Ok(seats);
        }
    }
}