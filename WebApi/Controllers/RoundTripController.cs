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

        [HttpGet("trainCompany/{id}")]
        public async Task<ActionResult<IEnumerable<RoundTripDto>>> GetRoundTripsByTrainCompanyId(int id)
        {
            var roundTrips = await _roundTripService.GetDtoByTrainCompanyIdAsync(id);

            if (roundTrips is null)
            {
                return Ok(new RoundTripDto
                {
                    Discount = 0,
                });
            }

            return Ok(roundTrips);
        }

        [HttpPost]
        public async Task<ActionResult> AddRoundTrip([FromBody] RoundTrip roundTrip)
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
        public async Task<ActionResult> UpdateRoundTrip(int id, [FromBody] RoundTrip roundTrip)
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
        public async Task<ActionResult> SoftDeleteRoundTrip(int id)
        {
            var roundTrip = await _roundTripService.GetByIdAsync(id);
            if (roundTrip is null) return NotFound(new ErrorResponse(404));

            await _roundTripService.SoftDeleteAsync(roundTrip);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRoundTrip(int id)
        {
            var roundTrip = await _roundTripService.GetByIdAsync(id);

            if (roundTrip == null) return NotFound(new ErrorResponse(404));

            await _roundTripService.DeleteAsync(roundTrip);

            return NoContent();
        }
    }
}