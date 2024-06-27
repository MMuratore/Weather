namespace Weather.SharedKernel.Exception;

public sealed class UnauthorizedException : DomainException
{
    public UnauthorizedException()
    {
    }

    public UnauthorizedException(string message) : base(message)
    {
    }

    public UnauthorizedException(string message, System.Exception inner) : base(message, inner)
    {
    }
}