using Domain.Exceptions;
using Microsoft.AspNetCore.Components.Forms;
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
            var queryParamsJson = JsonConvert.SerializeObject(queryParams);// Chuyển đối tượng thành JSON
            HttpContext.Session.SetString("BookingParams", queryParamsJson);// Lưu trữ vào Session

            var schedulesDto = await _bookingService.GetBookingInfoWithScheduleAsync(queryParams);

            var scheduleJson = JsonConvert.SerializeObject(schedulesDto);
            HttpContext.Session.SetString("ScheduleParams", queryParamsJson);

            var result = new {
                Schedule = schedulesDto,
                BookingParams = queryParamsJson,
                ScheduleParams = scheduleJson
            };

            return Ok(result);
        }

        [HttpGet("schedule/{id}")]
        public async Task<ActionResult<List<object>>> GetScheduleById(int id)
        {
            // Lấy giá trị queryParams từ Session
            var queryParamsJson = HttpContext.Session.GetString("BookingParams");

            if (string.IsNullOrEmpty(queryParamsJson))
            {
                return BadRequest("BookingParams is missing in session.");
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

        // [HttpGet("carriageTypes/{id}")]
        // public async Task<ActionResult> GetCarriageTypesByTrainId(int id)
        // {
        //     var carriageTypes = await _bookingService.GetCarriageTypesByTrainIdAsync(id);

        //     if (carriageTypes is null) return NotFound(new ErrorResponse(404));

        //     return Ok(carriageTypes);
        // }

        [HttpGet("train/{id}")]
        public async Task<ActionResult<List<object>>> GetTrainDetailsByScheduleId(int id)
        {
            var queryParamsJson = HttpContext.Session.GetString("BookingParams");

            if (string.IsNullOrEmpty(queryParamsJson))
            {
                return BadRequest("BookingRoundTripParams is missing in session.");
            }
            var queryParams = JsonConvert.DeserializeObject<BookingQueryParams>(queryParamsJson);

            var train = await _bookingService.GetTrainDetailsWithTrainIdAsync(id);

            var trainDetailsJson = JsonConvert.SerializeObject(queryParams);
            HttpContext.Session.SetString("TrainDetails", trainDetailsJson);

            var scheduleJson = JsonConvert.SerializeObject(id);
            HttpContext.Session.SetString("ScheduleId", scheduleJson);

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

        [HttpPost("payment")]
        public async Task<IActionResult> Payment([FromBody] PaymentDto paymentDto, string userId, int carriageId, int seatId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var queryParamsJson = HttpContext.Session.GetString("BookingParams");

            if (string.IsNullOrEmpty(queryParamsJson))
            {
                return BadRequest("BookingRoundTripParams is missing in session.");
            }
            var queryParams = JsonConvert.DeserializeObject<BookingQueryParams>(queryParamsJson);
            //

            // var trainDetailsJson = HttpContext.Session.GetString("TrainDetails");

            // if (string.IsNullOrEmpty(trainDetailsJson))
            // {
            //     return BadRequest("TrainDetails is missing in session.");
            // }
            //

            var scheduleParamsJson = HttpContext.Session.GetString("ScheduleParams");

            if (string.IsNullOrEmpty(scheduleParamsJson))
            {
                return BadRequest("ScheduleParams is missing in session.");
            }

            // TrainDetailsDto trainDetails = null;

            // try
            // {
            //     trainDetails = JsonConvert.DeserializeObject<TrainDetailsDto>(trainDetailsJson);
            // }
            // catch (JsonException ex)
            // {
            //     return BadRequest("Error deserializing TrainDetails. " + ex.Message);
            // }

            ScheduleDto schedule = null;
            try
            {
                schedule = JsonConvert.DeserializeObject<ScheduleDto>(scheduleParamsJson);
            }
            catch (JsonException ex)
            {
                return BadRequest("Error deserializing ScheduleId. " + ex.Message);
            }
            
            //Kiểm tra RoundTrip
            if (queryParams.RoundTrip)
            {   
                BookingQueryParams roundTripParams = new BookingQueryParams
                {
                    DepartureStationId = queryParams.ArrivalStationId,
                    ArrivalStationId = queryParams.DepartureStationId,
                    DepartureTime = queryParams.ArrivalTime,
                    ArrivalTime = null,
                    RoundTrip = false
                };

                var schedulesDto = await GetBookingSchedule(roundTripParams);

                // ...

                return Ok("Payment successful.");
            } else
            {
                try
                {
                    var passenger = await _bookingService.AddPassengerAsync(paymentDto.Passenger);

                    paymentDto.Payment.AspNetUserId = userId;
                    var payment = await _bookingService.AddPaymentAsync(paymentDto.Payment);

                    paymentDto.Ticket.PassengerId = passenger.Id;
                    paymentDto.Ticket.TrainId = schedule.TrainId;
                    paymentDto.Ticket.DistanceFareId = payment.Ticket.DistanceFareId;
                    paymentDto.Ticket.CarriageId = carriageId;
                    paymentDto.Ticket.SeatId = seatId;
                    paymentDto.Ticket.ScheduleId = schedule.Id;
                    paymentDto.Ticket.PaymentId = payment.Payment.Id;
                    await _bookingService.AddTicketAsync(paymentDto.Ticket);
                }
                catch (Exception ex)
                {
                    return BadRequest($"Error processing payment: {ex.Message}");
                }
            return Ok("Payment successful.");
            }
        }
    }
}