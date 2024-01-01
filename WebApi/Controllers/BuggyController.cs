using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Exceptions;

namespace WebApi.Controllers;

public class BuggyController : BaseApiController
{
    private readonly ApplicationDbContext _context;

    public BuggyController(ApplicationDbContext context)
    {
        _context = context;
    }

    // 404 page not found -- request to a non-existent endpoint


    // 401 unauthorized
    [Authorize]
    [HttpGet("auth")]
    public ActionResult<string> GetSecret()
    {
        return "secret text";
    }

    // 400 bad request
    [HttpGet("bad-request")]
    public IActionResult GetBadRequest()
    {
        return BadRequest(new ErrorResponse(400));
    }

    // 400 validation error -- input string in the id field
    [HttpGet("bad-request/{id}")]
    public IActionResult GetValidationError(int id)
    {
        return Ok();
    }

    // 500 server error
    [HttpGet("server-error")]
    public async Task<IActionResult> GetServerError()
    {
        var thing = await _context.Passengers.FindAsync(9999);
        var thingToReturn = thing.ToString();
        return Ok();
    }
}