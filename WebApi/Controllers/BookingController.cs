using Domain.Exceptions;
using Newtonsoft.Json;

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
            // Chuyển đối tượng thành một dạng có thể lưu trữ (ở đây sử dụng JSON)
            var queryParamsJson = JsonConvert.SerializeObject(queryParams);
            // Lưu trữ vào Session
            HttpContext.Session.SetString("BookingParams", queryParamsJson);

            var schedulesDto = await _bookingService.GetBookingInfoWithScheduleAsync(queryParams);

            return Ok(schedulesDto);
        }

        [HttpGet("schedule/{id}")]
        public async Task<ActionResult<List<object>>> GetScheduleById(int id)
        {
            // Lấy giá trị queryParams từ Session
            var queryParamsJson = HttpContext.Session.GetString("BookingParams");

            if (string.IsNullOrEmpty(queryParamsJson))
            {
                return BadRequest("BookingRoundTripParams is missing in session.");
            }

            // Chuyển đổi từ dạng đã lưu trữ về đối tượng ban đầu
            var queryParams = JsonConvert.DeserializeObject<BookingQueryParams>(queryParamsJson);


            var schedule = await _bookingService.GetBookingInfoWithScheduleIdAsync(id);
            var carriageTypes = await _bookingService.GetCarriageTypesByTrainIdAsync(schedule.TrainId);


            if (schedule == null || carriageTypes == null)
            {
                return NotFound(new ErrorResponse(404));
            }

            var result = new {
                Schedule = schedule,
                CarriageTypes = carriageTypes,
                BookingParams = queryParams
            };

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
        public async Task<ActionResult<List<object>>> GetTrainDetailsByScheduleId(int id)
        {
            // Lấy giá trị queryParams từ Session
            var queryParamsJson = HttpContext.Session.GetString("BookingParams");

            if (string.IsNullOrEmpty(queryParamsJson))
            {
                return BadRequest("BookingRoundTripParams is missing in session.");
            }

            // Chuyển đổi từ dạng đã lưu trữ về đối tượng ban đầu
            var queryParams = JsonConvert.DeserializeObject<BookingQueryParams>(queryParamsJson);

            var train = await _bookingService.GetTrainDetailsWithTrainIdAsync(id);

            if (train == null)
            {
                return NotFound(new ErrorResponse(404));
            }

            var result = new {
                Train = train,
                BookingParams = queryParams

            };

            return Ok(result);
        }

    }
}