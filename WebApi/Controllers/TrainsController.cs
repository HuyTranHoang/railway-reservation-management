using Application.Common.Interfaces.Services;
using Application.Common.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Exceptions;

namespace WebApi.Controllers;

public class TrainsController : BaseApiController
{
    private readonly ITrainService _trainService;

    public TrainsController(ITrainService trainService)
    {
        _trainService = trainService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrainDto>>> GetTrains()
    {
        var trainsDto = await _trainService.GetAllTrainDtoAsync();
        return Ok(trainsDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TrainDto>> GetTrain(int id)
    {
        var trains = await _trainService.GetTrainDtoByIdAsync(id);

        if (trains == null) return NotFound(new ErrorResponse(404));

        return Ok(trains);
    }

    [HttpPost]
    public async Task<IActionResult> PostTrain([FromBody] Train train)
    {
        await _trainService.AddTrainAsync(train);
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
        catch (DbUpdateConcurrencyException)
        {
            if (!trainsExists(id))
                return NotFound(new ErrorResponse(404));
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrain(int id)
    {
        var train = await _trainService.GetTrainByIdAsync(id);
        if (train == null) return NotFound(new ErrorResponse(404));

        await _trainService.SoftDeleteTrainAsync(train);

        return NoContent();
    }

    private bool trainsExists(int id)
    {
        return _trainService.GetTrainByIdAsync(id) != null;
    }
}