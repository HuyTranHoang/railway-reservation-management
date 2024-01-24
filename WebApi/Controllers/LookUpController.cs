using Domain.Exceptions;

namespace WebApi.Controllers
{
    public class LookUpController : BaseApiController
    {
        private readonly ILookUpService _lookUpService;

        public LookUpController(ILookUpService lookUpService)
        {
            _lookUpService = lookUpService;
        }

        [HttpGet("{code}/{phone}")]
        public async Task<ActionResult<TicketDto>> GetTicketByCodeAndPhone(string code, string phone)
        {
            var ticketDto = await _lookUpService.GetByCodeAndPhoneAsync(code, phone);

            if (ticketDto is null) return NotFound(new ErrorResponse(404));

            return Ok(ticketDto);
        }
    }
}