using Domain.Exceptions;

namespace WebApi.Controllers
{
    public class TicketController : BaseApiController
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetTickets([FromQuery] TicketQueryParams queryParams)
        {
            var ticketssDto = await _ticketService.GetAllDtoAsync(queryParams);

            var paginationHeader = new PaginationHeader(queryParams.PageNumber, queryParams.PageSize, 
            ticketssDto.TotalCount, ticketssDto.TotalPages);

            Response.AddPaginationHeader(paginationHeader);

            return Ok(ticketssDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDto>> GetTicKet(int id)
        {
            var ticketsDto = await _ticketService.GetDtoByIdAsync(id);

            if (ticketsDto is null) return NotFound(new ErrorResponse(404));

            return Ok(ticketsDto);
        }

        [HttpPost]
        public async Task<ActionResult> PostTicket([FromBody] Ticket ticket)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            
            try
            {
                await _ticketService.AddAsync(ticket);
            }
            catch (BadRequestException ex)
            {
                var errorResponse = new ValidateInputError(400, new List<string> { ex.Message });
                return BadRequest(errorResponse);
            }

            return CreatedAtAction("GetTicKet", new { id = ticket.Id }, ticket);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(int id, [FromBody] Ticket ticket)
        {
            if (id != ticket.Id) return BadRequest(new ErrorResponse(400));

            if(!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _ticketService.UpdateAsync(ticket);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ErrorResponse(404, ex.Message));
            }
            catch (BadRequestException ex)
            {
                var errorResponse = new ValidateInputError(400, new List<string> { ex.Message });
                BadRequest(errorResponse);
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> SoftDeleteTicket(int id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);

            if(ticket == null) return NotFound(new ErrorResponse(404));

            await _ticketService.SoftDeleteAsync(ticket);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);

            if (ticket == null) return NotFound(new ErrorResponse(404));

            await _ticketService.DeleteAsync(ticket);

            return NoContent();
        }
    }
}