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

        [HttpGet("{code}/{email}")]
        public async Task<ActionResult<TicketDto>> GetTicketByCodeAndEmail(string code, string email)
        {
            var ticketDto = await _lookUpService.GetByCodeAndEmailAsync(code, email);

            if (ticketDto is null) return BadRequest(new ValidateInputError(400, "Ticket not found"));

            return Ok(ticketDto);
        }
    }
}