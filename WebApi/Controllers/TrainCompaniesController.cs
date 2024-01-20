using Domain.Exceptions;

namespace WebApi.Controllers;

public class TrainCompaniesController : BaseApiController
{
    private readonly ITrainCompanyService _trainCompanySerivce;
    private readonly IWebHostEnvironment _hostEnvironment;

    public TrainCompaniesController(ITrainCompanyService trainCompanySerivce,
                                    IWebHostEnvironment hostEnvironment)
    {
        _trainCompanySerivce = trainCompanySerivce;
        _hostEnvironment = hostEnvironment;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrainCompanyDto>>> GetTrainCompanies(
        [FromQuery] QueryParams queryParams)
    {
        var trainCompaniesDto = await _trainCompanySerivce.GetAllDtoAsync(queryParams);

        var paginationHeader = new PaginationHeader(
            trainCompaniesDto.CurrentPage, trainCompaniesDto.PageSize,
            trainCompaniesDto.TotalCount, trainCompaniesDto.TotalPages);

        Response.AddPaginationHeader(paginationHeader);

        return Ok(trainCompaniesDto);
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<TrainCompanyDto>>> GetAllTrainCompanies()
    {
        var trainCompaniesDto = await _trainCompanySerivce.GetAllDtoNoPagingAsync();

        return Ok(trainCompaniesDto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TrainCompanyDto>> GetTrainCompany(int id)
    {
        var trainCompanies = await _trainCompanySerivce.GetDtoByIdAsync(id);

        if (trainCompanies is null)
            return NotFound(new ErrorResponse(404));

        return Ok(trainCompanies);
    }

    [HttpPost]
    public async Task<ActionResult> PostTrainCompany([FromBody] TrainCompany trainCompany)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            await _trainCompanySerivce.AddAsync(trainCompany);
        }
        catch (BadRequestException ex)
        {
            var errorResponse = new ValidateInputError(400, new List<string> { ex.Message });
            return BadRequest(errorResponse);
        }

        return CreatedAtAction("GetTrainCompany", new { id = trainCompany.Id }, trainCompany);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutTrainCompany(int id, [FromBody] TrainCompany trainCompany)
    {
        if (id != trainCompany.Id) return BadRequest(new ErrorResponse(400));

        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            await _trainCompanySerivce.UpdateAsync(trainCompany);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, ex.Message));
        }

        return NoContent();
    }


    [HttpPatch("{id}")]
    public async Task<ActionResult> SoftDeleteTrainCompany(int id)
    {
        var trainCompany = await _trainCompanySerivce.GetByIdAsync(id);

        if (trainCompany is null) return NotFound(new ErrorResponse(404));

        await _trainCompanySerivce.SoftDeleteAsync(trainCompany);

        return NoContent();
    }


    [HttpPost("create")]
    public async Task<ActionResult> CreateTrainCompany([FromForm] TrainCompany trainCompany, [FromForm] IFormFile logo)
    {  
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var wwwRootPath = _hostEnvironment.WebRootPath;
            if (logo != null)
            {
                var logoName = Guid.NewGuid() + Path.GetExtension(logo.FileName);

                var logoPath = Path.Combine(wwwRootPath, "Images/TrainLogo");

                if (!Directory.Exists(logoPath))
                {
                    Directory.CreateDirectory(logoPath);
                }

                await using (var fileStream = new FileStream(Path.Combine(logoPath, logoName), FileMode.Create))
                {
                    await logo.CopyToAsync(fileStream);
                }
                trainCompany.Logo = logoName;
                await _trainCompanySerivce.AddAsync(trainCompany);

            }
            else
            {
                trainCompany.Logo = "default.jpg";
                await _trainCompanySerivce.AddAsync(trainCompany);
            }
        }
        catch (BadRequestException ex)
        {
            var errorResponse = new ValidateInputError(400, new List<string> { ex.Message });
            return BadRequest(errorResponse);
        }

        return CreatedAtAction("GetTrainCompany", new { id = trainCompany.Id }, trainCompany);
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateTrainCompany(int id, [FromForm] TrainCompany trainCompany, [FromForm] IFormFile logo)
    {
        if (id != trainCompany.Id) return BadRequest(new ErrorResponse(400));

        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var existingTrainCompany = await _trainCompanySerivce.GetByIdAsync(id);

            if (existingTrainCompany == null)
            {
                return NotFound();
            }

            existingTrainCompany.Name = trainCompany.Name;
            existingTrainCompany.Status = trainCompany.Status;

            var wwwRootPath = _hostEnvironment.WebRootPath;

            if (logo != null)
            {
                if (existingTrainCompany.Logo != null && !existingTrainCompany.Logo.Contains("http"))
                {
                    var imagePath = Path.Combine(wwwRootPath, "Images", existingTrainCompany.Logo);
                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);
                }
                var logoName = Guid.NewGuid() + Path.GetExtension(logo.FileName);

                var logoPath = Path.Combine(wwwRootPath, "Images/TrainLogo");

                if (!Directory.Exists(logoPath))
                {
                    Directory.CreateDirectory(logoPath);
                }

                await using (var fileStream = new FileStream(Path.Combine(logoPath, logoName), FileMode.Create))
                {
                    await logo.CopyToAsync(fileStream);
                }

                existingTrainCompany.Logo = logoName;
            }

            await _trainCompanySerivce.UpdateAsync(existingTrainCompany);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, ex.Message));
        }

        return NoContent();

    }

}