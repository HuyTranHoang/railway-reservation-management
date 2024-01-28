using System.Globalization;
using System.Security.Claims;
using System.Text;
using Application.Common.Models.Authentication;
using Application.Services;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    public class BookingController : BaseApiController
    {
        private readonly IBookingService _bookingService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly EmailService _emailService;
        private readonly IScheduleService _scheduleService;
        private readonly ICarriageService _carriageService;
        private readonly ICompartmentService _compartmentService;
        private readonly ISeatService _seatService;
        private readonly ITicketService _ticketService;

        public BookingController(IBookingService bookingService,
            UserManager<ApplicationUser> userManager,
            IConfiguration config,
            IWebHostEnvironment hostingEnvironment,
            EmailService emailService,
            IScheduleService scheduleService,
            ICarriageService carriageService,
            ICompartmentService compartmentService,
            ISeatService seatService,
            ITicketService ticketService)
        {
            _bookingService = bookingService;
            _userManager = userManager;
            _config = config;
            _hostingEnvironment = hostingEnvironment;
            _emailService = emailService;
            _scheduleService = scheduleService;
            _carriageService = carriageService;
            _compartmentService = compartmentService;
            _seatService = seatService;
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

            var listTicketEmailDto = new List<TicketEmailDto>();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!paymentDto.Passengers.Any())
            {
                return BadRequest(new ErrorResponse(400, "The list of passengers is null or empty."));
            }

            if (paymentDto.Passengers.Count != paymentDto.Tickets.Count && !paymentDto.IsRoundTrip)
            {
                return BadRequest(new ErrorResponse(400, "Mismatched number of passengers and tickets."));
            }

            if (paymentDto.Passengers.Count != paymentDto.Tickets.Count / 2 && paymentDto.IsRoundTrip)
            {
                return BadRequest(new ErrorResponse(400, "Mismatched number of passengers and tickets."));
            }

            // Thêm vé cho lịch trình khởi hành
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
                    TrainId = paymentDto.TrainId[0],
                    CarriageId = ticketData.CarriageId,
                    SeatId = ticketData.SeatId,
                    ScheduleId = paymentDto.ScheduleId[0],
                    PaymentId = paymentDto.PaymentId
                };

                await _ticketService.AddAsync(ticket);

                // Thông tin cần để gửi email
                var schedule = await _scheduleService.GetDtoByIdAsync(paymentDto.ScheduleId[0]);
                var carriage = await _carriageService.GetDtoByIdAsync(ticketData.CarriageId);
                var seat = await _seatService.GetDtoByIdAsync(ticketData.SeatId);
                var ticketEmailDto = new TicketEmailDto()
                {
                    DepartureStationName = schedule.DepartureStationName,
                    ArrivalStationName = schedule.ArrivalStationName,
                    DepartureDate = schedule.DepartureTime.ToString("dd/MM/yyyy"),
                    DepartureTime = schedule.DepartureTime.ToString("HH:mm"),
                    TicketCode = ticket.Code,
                    TrainName = schedule.TrainName,
                    PassengerName = passengerData.FullName,
                    PassengerCardId = passengerData.CardId,
                    CarriageTypeName = carriage.CarriageTypeName,
                    CompartmentName = seat.CompartmentName,
                    SeatName = seat.Name,
                    Price = ticket.Price
                };

                listTicketEmailDto.Add(ticketEmailDto);
            }

            // Nếu có roundtrip thì thêm vé cho lịch trình trở về, cùng thông tin hành khách
            if (paymentDto.IsRoundTrip)
            {
                var totalTicket = paymentDto.Tickets.Count;
                for (int i = totalTicket / 2; i < totalTicket; i++)
                {
                    var passengerData = paymentDto.Passengers[i - totalTicket / 2];
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
                        TrainId = paymentDto.TrainId[1],
                        CarriageId = ticketData.CarriageId,
                        SeatId = ticketData.SeatId,
                        ScheduleId = paymentDto.ScheduleId[1],
                        PaymentId = paymentDto.PaymentId
                    };

                    await _ticketService.AddAsync(ticket);

                    // Thông tin cần để gửi email
                    var schedule = await _scheduleService.GetDtoByIdAsync(paymentDto.ScheduleId[1]);
                    var carriage = await _carriageService.GetDtoByIdAsync(ticketData.CarriageId);
                    var seat = await _seatService.GetDtoByIdAsync(ticketData.SeatId);
                    var ticketEmailDto = new TicketEmailDto()
                    {
                        DepartureStationName = schedule.DepartureStationName,
                        ArrivalStationName = schedule.ArrivalStationName,
                        DepartureDate = schedule.DepartureTime.ToString("dd/MM/yyyy"),
                        DepartureTime = schedule.DepartureTime.ToString("HH:mm"),
                        TicketCode = ticket.Code,
                        TrainName = schedule.TrainName,
                        PassengerName = passengerData.FullName,
                        PassengerCardId = passengerData.CardId,
                        CarriageTypeName = carriage.CarriageTypeName,
                        CompartmentName = seat.CompartmentName,
                        SeatName = seat.Name,
                        Price = ticket.Price
                    };

                    listTicketEmailDto.Add(ticketEmailDto);
                }
            }

            var sendEmailResult = await SendTicketEmail(user, listTicketEmailDto);

            if (!sendEmailResult)
            {
                return BadRequest("Error sending email.");
            }

            return Ok(new JsonResult(new { message = "Payment successful." }));
        }

        private async Task<bool> SendTicketEmail(ApplicationUser user, List<TicketEmailDto> listTicketEmailDto)
        {
            // Lấy mẫu ticket không trước
            var emailTicketBuilder = new StringBuilder();
            var ticketTemplatePath = Path.Combine(_hostingEnvironment.WebRootPath, "ticket_template.html");

            foreach (var item in listTicketEmailDto)
            {
                using (var ticketReader = new StreamReader(ticketTemplatePath))
                {
                    var ticketTemplate = await ticketReader.ReadToEndAsync();
                    ticketTemplate = ticketTemplate.Replace("{DepartureStation}", item.DepartureStationName);
                    ticketTemplate = ticketTemplate.Replace("{ArrivalStation}", item.ArrivalStationName);
                    ticketTemplate = ticketTemplate.Replace("{TicketCode}", item.TicketCode);
                    ticketTemplate = ticketTemplate.Replace("{DepartureDate}", item.DepartureDate);
                    ticketTemplate = ticketTemplate.Replace("{DepartureTime}", item.DepartureTime);
                    ticketTemplate = ticketTemplate.Replace("{TrainName}", item.TrainName);
                    ticketTemplate = ticketTemplate.Replace("{SeatName}", item.SeatName);
                    ticketTemplate = ticketTemplate.Replace("{CarriageTypeName}", item.CarriageTypeName);
                    ticketTemplate = ticketTemplate.Replace("{CompartmentName}", item.CompartmentName);
                    ticketTemplate = ticketTemplate.Replace("{PassengerName}", item.PassengerName);
                    ticketTemplate = ticketTemplate.Replace("{PassengerCardId}", item.PassengerCardId);
                    ticketTemplate = ticketTemplate.Replace("{Price}",
                        item.Price.ToString(CultureInfo.InvariantCulture) + " VND");
                    emailTicketBuilder.AppendLine(ticketTemplate);
                }
            }

            // Lấy mẫu email to và chèn ticket template vào
            var templatePath = Path.Combine(_hostingEnvironment.WebRootPath, "e_ticket_template.html");
            using (var reader = new StreamReader(templatePath))
            {
                var emailTemplate = await reader.ReadToEndAsync();

                emailTemplate = emailTemplate.Replace("{FirstName}",
                    CultureInfo.CurrentCulture.TextInfo.ToTitleCase(user.FirstName));
                emailTemplate = emailTemplate.Replace("{LastName}",
                    CultureInfo.CurrentCulture.TextInfo.ToTitleCase(user.LastName));
                emailTemplate = emailTemplate.Replace("{ApplicationName}", _config["Email:ApplicationName"]);
                emailTemplate = emailTemplate.Replace("{Ticket}", emailTicketBuilder.ToString());

                var emailSend = new EmailSendDto(user.Email, "Your Confirmed Train Tickets", emailTemplate);

                return await _emailService.SendEmailAsync(emailSend);
            }
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