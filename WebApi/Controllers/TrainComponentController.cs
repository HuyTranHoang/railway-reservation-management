using Domain.Exceptions;

namespace WebApi.Controllers
{
    public class TrainComponentController : BaseApiController
    {
        private readonly ITrainComponentService _trainComponentService;
        private readonly ICarriageService _carriageService;
        private readonly ICompartmentService _compartmentService;
        private readonly ISeatService _seatService;

        public TrainComponentController(ITrainComponentService trainComponentService,
                                        ICarriageService carriageService,
                                        ICompartmentService compartmentService,
                                        ISeatService seatService)
        {
            _trainComponentService = trainComponentService;
            _carriageService = carriageService;
            _compartmentService = compartmentService;
            _seatService = seatService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetTrainComponents(int id)
        {
            try
            {
                var carriage = await _carriageService.GetByIdAsync(id);

                if (carriage == null)
                {
                    return NotFound();
                }

                var compartments = await _compartmentService.GetCompartmentsByCarriageIdAsync(id);

                var result = new
                {
                    Carriage = carriage,
                    Compartments = compartments
                };

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("update-carriage/{currentCarriageId}")]
        public async Task<ActionResult> UpdateCarriage([FromBody] Carriage carriage, int currentCarriageId)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            
            try
            {
                await _trainComponentService.UpdateTrainComponentsAsync(carriage, currentCarriageId);
            }
            catch (BadRequestException ex)
            {
                var errorResponse = new ValidateInputError(400, new List<string> { ex.Message });
                return BadRequest(errorResponse);
            }

            return Ok(new JsonResult(new {message = "Success", id = carriage.Id }));
        }

    }
}