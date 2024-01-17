
using Application.Services;

namespace WebApi.Controllers
{
    public class TemplateController : BaseApiController
    {
        private readonly TemplateService<CarriageTemplate> _carriageTemplateService;
        private readonly TemplateService<CompartmentTemplate> _compartmentTemplateService;

        public TemplateController(TemplateService<CarriageTemplate> carriageTemplateService,
                                    TemplateService<CompartmentTemplate> compartmentTemplateService)
        {
            _carriageTemplateService = carriageTemplateService;
            _compartmentTemplateService = compartmentTemplateService;
        }

         [HttpGet("carriage")]
        public async Task<ActionResult> GetAllCarriages()
        {
            try
            {
                var carriageItems = await _carriageTemplateService.GetAllCarriageTemplateDtoAsync();
                return Ok(carriageItems);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("compartment")]
        public async Task<ActionResult> GetAllCompartments()
        {
            try
            {
                var compartmentItems = await _compartmentTemplateService.GetAllCompartmentTemplateDtoAsync();
                return Ok(compartmentItems);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }
        
    }
}