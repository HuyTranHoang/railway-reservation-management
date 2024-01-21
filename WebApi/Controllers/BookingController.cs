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
        public async Task<ActionResult<List<ScheduleDto>>> GetBookingSchedule([FromQuery] BookingQueryParams queryParams)
        {
            var schedulesDto = await _bookingService.GetBookingInfoWithScheduleAsync(queryParams);

            return Ok(schedulesDto);
        }

        [HttpGet("schedule/{id}")]
        public async Task<ActionResult<List<object>>> GetScheduleById(int id)
        {
            var schedule = await _bookingService.GetBookingInfoWithScheduleIdAsync(id);
            var carriageTypes = await _bookingService.GetCarriageTypesByTrainIdAsync(schedule.TrainId);


            if (schedule == null || carriageTypes == null)
            {
                return NotFound(new ErrorResponse(404));
            }

            var result = new List<object> { schedule, carriageTypes };

            return Ok(result);
        }

        [HttpGet("carriageTypes/{id}")]
        public async Task<ActionResult> GetCarriageTypesByTrainId(int id)
        {
            var carriageTypes = await _bookingService.GetCarriageTypesByTrainIdAsync(id);

            if (carriageTypes is null) return NotFound(new ErrorResponse(404));

            return Ok(carriageTypes);
        }

        [HttpGet("train/{id}")]
        public async Task<ActionResult<List<object>>> GetTrainByScheduleId(int id)
        {
            var train = await _bookingService.GetTrainDetailsWithTrainIdAsync(id);

            if (train == null)
            {
                return NotFound(new ErrorResponse(404));
            }

            var result = new List<object> { train };

            return Ok(result);
        }

    }
}