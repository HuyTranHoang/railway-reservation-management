using System.Globalization;
using System.Security.Claims;
using System.Text;
using Application.Common.Models.Authentication;
using Application.Services;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

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
            var schedulesDto = await _bookingService.GetBookingInfoWithScheduleAsync(queryParams);
            return Ok(schedulesDto);
        }

        [HttpGet("schedule/{id}")]
        public async Task<ActionResult<List<object>>> GetTrainDetailsByScheduleId(int id)
        {
            // var schedule = await _bookingService.GetBookingInfoWithScheduleIdAsync(id);
            var train = await _bookingService.GetTrainDetailsByScheduleIdAsync(id);

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


            try
            {
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


                        await _ticketService.AddRoundTripAsync(ticket);

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
            }
            catch (BadRequestException)
            {
                return BadRequest(new ErrorResponse(400, "Invalid data."));
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
    }
}