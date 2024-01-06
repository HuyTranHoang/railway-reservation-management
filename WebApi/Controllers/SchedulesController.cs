using System.Globalization;
using Domain.Exceptions;

namespace WebApi.Controllers;

public class SchedulesController : BaseApiController
{

    private readonly IScheduleService _scheduleService;

    public SchedulesController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetSchedules([FromQuery] ScheduleQueryParams queryParams)
    {
        var schedulesDto = await _scheduleService.GetAllDtoAsync(queryParams);

        var paginationHeader = new PaginationHeader(queryParams.PageNumber, queryParams.PageSize,
            schedulesDto.TotalCount, schedulesDto.TotalPages);

        Response.AddPaginationHeader(paginationHeader);

        return Ok(schedulesDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ScheduleDto>> GetSchedule(int id)
    {
        var schedules = await _scheduleService.GetDtoByIdAsync(id);

        if (schedules is null) return NotFound(new ErrorResponse(404));

        return Ok(schedules);
    }

    [HttpPost]
    public async Task<IActionResult> PostSchedule([FromBody] Schedule schedule)
    {
        string dateString = "2024-01-07T15:00:00";
        string dateString2 = "2024-01-07T16:00:00";
        string dateString3 = "2024-01-07T15:00:00";
        DateTime dateTime = DateTime.ParseExact(dateString, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InstalledUICulture);
        DateTime dateTime2 = DateTime.ParseExact(dateString2, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InstalledUICulture);
        DateTime dateTime3 = DateTime.ParseExact(dateString3, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InstalledUICulture);
        schedule.DepartureTime = dateTime;
        schedule.ArrivalDate = dateTime2;
        schedule.DepartureDate = dateTime3;
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            await _scheduleService.AddAsync(schedule);
        }
        catch (BadRequestException ex)
        {
            var errorResponse = new ValidateInputError(400, new List<string> { ex.Message });
            return BadRequest(errorResponse);
        }

        return CreatedAtAction("Getschedule", new { id = schedule.Id }, schedule);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutSchedule(int id, [FromBody] Schedule schedule)
    {
        if (id != schedule.Id) return BadRequest(new ErrorResponse(400));

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {

            await _scheduleService.UpdateAsync(schedule);
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
    public async Task<IActionResult> SoftDeleteschedule(int id)
    {
        var schedule = await _scheduleService.GetByIdAsync(id);

        if (schedule is null) return NotFound(new ErrorResponse(404));

        await _scheduleService.SoftDeleteAsync(schedule);

        return NoContent();
    }

}
