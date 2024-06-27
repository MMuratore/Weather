namespace Weather.SharedKernel.Exception;

public abstract class DomainException : System.Exception
{
    public DomainException()
    {
    }

    public DomainException(string message) : base(message)
    {
    }

    public DomainException(string message, System.Exception inner) : base(message, inner)
    {
    }
}