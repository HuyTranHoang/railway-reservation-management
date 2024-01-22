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

        [HttpGet("train/{id}")]
        public async Task<ActionResult<List<object>>> GetTrainDetailsByScheduleId(int id)
        {
            var queryParamsJson = HttpContext.Session.GetString("BookingParams");

            if (string.IsNullOrEmpty(queryParamsJson))
            {
                return BadRequest("BookingRoundTripParams is missing in session.");
            }
            var queryParams = JsonConvert.DeserializeObject<BookingQueryParams>(queryParamsJson);
            
            
            var schedule = await _bookingService.GetBookingInfoWithScheduleIdAsync(id);
            var train = await _bookingService.GetTrainDetailsWithTrainIdAsync(schedule.TrainId);

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
            var scheduleParamsJson = HttpContext.Session.GetString("ScheduleParams");

            if (string.IsNullOrEmpty(scheduleParamsJson))
            {
                return BadRequest("ScheduleParams is missing in session.");
            }

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
                    paymentDto.Tickets = new List<Ticket>();
                    if (paymentDto.Passengers == null || !paymentDto.Passengers.Any())
                    {
                        return BadRequest("The list of passengers is null or empty.");
                    }

                    foreach (var passengerData in paymentDto.Passengers)
                    {
                        var passenger = await _bookingService.AddPassengerAsync(passengerData);
                        paymentDto.Payments.AspNetUserId = userId;
                        paymentDto.Payments ??= new Payment();

                        var payment = await _bookingService.AddPaymentAsync(paymentDto.Payments);

                        var ticket = new Ticket
                        {
                            PassengerId = passenger.Id,
                            TrainId = schedule.TrainId,
                            DistanceFareId = payment.Ticket.DistanceFareId,
                            CarriageId = carriageId,
                            SeatId = seatId,
                            ScheduleId = schedule.Id,
                            PaymentId = payment.Id
                        };
                        paymentDto.Tickets.Add(ticket);
                    }

                    if (paymentDto.Tickets.Any())
                    {
                        await _bookingService.AddTicketListAsync(paymentDto.Tickets);
                    }
                    else
                    {
                        return BadRequest("The list of tickets is empty.");
                    }
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