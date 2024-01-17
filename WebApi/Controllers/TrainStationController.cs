using Domain.Exceptions;

namespace WebApi.Controllers
{
    public class TrainStationController : BaseApiController
    {
        private readonly ITrainStationService _trainStationService;

        public TrainStationController(ITrainStationService trainStationService)
        {
            _trainStationService = trainStationService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TrainStationDto>>> GetTrainStations([FromQuery] QueryParams queryParams)
        {
            var trainStationDto = await _trainStationService.GetAllDtoAsync(queryParams);

            var paginationHeader = new PaginationHeader(queryParams.PageNumber, queryParams.PageSize,
                trainStationDto.TotalCount, trainStationDto.TotalPages);

            Response.AddPaginationHeader(paginationHeader);

            return Ok(trainStationDto);
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<TrainStationDto>>> GetAllTrainStationsNoPaging()
        {
            var trainStationDtos = await _trainStationService.GetAllDtoNoPagingAsync();

            return Ok(trainStationDtos);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<TrainStationDto>> GetTrainStation(int id)
        {
            var trainStation = await _trainStationService.GetDtoByIdAsync(id);

            if (trainStation is null) return NotFound(new ErrorResponse(404));

            return Ok(trainStation);
        }

        [HttpPost]
        public async Task<ActionResult> AddTrainStation([FromBody] TrainStation trainStation)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _trainStationService.AddAsync(trainStation);
            return CreatedAtAction("GetTrainStation", new { id = trainStation.Id }, trainStation);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTrainStation(int id, [FromBody] TrainStation trainStation)
        {
            if (id != trainStation.Id) return BadRequest(new ErrorResponse(400));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _trainStationService.UpdateAsync(trainStation);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ErrorResponse(404, ex.Message));
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTrainStation(int id)
        {
            var trainStation = await _trainStationService.GetByIdAsync(id);

            if (trainStation == null) return NotFound(new ErrorResponse(404));

            await _trainStationService.DeleteAsync(trainStation);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> SoftDeleteTrainStation(int id)
        {
            var trainStation = await _trainStationService.GetByIdAsync(id);

            if (trainStation is null) return NotFound(new ErrorResponse(404));

            await _trainStationService.SoftDeleteAsync(trainStation);

            return Ok();
        }
    }
}