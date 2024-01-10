using Domain.Exceptions;

namespace WebApi.Controllers
{
    public class CancellationController : BaseApiController
    {
        private readonly ICancellationService _cancellationService;

        public CancellationController(ICancellationService cancellationService)
        {
            _cancellationService = cancellationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CancellationDto>>> GetCancellationss([FromQuery] CancellationQueryParams queryParams)
        {
            var cancellationsDto = await _cancellationService.GetAllDtoAsync(queryParams);

            var paginationHeader = new PaginationHeader(queryParams.PageNumber, queryParams.PageSize, 
            cancellationsDto.TotalCount, cancellationsDto.TotalPages);

            Response.AddPaginationHeader(paginationHeader);

            return Ok(cancellationsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CancellationDto>> GetCancellation(int id)
        {
            var cancellationsDto = await _cancellationService.GetDtoByIdAsync(id);

            if (cancellationsDto is null) return NotFound(new ErrorResponse(404));

            return Ok(cancellationsDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddCancellation([FromBody] Cancellation cancellation)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _cancellationService.AddAsync(cancellation);
            }
            catch (BadRequestException ex)
            {
                var errorResponse = new ValidateInputError(400, new List<string> { ex.Message });
                return BadRequest(errorResponse);
            }

            return CreatedAtAction("GetCancellation", new { id = cancellation.Id }, cancellation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCancellation(int id, [FromBody] Cancellation cancellation)
        {
            if (id != cancellation.Id) return BadRequest(new ErrorResponse(400));

            if(!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _cancellationService.UpdateAsync(cancellation);
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
        public async Task<IActionResult> SoftDeleteCancellation(int id)
        {
            var cancellation = await _cancellationService.GetByIdAsync(id);

            if(cancellation == null) return NotFound(new ErrorResponse(404));

            await _cancellationService.SoftDeleteAsync(cancellation);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCancellation(int id)
        {
            var ticket = await _cancellationService.GetByIdAsync(id);

            if (ticket == null) return NotFound(new ErrorResponse(404));

            await _cancellationService.DeleteAsync(ticket);

            return NoContent();
        }
    }
}