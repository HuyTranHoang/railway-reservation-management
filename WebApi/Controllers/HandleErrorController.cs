using Domain.Exceptions;

namespace WebApi.Controllers;

[ApiController]
[Route("/errors/{code:int}")]
public class HandleErrorController : ControllerBase
{
    public ActionResult Error(int code)
    {
        return new ObjectResult(new ErrorResponse(code));
    }
}