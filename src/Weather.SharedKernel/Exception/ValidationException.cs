namespace Weather.SharedKernel.Exception;

public sealed class ValidationException : DomainException
{
    public ValidationException()
    {
    }

    public ValidationException(string message) : base(message)
    {
    }

    public ValidationException(string message, System.Exception inner) : base(message, inner)
    {
    }
}