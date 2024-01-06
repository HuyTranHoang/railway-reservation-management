using Domain.Exceptions;

namespace WebApi.Controllers
{
    public class RoundTripController : BaseApiController
    {
        private readonly IRoundTripService _roundTripService;

        public RoundTripController(IRoundTripService roundTripService)
        {
            _roundTripService = roundTripService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoundTripDto>>> GetRoundTrips([FromQuery] RoundTripParams queryParams)
        {
            var roundTripDto = await _roundTripService.GetAllDtoAsync(queryParams);

            var paginationHeader = new PaginationHeader(queryParams.PageNumber, queryParams.PageSize,
                roundTripDto.TotalCount, roundTripDto.TotalPages);

            Response.AddPaginationHeader(paginationHeader);

            return Ok(roundTripDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoundTripDto>> GetRoundTrip(int id)
        {
            var roundTrips = await _roundTripService.GetDtoByIdAsync(id);

            if (roundTrips is null) return NotFound(new ErrorResponse(404));

            return Ok(roundTrips);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoundTrip([FromBody] RoundTrip roundTrip)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _roundTripService.AddAsync(roundTrip);
            }
            catch (BadRequestException ex)
            {
                var errorResponse = new ValidateInputError(400, new List<string> { ex.Message });
                return BadRequest(errorResponse);
            }

            return CreatedAtAction("GetRoundTrip", new { id = roundTrip.Id }, roundTrip);
        }
    
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoundTrip(int id, [FromBody] RoundTrip roundTrip)
        {
            if (id != roundTrip.Id) return BadRequest(new ErrorResponse(400));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _roundTripService.UpdateAsync(roundTrip);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ErrorResponse(404, ex.Message));
            }
            catch (BadRequestException ex)
            {
                var errorResponse = new ValidateInputError(400, new List<string> { ex.Message });
                return BadRequest(errorResponse);
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> SoftDeleteRoundTrip(int id)
        {
            var train = await _roundTripService.GetByIdAsync(id);
            if (train is null) return NotFound(new ErrorResponse(404));

            await _roundTripService.SoftDeleteAsync(train);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoundTrip(int id)
        {
            var passenger = await _roundTripService.GetByIdAsync(id);

            if (passenger == null) return NotFound(new ErrorResponse(404));

            await _roundTripService.DeleteAsync(passenger);

            return NoContent();
        }
    }
}