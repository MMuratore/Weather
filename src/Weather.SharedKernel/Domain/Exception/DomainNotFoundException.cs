namespace Weather.SharedKernel.Domain.Exception;

public sealed class DomainNotFoundException : DomainException
{
    public DomainNotFoundException()
    {
    }

    public DomainNotFoundException(string code) : base(code)
    {
        Code = code;
    }
    
    public DomainNotFoundException(string property, string code) : base(property, code)
    {
        Property = property;
        Code = code;
    }

    public DomainNotFoundException(string property, string code, System.Exception inner) : base(property, code, inner)
    {
        Property = property;
        Code = code;
    }
}