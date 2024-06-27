namespace Weather.SharedKernel.Exception;

public sealed class NotFoundException : DomainException
{
    public NotFoundException()
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string message, System.Exception inner) : base(message, inner)
    {
    }
}