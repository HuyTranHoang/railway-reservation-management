using System.Security.Claims;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    public class BookingController : BaseApiController
    {
        private readonly IBookingService _bookingService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITicketService _ticketService;

        public BookingController(IBookingService bookingService,
            UserManager<ApplicationUser> userManager,
            ITicketService ticketService)
        {
            _bookingService = bookingService;
            _userManager = userManager;
            _ticketService = ticketService;
        }

        [HttpGet("schedule")]
        public async Task<ActionResult<List<ScheduleDto>>> GetBookingSchedule(
            [FromQuery] BookingQueryParams queryParams)
        {
            var queryParamsJson = JsonConvert.SerializeObject(queryParams); // Chuyển đối tượng thành JSON
            HttpContext.Session.SetString("BookingParams", queryParamsJson); // Lưu trữ vào Session

            var schedulesDto = await _bookingService.GetBookingInfoWithScheduleAsync(queryParams);

            var result = new
            {
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

            var trainDetailsJson = JsonConvert.SerializeObject(train); // Chuyển đối tượng thành JSON
            HttpContext.Session.SetString("TrainDetails", trainDetailsJson); // Lưu trữ vào Session

            var scheduleIdJson = JsonConvert.SerializeObject(id); // Chuyển đối tượng thành JSON
            HttpContext.Session.SetString("ScheduleId", scheduleIdJson); // Lưu trữ vào Session

            if (train == null)
            {
                return NotFound(new ErrorResponse(404));
            }

            return Ok(train);
        }


        [Authorize]
        [HttpPost("payment")]
        public async Task<IActionResult> Payment([FromBody] PaymentTransactionDto paymentDto)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Email));

            if (user == null) return Unauthorized("Invalid username or password");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!paymentDto.Passengers.Any())
            {
                return BadRequest("The list of passengers is null or empty.");
            }

            if (paymentDto.Passengers.Count != paymentDto.Tickets.Count)
            {
                return BadRequest("Mismatched number of passengers and tickets.");
            }

            for (int i = 0; i < paymentDto.Passengers.Count; i++)
            {
                var passengerData = paymentDto.Passengers[i];
                var ticketData = paymentDto.Tickets[i];

                var passengerToAdd = new Passenger
                {
                    FullName = passengerData.FullName,
                    CardId = passengerData.CardId,
                    Gender = passengerData.Gender,
                    Age = passengerData.Age,
                    Phone = passengerData.Phone,
                    Email = passengerData.Email
                };

                var passenger = await _bookingService.AddPassengerAsync(passengerToAdd);

                var ticket = new Ticket
                {
                    PassengerId = passenger.Id,
                    TrainId = paymentDto.TrainId,
                    CarriageId = ticketData.CarriageId,
                    SeatId = ticketData.SeatId,
                    ScheduleId = paymentDto.ScheduleId,
                    PaymentId = paymentDto.PaymentId
                };

                await _ticketService.AddAsync(ticket);
            }

            return Ok(new JsonResult(new {message = "Payment successful."}));
        }

        // [Authorize]
        // [HttpPost("payment")]
        // public async Task<IActionResult> Payment([FromBody] PaymentTransactionDto paymentDto)
        // {
        //     var user = await _userManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Email));
        //
        //     if (user == null) return Unauthorized("Invalid username or password");
        //
        //     if (!ModelState.IsValid) return BadRequest(ModelState);
        //
        //     // var queryParamsJson = HttpContext.Session.GetString("BookingParams");
        //     // if (string.IsNullOrEmpty(queryParamsJson))
        //     // {
        //     //     return BadRequest("BookingParams is missing in session.");
        //     // }
        //     //
        //     // var queryParams = JsonConvert.DeserializeObject<BookingQueryParams>(queryParamsJson);
        //     //
        //     // var trainDetailsJson = HttpContext.Session.GetString("TrainDetails");
        //     // if (string.IsNullOrEmpty(trainDetailsJson))
        //     // {
        //     //     return BadRequest("TrainDetails is missing in session.");
        //     // }
        //     //
        //     // var trainDetails = JsonConvert.DeserializeObject<TrainDetailDto>(trainDetailsJson);
        //     //
        //     // var scheduleIdJson = HttpContext.Session.GetString("ScheduleId");
        //     // if (string.IsNullOrEmpty(scheduleIdJson))
        //     // {
        //     //     return BadRequest("ScheduleId is missing in session.");
        //     // }
        //     //
        //     // var scheduleId = JsonConvert.DeserializeObject<int>(scheduleIdJson);
        //
        //
        //     //Kiểm tra RoundTrip
        //     // if (queryParams.RoundTrip == true)
        //     // {
        //     //     BookingQueryParams roundTripParams = new BookingQueryParams
        //     //     {
        //     //         DepartureStationId = queryParams.ArrivalStationId,
        //     //         ArrivalStationId = queryParams.DepartureStationId,
        //     //         DepartureTime = queryParams.ArrivalTime,
        //     //         ArrivalTime = null,
        //     //         RoundTrip = false
        //     //     };
        //     //
        //     //     // ...
        //     //
        //     //     return Ok("Payment successful.");
        //     // }
        //     // else
        //     // {
        //         try
        //         {
        //             if (!paymentDto.Passengers.Any())
        //             {
        //                 return BadRequest("The list of passengers is null or empty.");
        //             }
        //
        //             if (paymentDto.Passengers.Count != paymentDto.Tickets.Count)
        //             {
        //                 return BadRequest("Mismatched number of passengers and tickets.");
        //             }
        //
        //             for (int i = 0; i < paymentDto.Passengers.Count; i++)
        //             {
        //                 var passengerData = paymentDto.Passengers[i];
        //                 var ticketData = paymentDto.Tickets[i];
        //
        //                 var passengerToAdd = new Passenger
        //                 {
        //                     FullName = passengerData.FullName,
        //                     CardId = passengerData.CardId,
        //                     Gender = passengerData.Gender,
        //                     Age = passengerData.Age,
        //                     Phone = passengerData.Phone,
        //                     Email = passengerData.Email
        //                 };
        //
        //                 var passenger = await _bookingService.AddPassengerAsync(passengerToAdd);
        //
        //                 var ticket = new Ticket
        //                 {
        //                     PassengerId = passenger.Id,
        //                     TrainId = paymentDto.TrainId,
        //                     CarriageId = ticketData.CarriageId,
        //                     SeatId = ticketData.SeatId,
        //                     ScheduleId = paymentDto.ScheduleId,
        //                     PaymentId = paymentDto.PaymentId
        //                 };
        //
        //                 await _ticketService.AddAsync(ticket);
        //             }
        //
        //             return Ok("Payment successful.");
        //         }
        //         catch (Exception ex)
        //         {
        //             return BadRequest($"Error processing payment: {ex.Message}");
        //         }
        //     // }
        // }
    }
}