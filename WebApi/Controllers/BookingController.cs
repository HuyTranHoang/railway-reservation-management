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

            var result = new {
                Schedule = schedulesDto,
                BookingParams = queryParamsJson
            };

            return Ok(result);
        }

        [HttpGet("schedule/{id}")]
        public async Task<ActionResult<List<object>>> GetTrainDetailsByScheduleId(int id)
        {
            // var schedule = await _bookingService.GetBookingInfoWithScheduleIdAsync(id);
            var train = await _bookingService.GetTrainDetailsByScheduleIdAsync(id);

            var trainDetailsJson = JsonConvert.SerializeObject(train);// Chuyển đối tượng thành JSON
            HttpContext.Session.SetString("TrainDetails", trainDetailsJson);// Lưu trữ vào Session

            if (train == null)
            {
                return NotFound(new ErrorResponse(404));
            }

            return Ok(train);
        }

        [HttpPost("payment")]
        public async Task<IActionResult> Payment([FromBody] PaymentDto paymentDto, string userId, int carriageId, int seatId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var queryParamsJson = HttpContext.Session.GetString("BookingParams");
            if (string.IsNullOrEmpty(queryParamsJson))
            {
                return BadRequest("BookingParams is missing in session.");
            }
            var queryParams = JsonConvert.DeserializeObject<BookingQueryParams>(queryParamsJson);

            var trainDetailsJson = HttpContext.Session.GetString("TrainDetails");
            if (string.IsNullOrEmpty(trainDetailsJson))
            {
                return BadRequest("TrainDetails is missing in session.");
            }
            var trainDetails = JsonConvert.DeserializeObject<BookingQueryParams>(trainDetailsJson);

            ScheduleDto schedule = null;
            
            //Kiểm tra RoundTrip
            if (queryParams.RoundTrip == true)
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