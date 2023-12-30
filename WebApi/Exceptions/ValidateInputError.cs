namespace WebApi.Exceptions;

public class ValidateInputError : ErrorResponse
{
    public IEnumerable<string> Errors { get; set; }

    public ValidateInputError(int statusCode, IEnumerable<string> errors) : base(statusCode)
    {
        Errors = errors;
    }

    public ValidateInputError(int statusCode, string error) : base(statusCode)
    {
        Errors = new List<string> { error };
    }
}