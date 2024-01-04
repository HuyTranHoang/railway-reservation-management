namespace Domain.Exceptions;

public class BadRequestException : Exception
{
    public int StatusCode { get; private set; }

    public BadRequestException(int statusCode, string message)
        : base(message)
    {
        StatusCode = statusCode;
    }
}