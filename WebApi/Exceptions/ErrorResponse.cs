namespace WebApi.Exceptions;

public class ErrorResponse
{
    public int StatusCode { get; set; }

    public string Message { get; set; }

    public ErrorResponse(int statusCode, string message = null)
    {
        StatusCode = statusCode;
        Message = message ?? GetDefaultMessageForStatusCode(statusCode);
    }

    private string GetDefaultMessageForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            400 => "A bad request from client",
            401 => "You are not authorized",
            404 => "Resource not found",
            500 => "Server error",
            _ => null
        };
    }
}