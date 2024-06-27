namespace Weather.SharedKernel.Exception;

public sealed class ConflictException : DomainException
{
    public ConflictException()
    {
    }

    public ConflictException(string message) : base(message)
    {
    }

    public ConflictException(string message, System.Exception inner) : base(message, inner)
    {
    }
}