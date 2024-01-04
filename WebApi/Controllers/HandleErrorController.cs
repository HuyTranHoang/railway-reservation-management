using Domain.Exceptions;

namespace WebApi.Controllers;

[ApiController]
[Route("/errors/{code:int}")]
public class HandleErrorController : ControllerBase
{
    public IActionResult Error(int code)
    {
        return new ObjectResult(new ErrorResponse(code));
    }
}