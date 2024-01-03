using Application.Common.Exceptions;
using Application.Common.Interfaces.Services;
using Application.Common.Models;
using Application.Common.Models.Pagination;
using Application.Common.Models.QueryParams;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi.Exceptions;
using WebApi.Extensions;

namespace WebApi.Controllers;

public class TrainsController : BaseApiController
{
    private readonly ITrainService _trainService;

    public TrainsController(ITrainService trainService)
    {
        _trainService = trainService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrainDto>>> GetTrains([FromQuery] TrainQueryParams queryParams)
    {
        var trainsDto = await _trainService.GetAllTrainDtoAsync(queryParams);

        var paginationHeader = new PaginationHeader(queryParams.PageNumber, queryParams.PageSize,
            trainsDto.TotalCount, trainsDto.TotalPages);

        Response.AddPaginationHeader(paginationHeader);

        return Ok(trainsDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TrainDto>> GetTrain(int id)
    {
        var trains = await _trainService.GetTrainDtoByIdAsync(id);

        if (trains is null) return NotFound(new ErrorResponse(404));

        return Ok(trains);
    }

    [HttpPost]
    public async Task<IActionResult> PostTrain([FromBody] Train train)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _trainService.AddTrainAsync(train);
        }
        catch (BadRequestException ex)
        {
            var errorResponse = new ValidateInputError(400, new List<string> { ex.Message });
            return BadRequest(errorResponse);
        }

        return CreatedAtAction("GetTrain", new { id = train.Id }, train);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTrainCompany(int id, [FromBody] Train train)
    {
        if (id != train.Id) return BadRequest(new ErrorResponse(400));

        try
        {
            await _trainService.UpdateTrainAsync(train);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, ex.Message));
        }

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> SoftDeleteTrain(int id)
    {
        var train = await _trainService.GetTrainByIdAsync(id);
        if (train is null) return NotFound(new ErrorResponse(404));

        await _trainService.SoftDeleteTrainAsync(train);

        return NoContent();
    }
}