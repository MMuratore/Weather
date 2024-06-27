namespace Weather.SharedKernel.Exception;

public sealed class ForbiddenException : DomainException
{
    public ForbiddenException()
    {
    }

    public ForbiddenException(string message) : base(message)
    {
    }

    public ForbiddenException(string message, System.Exception inner) : base(message, inner)
    {
    }
}