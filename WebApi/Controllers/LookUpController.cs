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

            if (ticketDto is null) return NotFound(new ErrorResponse(404));

            return Ok(ticketDto);
        }
    }
}